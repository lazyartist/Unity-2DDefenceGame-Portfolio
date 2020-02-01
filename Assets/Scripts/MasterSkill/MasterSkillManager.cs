using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasterSkillManager : SingletonBase<MasterSkillManager>
{
    public Animator ClickAnimator;

    public ProjectileAbstract Projectile_FireDrop;
    public ProjectileAbstract Projectile_RainDrop;

    public Types.MasterSkillType MasterSkillType;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;
            ClickAnimator.transform.position = clickPosition;
            ClickAnimator.SetTrigger("Click");

            ProjectileAbstract projectile = null;
            switch (MasterSkillType)
            {
                case Types.MasterSkillType.None:
                    return;
                case Types.MasterSkillType.Fire:
                    projectile = Instantiate<ProjectileAbstract>(Projectile_FireDrop, clickPosition, Quaternion.identity, transform);
                    break;
                case Types.MasterSkillType.Rain:
                    projectile = Instantiate<ProjectileAbstract>(Projectile_RainDrop, clickPosition, Quaternion.identity, transform);
                    break;
                default:
                    break;
            }

            if (projectile != null)
            {
                projectile.InitByPosition(clickPosition);
            }
        }
    }
}
