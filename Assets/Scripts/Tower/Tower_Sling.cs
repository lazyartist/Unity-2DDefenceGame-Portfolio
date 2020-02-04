using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tower_Sling : Tower
{
    //public Consts.TeamType TeamType;

    public ProjectileAbstract BulletPrefab;
    public GameObject BulletSpawnPosition;

    private GameObject _target;
    private Animator _animator;
    private ProjectileAbstract _bullet;

    private float _elapsedFireTime;
    private Vector3 _lastTargetPosition;

    void Start()
    {
        _animator = GetComponent<Animator>();

        _animator.StopPlayback();
    }

    void Update()
    {
        //if (a.IsName("Tower1_Fire"))
        //{
        //    //_bullet.gameObject.SetActive(true);
        //    _bullet.transform.position = BulletPlaceholder.transform.position;
        //}
        //else
        //{
        //    //_bullet.gameObject.SetActive(false);
        //}
    }

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, UnitData.TargetRange, Consts.lmUnit);
        if (colliders.Length == 0) return;

        _target = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            if (collider.tag == Consts.tagUnit)
            {
                Unit unit = collider.GetComponent<Unit>();
                _target = collider.gameObject;
                break;
            }
        }

        _elapsedFireTime += Time.fixedDeltaTime;

        if (_target == null)
        {
            return;
        }

        if (_elapsedFireTime >= UnitData.AttackCoolTime)
        {
            Fire();
            //_lastTargetPosition = Target.transform.position;
        }
    }

    public void Fire()
    {
        _elapsedFireTime = 0f;
        _animator.SetTrigger("Fire");
    }

    public void FireBullet()
    {
        if (_target != null)
        {
            _bullet = Instantiate(BulletPrefab, BulletSpawnPosition.transform.position, Quaternion.identity, BulletSpawnPosition.transform);
            //_bullet.Init(_lastTargetPosition);
            _bullet.InitByTarget(_target);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, UnitData.TargetRange);
    }

    //private void OnMouseDown()
    //{
    //    Fire();
    //}
}
