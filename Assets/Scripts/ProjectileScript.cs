using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    public Rigidbody2D rd2D;
    public float damage;
    [Range(0, 50)]
    public float speed = 1f;
    public float lifeDuration = 10f;
    //[HideInInspector]
    //public Vector3 direction;

    private readonly float angleOffset = Mathf.PI / 2;
    private bool initialized = false;
    private GameObject _target;
    private Vector3 _lastTargetPosition;

    // Use this for initialization
    void Start () {

        //direction = direction.normalized;
        //direction = Target.transform.position - transform.position;
        //direction = direction.normalized;

        //float angle = (Mathf.Atan2(direction.y, direction.x) - angleOffset) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Destroy(gameObject, lifeDuration);
    }
	
	// Update is called once per frame
	void Update () {
        //transform.position += direction * speed * Time.deltaTime;
    }

    public void Init(GameObject target)
    {
        _target = target;
        initialized = true;
    }

    void FixedUpdate()
    {
        if (initialized == false) return;

        if (_target != null)
        {
            _lastTargetPosition = _target.transform.position;
        }

        float distance = Vector3.Distance(_lastTargetPosition, transform.position);
        if (distance < 0.02)
        {
            Destroy(gameObject);
            return;
        }

        float move = Mathf.Min(speed * Time.fixedDeltaTime, distance);

        Vector3 direction = _lastTargetPosition - transform.position;
        direction = direction.normalized;

        float angle = (Mathf.Atan2(direction.y, direction.x) - angleOffset) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rd2D.MovePosition(transform.position  + (direction * move));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Consts.tagUnit)
        {
            Destroy(gameObject);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position + transform.right);
    //}
}
