using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile_Arrow : AProjectile
{
    public float HeightLimit = 5f;
    public float HeightLimitMultiflier = 2f;
    public float TimeToTopmostHeight = 2f;

    private Animator _animator;
    private float _elapsedTime;
    private bool _isMoving = false;
    private ParabolaAlgorithm _paralobaAlgorithm;
    private Vector3 _startPosition;
    private Vector3 _prevPosition;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _paralobaAlgorithm = new ParabolaAlgorithm();
        name += Consts.GetUnitId();
    }

    void Update()
    {
        if (_isMoving)
        {
            _elapsedTime += Time.deltaTime;

            Vector3 position = _paralobaAlgorithm.GetPosition(_elapsedTime);
            transform.position = position;

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

                // 화살이 타겟에 꽂혀있도록 맞는 순간 부모를 타겟으로 바꿔준다.
                if (_targetUnit != null && _targetUnit.IsDied == false)
                {
                    _isMoving = false;
                    _elapsedTime = 0;
                    transform.SetParent(_targetUnit.UnitBody.transform);
                    transform.position = _targetUnit.GetCenterPosition();
                    Hit();
                }
                else if (transform.position.y <= _targetBottomPosition.y)
                {
                    _isMoving = false;
                    _elapsedTime = 0;
                }
            }
        }
    }

    public override void MoveToTarget()
    {
        _startPosition = transform.position;
        _prevPosition = _startPosition;

        // 높이 보정
        // 화살의 시작위치 + HeightLimit 값 보다 타겟이 더 높이 올라가면 위치값 계산이 안되므로(NaN)
        // 타겟의 현재 위치가 HeightLimit 보다 높아지면 HeightLimit 값을 증가시켜준다.
        // 움직임이 어색해질 수 있지만 타겟이 느리다면 눈치채기 어렵다.
        float distanceY = _targetCenterPosition.y - _startPosition.y;
        float height = 0f;
        if (distanceY > HeightLimit)
        {
            // 최대 높이보다 더 위에 있는 타겟인 경우 차이만큼 더해서 보정한다.
            // 차이만큼만 더하면 타겟까지 포물선 이동은 하지만 떨어지는 느낌이 없기 때문에
            // 차이의 두 세배 해줘야한다.
            height = (distanceY - HeightLimit) * HeightLimitMultiflier;
        }

        // 현재 위치를 기준으로 궤도를 계산한다.
        _paralobaAlgorithm.Init(HeightLimit + height, TimeToTopmostHeight, _startPosition, _targetCenterPosition);
        // 공격대상이 있다면 맞는 저점을 예측하여 발사한다.
        if (_targetUnit != null)
        {
            float moveSpeed = _targetUnit.UnitData.MoveSpeed;
            float distance = moveSpeed * _paralobaAlgorithm.TimeToEndPosition;
            Vector3 guessedLastCenterPosition = _targetCenterPosition + (_targetUnit.MoveDirection.normalized * distance);

            // 높이 보정
            distanceY = guessedLastCenterPosition.y - _startPosition.y;
            height = 0f;
            if (distanceY > HeightLimit)
            {
                height = (distanceY - HeightLimit) * HeightLimitMultiflier;
            }

            // 예측한 마지막 위치로 다시 계산한다.
            _targetBottomPosition = guessedLastCenterPosition + (_targetBottomPosition - _targetCenterPosition);
            _targetCenterPosition = guessedLastCenterPosition;
            _paralobaAlgorithm.Init(HeightLimit + height, TimeToTopmostHeight, _startPosition, guessedLastCenterPosition);

            // todo 현재 목표까지의 거리와 맞은 지점까지의 거리의 차에 따라 시간을 조절한다.
        }

        _elapsedTime = 0;
        _isMoving = true;
    }

    void Hit()
    {
        AudioManager.Inst.PlayAttackHit(AttackData);
        if (_targetUnit != null)
        {
            _targetUnit.TakeDamage(AttackData);
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

        if (AttackData != null)
        {
            Gizmos.DrawWireSphere(transform.position, AttackData.AttackRange);
        }
    }
}

