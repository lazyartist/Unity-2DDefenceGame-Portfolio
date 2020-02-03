using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Archer : Unit
{
    public ProjectileAbstract Projectile;
    public Transform ProjectileSpawnPosition;
    public float AttackRange = 2.5f;

    protected new void Start()
    {
        base.Start();

        // TargetRange가 AttackRange보다 클 경우 타겟 범위에 있어서 공격 타겟으로 지정했는데
        // AttackRange에 있던 타겟이 Attack 애니 플레이 후 AttackRange에서 나가버렸고
        // 타겟 범위에 있기 때문에 Target은 이전 타겟을 지정하고 있다.
        // 따라서 공격 애니에서 Attack()을 실행할 경우 공격 범위와 충돌 범위를 같게 만들어야한다.
        //UnitAttackArea.GetComponent<CircleCollider2D>().radius = UnitData.TargetRange;
        //UnitAttackArea.GetComponent<CircleCollider2D>().radius = AttackRange;
    }

    //public override void Attack()
    //{
    //    if (AttackTargetUnit != null && AttackTargetUnit.IsDied == false)
    //    {
    //        Vector3 projectileLocalPosition = ProjectileSpawnPosition.localPosition;
    //        projectileLocalPosition.x = ProjectileSpawnPosition.position.x - UnitBody.UnitBodyContainer.transform.position.x;
    //        ProjectileAbstract projectile = Instantiate<ProjectileAbstract>(Projectile, transform.position + projectileLocalPosition, Quaternion.identity, transform);
    //        projectile.InitByTarget(AttackTargetUnit.gameObject);
    //    }
    //}

    private void OnDrawGizmos()
    {
        Vector3 projectileLocalPosition = ProjectileSpawnPosition.localPosition;
        projectileLocalPosition.x = ProjectileSpawnPosition.position.x - UnitBody.UnitBodyContainer.transform.position.x;
        Gizmos.DrawWireSphere(transform.position + projectileLocalPosition, 0.1f);

        if (AttackTargetUnit != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, AttackTargetUnit.transform.position);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("OnTriggerEnter2D Unit");
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log("OnTriggerStay2D Unit");
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    Debug.Log("OnTriggerExit2D Unit");
    //}
}
