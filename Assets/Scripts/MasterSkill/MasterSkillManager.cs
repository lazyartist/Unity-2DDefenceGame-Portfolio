﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasterSkillManager : SingletonBase<MasterSkillManager>
{
    public GameObject ClickContainer;
    public Animator ClickAnimator;

    public AProjectile Projectile_FireDrop;
    public AProjectile Projectile_RainDrop;

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
            ClickContainer.transform.position = clickPosition;
            ClickAnimator.SetTrigger("Click");

            AProjectile projectile = null;
            switch (MasterSkillType)
            {
                case Types.MasterSkillType.None:
                    return;
                case Types.MasterSkillType.Fire:
                    projectile = Instantiate<AProjectile>(Projectile_FireDrop, clickPosition, Quaternion.identity, transform);
                    break;
                case Types.MasterSkillType.Rain:
                    projectile = Instantiate<AProjectile>(Projectile_RainDrop, clickPosition, Quaternion.identity, transform);
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
