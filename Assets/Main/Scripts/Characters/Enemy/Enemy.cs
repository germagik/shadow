using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Utils;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Character
{
    [SerializeField] protected float _pathFindingInterval = 0.25f;
    [SerializeField] protected List<Transform> _patrolNodes = new();
    [SerializeField] protected float _patrolMinIdleTime = 2f;
    [SerializeField] protected float _patrolMaxIdleTime = 5f;
    [SerializeField] protected float _patrolNodeChangeDistance = 0.1f;
    [SerializeField] protected bool _randomPatrol = false;
    [SerializeField] protected float _alertSightSpeed = 2f;
    [SerializeField] protected float _alertHeardSpeed = 1f;
    [SerializeField] protected float _alertMaxTime = 15f;
    [SerializeField] protected float _searchMaxTime = 10f;
    [SerializeField] protected float _boundedMaxTime = 15f;
    [SerializeField] protected ActionZone _weakZone;
    [SerializeField] protected GameObject _bounded;
    protected float _alertTime = 0f;
    protected float _searchTime = 0f;
    protected float _randomMaxIdleTime = 0f;
    protected float _idleTime = 0f;
    protected float _boundedTime = 0f;
    protected Transform _target;
    protected Transform _currentNode;
    protected EnemyState _state;
    protected NavMeshAgent _agent;
    protected Vector3 _lastDestination;
    protected Vector3 _deltaDirection;
    protected Vector3 _lastDirection;
    protected PerceptionMark _lastHeard;
    protected PerceptionMark _lastSight;

    public Vector3 relDir;

    protected override void Awake()
    {
        base.Awake();
        _body.constraints |= RigidbodyConstraints.FreezePosition;
        _agent = GetComponent<NavMeshAgent>();
        if (_patrolNodes.Count > 0)
        {
            _currentNode = _patrolNodes[0];
        }
        _lastDirection = transform.position;
        SetState(Unaware.Instance);
    }

    protected virtual void Start()
    {
        StartCoroutine(PathFindingRoutine());
    }

    protected virtual IEnumerator PathFindingRoutine()
    {
        if (_target != null && !_target.position.Equals(_lastDestination))
        {
            _agent.SetDestination(_target.position);
            _lastDestination = _target.position;
        }
        yield return new WaitForSeconds(_pathFindingInterval);
        StartCoroutine(PathFindingRoutine());
    }
    protected override void DoMove(Vector3 normalizedDirection, float maxVelocity, float acceleration)
    {
        _agent.isStopped = false;
        _agent.acceleration = maxVelocity;
        _agent.speed = maxVelocity;
        Vector3 relativeDirection = new Vector3(Vector3.Dot(transform.right, normalizedDirection),0, Vector3.Dot(transform.forward,normalizedDirection)).normalized;
        _animator.SetFloat(AnimatorParametersNames.DirectionX, relativeDirection.x);
        _animator.SetFloat(AnimatorParametersNames.DirectionY, relativeDirection.z);
    }

    protected override void DoStay()
    {
        _agent.isStopped = true;
    }

    protected virtual void Update()
    {
        if (_eyes.HasSight)
        {
            _state.SightUpdate(this);
        }
        else if (_ears.HasHeard)
        {
            _state.HeardUpdate(this);
        }
        else
        {
            _state.IdleUpdate(this);
        }
        _state.Update(this);
    }

    protected virtual void FixedUpdate()
    {
        PositionFixedUpdate();
        if (_eyes.HasSight)
        {
            _state.SightFixedUpdate(this);
        }
        else if (_ears.HasHeard)
        {
            _state.HeardFixedUpdate(this);
        }
        else
        {
            _state.IdleFixedUpdate(this);
        }
    }

    protected virtual void PositionFixedUpdate()
    {
        _deltaDirection = transform.position - _lastDirection;
        _deltaDirection.y = 0;
        _deltaDirection = _deltaDirection.normalized;
        _lastDirection = transform.position;
        SetDirection(_deltaDirection);
    }

    public virtual void SetState(EnemyState state)
    {
        _state?.OnOut(this);
        _state = state;
        _state.OnIn(this);
    }

    public virtual void IdleUpdate()
    {
        Calm();
        Patrol();
    }

    protected virtual void Patrol()
    {
        if (_patrolNodes.Count == 0)
        {
            return;
        }
        if (_target != _currentNode)
        {
            _target = _currentNode;
        }

        if ((_currentNode.position - transform.position).sqrMagnitude < Mathf.Pow(_patrolNodeChangeDistance, 2)) {
            if (_randomMaxIdleTime == 0)
            {
                _randomMaxIdleTime = Random.Range(_patrolMinIdleTime, _patrolMaxIdleTime);
            }
            if (_idleTime < _randomMaxIdleTime) {
                _idleTime += Time.deltaTime;
                Stay();
            }
            else
            {
                _randomMaxIdleTime = 0f;
                _idleTime = 0f;
                Transform nextNode = NextPatrolNode();
                if (nextNode != null)
                {
                    _currentNode = nextNode;
                    _target = nextNode;
                }
            }
        }
        else
        {
            Walk();
        }
    }

    protected virtual Transform NextPatrolNode()
    {
        if (_patrolNodes.Count == 1)
        {
            return _patrolNodes[0];
        }
        if (_randomPatrol)
        {
            Transform nextNode;
            do
            {
                nextNode = _patrolNodes[Random.Range(0, _patrolNodes.Count)];
            }
            while (_currentNode == nextNode);
            return nextNode;
        }
        else if (_patrolNodes.Contains(_currentNode))
        {
            int nextIndex = _patrolNodes.IndexOf(_currentNode) + 1;
            if (nextIndex < _patrolNodes.Count)
            {
                return _patrolNodes[nextIndex];
            }
        }
        if (_patrolNodes.Count > 0)
        {
            return _patrolNodes[0];
        }
        return null;
    }

    public virtual bool CheckHeard()
    {
        if (_lastHeard.IsDestroyed())
        {
            _alertTime = 0;
            return false;
        }
        Stay();
        var heardDirection = _lastHeard.transform.position - transform.position;
        heardDirection.y = 0;

        transform.LookAt(heardDirection);
        _alertTime += _alertHeardSpeed * Time.deltaTime;
        return _alertTime >= _alertMaxTime;
    }

    public virtual bool CheckSight()
    {
        if (_lastSight.IsDestroyed())
        {
            _alertTime = 0;
            return false;
        }
        Stay();
        var sightDirection = _lastSight.transform.position - transform.position;
        sightDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(sightDirection);
        
        _eyes.transform.LookAt(_lastSight.transform);
        _alertTime += _alertSightSpeed * Time.deltaTime;
        return _alertTime >= _alertMaxTime;
    }

    protected virtual void Calm()
    {
        if (_alertTime > 0)
        {
            _alertTime -= Time.deltaTime;
            if (_alertTime < 0)
            {
                _alertTime = 0;
            }
        }
    }

    public virtual void SearchHeard()
    {
        if (_lastHeard.IsDestroyed())
        {
            return;
        }
        _lastHeard.Pause();
        _target = _lastHeard.transform;
        if ((_target.position - transform.position).sqrMagnitude < Mathf.Pow(1, 2))
        {
            Stay();
            if (_searchTime < _searchMaxTime)
            {
                _searchTime += Time.deltaTime;
            }
            else
            {
                _lastHeard.gameObject.SetActive(false);
                _searchTime = 0;
            }
        }
        else
        {
            _searchTime = 0;
            Walk();
        }
    }

    public virtual void Chase()
    {
        if (_lastSight.IsDestroyed())
        {
            return;
        }
        _lastSight.Pause();
        _target = _lastSight.transform;
        if ((_target.position - transform.position).sqrMagnitude < Mathf.Pow(5, 2))
        {
            Stay();
        }
        else
        {
            Run();
        }
    }

    public override void OnHear(PerceptionMark mark)
    {
        _lastHeard = mark;
    }

    public override void OnSight(PerceptionMark mark)
    {
        _lastSight = mark;
    }

    public virtual void ActivateWeakness()
    {
        _weakZone.gameObject.SetActive(true);
    }

    public virtual void DeactivateWeakness()
    {
        _weakZone.gameObject.SetActive(false);
    }

    public virtual void OnBounded()
    {
        _boundedTime = 0f;
        _bounded.SetActive(true);
    }

    public virtual void OnUnbounded()
    {
        _boundedTime = 0f;
        _bounded.SetActive(false);
    }
    public virtual bool CheckBounded()
    {
        if (_boundedTime < _boundedMaxTime)
        {
            _boundedTime += Time.deltaTime;
            Stay();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + (Vector3.up * 2), transform.position + _direction + (Vector3.up * 2));
        Gizmos.DrawSphere(transform.position + _direction * 1.5f + (Vector3.up * 2), 0.1f);
    }
}
