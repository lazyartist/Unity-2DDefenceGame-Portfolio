using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaScript : MonoBehaviour {
    //public PandaHpBar PandaHpGaugePrefab;

    public Animator Animator;
    public Rigidbody2D rd2D;

    public Vector3 HpBarPosition;
    [HideInInspector]
    public PandaHpBar PandaHpBar;

    //public float speed;
    private float health;
    //public float maxHealth = 1f;
    //public int Damage;
    //public int cakeEatenPerBite;

    static private GameManager _gameManager;
    static private HUDManager _hudManager;

    private int animDieTriggerHash = Animator.StringToHash("DieTrigger");
    private int animHitTriggerHash = Animator.StringToHash("HitTrigger");
    private int animEatTriggerHash = Animator.StringToHash("EatTrigger");

    private Waypoint _currentWaypoint;
    private AnimatorStateInfo _currentAnimatorStateInfo;
    private BoxCollider2D _collider;

    public bool IsDied { get; private set; }

    void Start () {
        //if(_gameManager == null)
        //{
        //    _gameManager = FindObjectOfType<GameManager>();
        //}
        _gameManager = GameManager.Inst;

        if (_hudManager == null)
        {
            _hudManager = FindObjectOfType<HUDManager>();
        }

        _collider = GetComponent<BoxCollider2D>();

        _currentAnimatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);

        _currentWaypoint = _gameManager.FirstWaypoint;

        //PandaHpBar = Instantiate<PandaHpBar>(PandaHpGaugePrefab, _hudManager.GaugeContainer.transform);

        health = Values.Inst.panda_maxHealth;
    }

    private void OnDestroy()
    {
        if(PandaHpBar != null && PandaHpBar.gameObject != null)
        {
            Destroy(PandaHpBar.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        ((RectTransform)PandaHpBar.transform).position = _hudManager.Camera.WorldToScreenPoint(transform.position + HpBarPosition);

        if (IsDied)
        {
            PandaHpBar.GaugeImage.fillAmount = 0f;
            return;
        }

		if(_currentAnimatorStateInfo.fullPathHash != Animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
        {
            Animator.ResetTrigger(animDieTriggerHash);
            Animator.ResetTrigger(animHitTriggerHash);
            Animator.ResetTrigger(animEatTriggerHash);

            _currentAnimatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        }

        PandaHpBar.GaugeImage.fillAmount = health / Values.Inst.panda_maxHealth;
    }

    private void FixedUpdate()
    {
        if (IsDied) return;

        if (_currentWaypoint != null)
        {
            MoveTowards(_currentWaypoint.transform.position);
            //((RectTransform)PandaHpGauge.transform).position = _hudManager.Camera.WorldToScreenPoint(transform.position + HpBarPosition);

            if (Vector3.Distance(transform.position, _currentWaypoint.transform.position) < 0.001f)
            {
                _currentWaypoint = _currentWaypoint.NextWaypoint;
            }
        }
        else
        {
            _gameManager.BiteTheCake(Values.Inst.panda_cakeEatenPerBite);
            //_hudManager.HealthbarScript.ApplyDamage(Damage);
            Animator.SetTrigger(animEatTriggerHash);
            PandaHpBar.gameObject.SetActive(false);
            IsDied = true;
        }
    }

    private void MoveTowards(Vector3 destination)
    {
        //float step = speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, destination, step);

        float maxDistanceDelta = Values.Inst.panda_speed * Time.fixedDeltaTime;
        //float maxDistanceDelta = speed * Time.fixedDeltaTime;
        rd2D.MovePosition(Vector3.MoveTowards(transform.position, destination, maxDistanceDelta));
    }

    private void Hit(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Animator.SetTrigger(animDieTriggerHash);
            PandaHpBar.gameObject.SetActive(false);
            _collider.enabled = false;
            _gameManager.OneMorePandaInHaven();

            PlayerManager.Inst.ChangeSugar(Values.Inst.panda_sugar_point);

            IsDied = true;
        }
        else
        {
            Animator.SetTrigger(animHitTriggerHash);
        }
    }

    private void Eat()
    {
        Animator.SetTrigger(animEatTriggerHash);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            ProjectileScript projectile = collision.GetComponent<ProjectileScript>();
            Hit(projectile.damage);
            //Debug.Log(projectile);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + HpBarPosition, 1);
        //Gizmos.DrawWireCube(transform.position + HpBarPosition, new Vector3());
    }
}
