using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

[RequireComponent(typeof(Collider))]
public class Eyes : Sense
{
    [SerializeField] protected LayerMask _sightLayers;
    protected List<SightPoint> _closePoints = new();
    public virtual bool HasSight
    {
        get 
        {
            var marks = _marks.Values.ToArray();
            foreach (PerceptionMark mark in marks)
            {
                if (mark.gameObject.activeSelf) {
                    return true;
                }
            }
            return false;
        }
    }


    protected virtual void FixedUpdate()
    {
        SightFixedUpdate();
    }

    protected virtual void SightFixedUpdate()
    {
        foreach (SightPoint point in _closePoints)
        {
            Vector3 direction = point.transform.position - transform.position;
            if (Physics.Raycast(new Ray(transform.position, direction), out RaycastHit hit, float.PositiveInfinity, _sightLayers))
            {
                if (hit.collider.gameObject.TryGetComponent(out SightPoint sightPoint)) {
                    OnSight(sightPoint);
                }
            }
        }
    }

    protected virtual void OnSight(SightPoint sightPoint)
    {
        SetPerceptionMarkFrom(sightPoint.From, sightPoint.transform);
        // int fromID = sightPoint.From.GetInstanceID();
        // if (fromID == _character.gameObject.GetInstanceID())
        // {
        //     return;
        // }
        // _marks.TryGetValue(fromID, out ChaseMark mark);
        // if (mark != null)
        // {
        //     if (!mark.gameObject.activeSelf)
        //     {
        //         _marks.Remove(fromID);
        //         Destroy(mark.gameObject);
        //     }
        //     else
        //     {
        //         mark.transform.position = sightPoint.From.transform.position;
        //         mark.ResetTime();
        //     }
        // }
        // else
        // {
        //     mark = Instantiate(_sightMarkPrefab, sightPoint.From.transform.position , _sightMarkPrefab.transform.rotation);
        //     mark.Initialize(sightPoint.From);
        //     _marks.Add(fromID, mark);
        //     _character.OnSight(mark);
        // }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out SightPoint sightPoint)) {
            if (!_closePoints.Contains(sightPoint))
            {
                _closePoints.Add(sightPoint);
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out SightPoint sightPoint)) {
            if (_closePoints.Contains(sightPoint))
            {
                _closePoints.Remove(sightPoint);
            }
        }
    }

    protected override void OnFirstSense(PerceptionMark mark)
    {
        _character.OnSight(mark);
    }

    protected override void OnSense(PerceptionMark mark)
    {
        
    }

    protected virtual void OnDrawGizmos()
    {
        foreach (SightPoint point in _closePoints)
        {
            Vector3 direction = point.transform.position - transform.position;
            Gizmos.DrawLine(transform.position, point.transform.position);
            if (Physics.Raycast(new Ray(transform.position, direction), out RaycastHit hit, float.PositiveInfinity, _sightLayers))
            {
                if (hit.collider.gameObject.TryGetComponent(out SightPoint sightPoint)) {
                    if (_producerLayers.Includes(sightPoint.From.layer))
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(transform.position, hit.transform.position);
                    }
                }
            }
        }
    }
}
