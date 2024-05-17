using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    protected float _alertTime = 0f;
    protected float _randomMaxIdleTime = 0f;
    protected float _idleTime = 0f;
    [SerializeField] protected Transform _target;
    protected Transform _currentNode;
    protected EnemyState _state = EnemyState.Initial;
    protected NavMeshAgent _agent;
    protected Vector3 _lastDestination;
    protected Vector3 _deltaPosition;
    protected HeardMark _lastHeard;
    protected SightMark _lastSight;

    protected override void Awake()
    {
        base.Awake();
        _body.constraints |= RigidbodyConstraints.FreezePosition;
        _agent = GetComponent<NavMeshAgent>();
        if (_patrolNodes.Count > 0)
        {
            _currentNode = _patrolNodes[0];
        }
    }

    protected virtual void Start()
    {
        StartCoroutine(PathFindingUpdate());
    }

    protected virtual IEnumerator PathFindingUpdate()
    {
        if (_target != null && !_target.position.Equals(_lastDestination))
        {
            _agent.SetDestination(_target.position);
            _lastDestination = _target.position;
        }
        yield return new WaitForSeconds(_pathFindingInterval);
        StartCoroutine(PathFindingUpdate());
    }
    protected override void DoMove(Vector3 relativeDirection, float maxVelocity, float acceleration)
    {
        _agent.isStopped = false;
        _agent.acceleration = acceleration;
        _agent.speed = maxVelocity;
    }

    protected override void DoStay()
    {
        _agent.isStopped = true;
    }

    protected virtual void Update()
    {
        if (_eyes.HasSight())
        {
            _state.SightUpdate(this);
        }
        else if (_ears.HasHeard())
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
        if (_eyes.HasSight())
        {
            _state.SightFixedUpdate(this);
        }
        else if (_ears.HasHeard())
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
        _deltaPosition -= transform.position;
        SetDirection(_deltaPosition);
    }

    public virtual void SetState(EnemyState state)
    {
        _state = state;
    }

    public virtual void IdleUpdate()
    {
        Calm();
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
        if (!_ears.HasHeard())
        {
            return false;
        }
        Stay();
        var sightDirection = _ears.Closer.transform.position - transform.position;
        sightDirection.y = 0;
        transform.LookAt(sightDirection);
        
        transform.LookAt(_ears.Closer.transform);
        _alertTime += _alertHeardSpeed * Time.deltaTime;
        return _alertTime >= _alertMaxTime;
    }

    public virtual bool CheckSight()
    {
        if (!_eyes.HasSight())
        {
            return false;
        }
        Stay();
        var sightDirection = _eyes.Closer.transform.position - transform.position;
        sightDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(sightDirection);
        
        _eyes.transform.LookAt(_eyes.Closer.transform);
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
        if (_ears.Closer.IsDestroyed())
        {
            return;
        }
        _ears.Closer.Keep();
        _target = _ears.Closer.transform;
        if ((_target.position - transform.position).sqrMagnitude < Mathf.Pow(1, 2))
        {
            Stay();
            _ears.Closer.gameObject.SetActive(false);
        }
        else
        {
            Walk();
        }
    }

    public virtual void Chase()
    {
        if (_lastSight.IsDestroyed())
        {
            return;
        }
        _lastSight.Keep();
        _target = _lastSight.transform;
        if ((_target.position - transform.position).sqrMagnitude < Mathf.Pow(5, 2))
        {
            Stay();
            _lastSight.gameObject.SetActive(false);
        }
        else
        {
            Run();
        }
    }

    public override void OnHear(HeardMark mark)
    {
        _lastHeard = mark;
    }

    public override void OnSight(SightMark mark)
    {
        _lastSight = mark;
    }
}
