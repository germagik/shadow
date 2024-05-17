using System.Collections.Generic;
using UnityEngine;
using Utils;

[RequireComponent(typeof(Collider))]
public class Eyes : MonoBehaviour
{
    [SerializeField] protected Character _character;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected LayerMask _fromLayerMask;
    [SerializeField] protected SightMark _sightMarkPrefab;
    public SightMark Closer
    {
        get
        {
            SightMark found = null;
            var marks = _marks.Values;
            foreach (SightMark mark in marks)
            {
                if (!mark.gameObject.activeSelf)
                {
                    continue;
                }
                found ??= mark;
                float distance = Vector3.Distance(found.transform.position, _character.transform.position);
                float currentDistance = Vector3.Distance(mark.transform.position, _character.transform.position);
                if (currentDistance < distance)
                {
                    found = mark;
                }
            }
            return found;
        }
    }
    protected List<SightPoint> _closePoints = new();
    protected Dictionary<int,SightMark> _marks = new();

    protected virtual void FixedUpdate()
    {
        SightFixedUpdate();
    }
    protected virtual void SightFixedUpdate()
    {
        foreach (SightPoint point in _closePoints)
        {
            Vector3 direction = point.transform.position - transform.position;
            if (Physics.Raycast(new Ray(transform.position, direction), out RaycastHit hit, float.PositiveInfinity, _layerMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out SightPoint sightPoint)) {
                    if (_fromLayerMask.Includes(sightPoint.From.layer))
                    {
                        OnSight(sightPoint);
                    }
                }
            }
        }
    }

    protected virtual void OnSight(SightPoint sightPoint)
    {
        int fromID = sightPoint.From.GetInstanceID();
        if (fromID == _character.gameObject.GetInstanceID())
        {
            return;
        }
        _marks.TryGetValue(fromID, out SightMark mark);
        if (mark != null)
        {
            if (!mark.gameObject.activeSelf)
            {
                _marks.Remove(fromID);
                Destroy(mark.gameObject);
            }
            else
            {
                mark.transform.position = sightPoint.transform.position;
                mark.ResetTime();
                _character.OnSight(mark);
            }
        }
        else
        {
            mark = Instantiate(_sightMarkPrefab, sightPoint.From.transform.position , _sightMarkPrefab.transform.rotation);
            mark.Initialize(sightPoint.From);
            _marks.Add(fromID, mark);
            _character.OnSight(mark);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out SightPoint sightPoint)) {
            if (!_closePoints.Contains(sightPoint)) {
                _closePoints.Add(sightPoint);
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out SightPoint sightPoint)) {
            if (_closePoints.Contains(sightPoint)) {
                _closePoints.Remove(sightPoint);
            }
        }
    }

    protected virtual void OnDrawGizmos()
    {
        foreach (SightPoint point in _closePoints)
        {
            Vector3 direction = point.transform.position - transform.position;
            Gizmos.DrawLine(transform.position, point.transform.position);
            if (Physics.Raycast(new Ray(transform.position, direction), out RaycastHit hit, float.PositiveInfinity, _layerMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out SightPoint sightPoint)) {
                    if (_fromLayerMask.Includes(sightPoint.From.layer))
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(transform.position, hit.transform.position);
                    }
                }
            }
        }
    }

    public virtual bool HasSight()
    {
        var marks = _marks.Values;
        foreach (SightMark mark in marks)
        {
            if (mark.gameObject.activeSelf) {
                return true;
            }
        }
        return false;
    }

}
