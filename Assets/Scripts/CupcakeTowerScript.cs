using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeTowerScript : MonoBehaviour {
    [Header("비용")]
    public int initialCost;
    public int upgradingCost;
    public int sellingValue;

    [Header("속성")]
    public float rangeRadius;
    public float reloadTime;

    [Tooltip("발사체")]
    public ProjectileScript projectilePrefab;
    public Sprite[] upgradeSprites;
    public bool isUpgradable = true;

    public SpriteRenderer SelectionAreaSpriteRenderer;

    private int upgradeLevel = 0;
    private float elapsedTime;
    private SpriteRenderer _spriteRenderer;



    public int GetUpgradeLevel()
    {
        return upgradeLevel;
    }

	// Use this for initialization
	void Start () {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetDraft(bool _isDraft)
    {
        if (_isDraft)
        {
            SelectionAreaSpriteRenderer.transform.localScale = new Vector3(rangeRadius/2, rangeRadius/2, 1);
        }

        SelectionAreaSpriteRenderer.enabled = _isDraft;
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Upgrade();
        //}

		if(elapsedTime >= reloadTime)
        {
            elapsedTime = 0;

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, rangeRadius);
            if(hitColliders.Length != 0)
            {
                float min = int.MaxValue;
                int index = -1;
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if(hitColliders[i].tag == Consts.tagUnit)
                    {
                        PandaScript panda = hitColliders[i].GetComponent<PandaScript>();

                        if (panda.IsDied) continue;

                        float distance = Vector2.Distance(hitColliders[i].transform.position, transform.position);
                        if(distance < min)
                        {
                            min = distance;
                            index = i;
                        }
                    }
                }

                if (index != -1)
                {

                    ProjectileScript projectile = GameObject.Instantiate<ProjectileScript>(projectilePrefab, transform);
                    projectile.transform.position = transform.position;

                    //Vector3 direction = hitColliders[index].transform.position - transform.position;
                    projectile.Init(hitColliders[index].gameObject);
                    //projectile.direction = direction;
                }
            }
        }
        elapsedTime += Time.deltaTime;
	}

    public void Upgrade()
    {
        if(!isUpgradable || upgradeLevel + 1 >= upgradeSprites.Length)
        {
            isUpgradable = false;
            return;
        }

        upgradeLevel++;

        sellingValue += 5;
        upgradingCost += 10;

        // stat up
        rangeRadius += 1f;
        reloadTime += 0.5f;

        _spriteRenderer.sprite = upgradeSprites[upgradeLevel];
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeRadius);
    }
}
