using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile_Arrow : AProjectile
{
    public float HeightLimit = 5f;
    public float TimeToTopmostHeight = 2f;

    private Animator _animator;
    private float _elapsedTime;
    private bool _isMoving = false;
    private ParabolaAlgorithm _paralobaAlgorithm;
    private GameObject _target;
    private Vector3 _startPosition;
    private Vector3 _prevPosition;
    private Vector3 _lastTargetPosition;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _paralobaAlgorithm = new ParabolaAlgorithm();
    }

    void Update()
    {
        if (_isMoving)
        {
            _elapsedTime += Time.deltaTime;

            Vector3 targetPosition = _target == null ? _lastTargetPosition : _target.transform.position;

            // 화살의 시작위치 + HeightLimit 값 보다 타겟이 더 높이 올라가면 위치값 계산이 안되므로(NaN)
            // 타겟의 현재 위치가 HeightLimit 보다 높아지면 HeightLimit 값을 증가시켜준다.
            // 움직임이 어색해질 수 있지만 타겟이 느리다면 눈치채기 어렵다.
            float distanceY = targetPosition.y - _startPosition.y;
            float height = 0f;
            if (distanceY > HeightLimit)
            {
                // 최대 높이보다 더 위에 있는 타겟인 경우 차이만큼 더해서 보정한다.
                // 차이만큼만 더하면 타겟까지 포물선 이동은 하지만 떨어지는 느낌이 없기 때문에
                // 차이의 두 세배 해줘야한다.
                height = (distanceY - HeightLimit) * 1.5f;
                //HeightLimit += (distanceY - HeightLimit) * 3;
            }

            // 타겟을 따라가기 위해 매번 포물선을 다시 계산한다.
            _paralobaAlgorithm.Init(HeightLimit + height, TimeToTopmostHeight, _startPosition, targetPosition);
            //_paralobaAlgorithm.Init(HeightLimit, TimeToTopmostHeight, _startPosition, targetPosition);

            if (_elapsedTime >= _paralobaAlgorithm.TimeToEndPosition)
            {
                _elapsedTime = _paralobaAlgorithm.TimeToEndPosition;
            }

            transform.position = _paralobaAlgorithm.GetPosition(_elapsedTime);

            // Angle
            Vector3 direction = transform.position - _prevPosition;
            float rad = Mathf.Atan2(direction.y, direction.x);
            transform.rotation = Quaternion.Euler(0f, 0f, rad * Mathf.Rad2Deg);

            _prevPosition = transform.position;

            // 화살이 타겟에 도달한 순간 꽂히게 보이도록 미리 바꿔준다.
            if (_paralobaAlgorithm.TimeToEndPosition - _elapsedTime < 0.1f)
            {
                _animator.SetTrigger("Hit");
            }

            if (_elapsedTime >= _paralobaAlgorithm.TimeToEndPosition)
            {
                _isMoving = false;
                _elapsedTime = 0;

                // 화살이 타겟에 꽂혀있도록 맞는 순간 부모를 타겟으로 바꿔준다.
                if(_target != null)
                {
                    Hit();
                    transform.SetParent(_target.transform);
                }
            }
        }
    }

    override public void Init(AttackData attackData, AttackTargetData attackTargetData, GameObject target, Vector3 position)
    {
        AttackData = attackData;
        AttackTargetData = attackTargetData;
        _target = target;
        InitByPosition(position);
    }

    override public void InitByTarget(GameObject target)
    {
        _target = target;
        InitByPosition(target.transform.position);
    }

    override public void InitByPosition(Vector3 position)
    {
        _startPosition = transform.position;
        _prevPosition = _startPosition;
        _lastTargetPosition = position;

        // 최대 높이보다 더 위에 있는 타겟인 경우 차이만큼 더해서 보정한다.
        // 차이만큼만 더하면 타겟까지 포물선 이동은 하지만 떨어지는 느낌이 없기 때문에
        // 차이의 두 세배 해줘야한다.
        //float distanceY = target.transform.position.y - transform.position.y;
        //if (distanceY > HeightLimit)
        //{
        //HeightLimit += (distanceY - HeightLimit) * 3;
        //}
        //_paralobaAlgorithm.Init(HeightLimit, TimeToTopmostHeight, transform.position, target.transform.position);

        // todo 타겟의 위치가 위쪽이면 도달 시간이 빠르고 아래면 느리다.
        // 위쪽은 포물선 최고점과 가깝고 아래쪽은 최고점과 멀기 때문에 더 먼 거리를 가기 때문이다.
        // 추후 보정해야한다.
        //Debug.Log(_paralobaAlgorithm.TimeToEndPosition);

        _elapsedTime = 0;
        _isMoving = true;
    }

    void Hit()
    {
        if (_target != null)
        {
            _target.GetComponent<Unit>().TakeDamage(AttackData);
        }
    }

    void OnDrawGizmos()
    {
        if (_isMoving)
        {
            Vector3 prevPosition = _paralobaAlgorithm.GetPosition(0f);
            Vector3 curPosition;
            float frame = _paralobaAlgorithm.TimeToEndPosition / 10;

            for (int i = 1; i <= 10; i++)
            {
                float t = frame * i;
                curPosition = _paralobaAlgorithm.GetPosition(t);

                Gizmos.DrawLine(prevPosition, curPosition);

                prevPosition = curPosition;
            }
        }

        if(AttackData != null) {
            Gizmos.DrawWireSphere(transform.position, AttackData.AttackRange);
        }
    }
}

