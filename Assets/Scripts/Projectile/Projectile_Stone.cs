using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile_Stone : AProjectile
{
    public float HeightLimit = 5f;
    public float TimeToTopmostHeight = 2f;

    private Animator _animator;
    private float _elapsedTime;
    private bool _isMoving = false;
    private ParabolaAlgorithm _paralobaAlgorithm;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _paralobaAlgorithm = new ParabolaAlgorithm();
    }

    void FixedUpdate()
    {
        if (_isMoving)
        {
            _elapsedTime += Time.fixedDeltaTime;

            if (_elapsedTime >= _paralobaAlgorithm.TimeToEndPosition)
            {
                _elapsedTime = _paralobaAlgorithm.TimeToEndPosition;
            }

            transform.position = _paralobaAlgorithm.GetPosition(_elapsedTime);

            if (_elapsedTime >= _paralobaAlgorithm.TimeToEndPosition)
            {
                _animator.SetTrigger("Hit");
                Hit();
                _isMoving = false;
                _elapsedTime = 0;
            }
        }
    }

    public override void MoveToTarget()
    {
        float distanceY = _targetCenterPosition.y - transform.position.y;

        float heightLimit = HeightLimit;
        if (distanceY > HeightLimit)
        {
            // 최대 높이보다 더 위에 있는 타겟인 경우 차이만큼 더해서 보정한다.
            // 차이만큼만 더하면 타겟까지 포물선 이동은 하지만 떨어지는 느낌이 없기 때문에
            // 차이의 두 세배 해줘야한다.
            heightLimit += (distanceY - HeightLimit) * 3;
        }


        _paralobaAlgorithm.Init(heightLimit, TimeToTopmostHeight, transform.position, _targetCenterPosition);

        // todo 타겟의 위치가 위쪽이면 도달 시간이 빠르고 아래면 느리다.
        // 위쪽은 포물선 최고점과 가깝고 아래쪽은 최고점과 멀기 때문에 더 먼 거리를 가기 때문이다.
        // 추후 보정해야한다.
        //Debug.Log(_paralobaAlgorithm.TimeToEndPosition);

        _elapsedTime = 0;
        _isMoving = true;
    }

    void Hit()
    {
        AudioManager.Inst.PlayAttackHit(AttackData);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, AttackData.AttackRange, _targetLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            collider.GetComponent<Unit>().TakeDamage(AttackData);
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