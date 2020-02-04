using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Support : Tower
{
    public Transform SpawnPosition;
    public Transform WaitingPosition;
    public float WaitingPositionRadius = 1f;

    // 생성 유닛 프리팹
    public Unit UnitPrefab;
    public int UnitCount;
    //public Vector3 UnitWaitingPosition;

    private List<Unit> _units;
    private float _elasedSpawnTime;

    private void Start()
    {
        _units = new List<Unit>();
    }

    private void Update()
    {
        _units.Remove(null);

        _elasedSpawnTime += Time.deltaTime;

        if (_elasedSpawnTime >= UnitData.AttackCoolTime && _units.Count < UnitCount)
        {
            Unit unit = Instantiate<Unit>(UnitPrefab, SpawnPosition.position, Quaternion.identity, transform);

            // 대기장소 인근 랜덤한 위치
            {
                float radius = Random.Range(WaitingPositionRadius / 3, WaitingPositionRadius);
                float radian = Random.Range(0f, Mathf.PI * 2);
                float x = Mathf.Cos(radian) * radius;
                float y = Mathf.Sin(radian) * radius;
                //float y = Mathf.Sin(rad) * (WaitingPositionRadius * 0.5f);

                WaypointManager.Inst.WaypointPool.Release(unit.WaitWaypoint);
                unit.WaitWaypoint = WaypointManager.Inst.WaypointPool.Get();
                unit.WaitWaypoint.transform.position = new Vector3(WaitingPosition.position.x + x, WaitingPosition.position.y + y, WaitingPosition.position.z);
                //unit.WaitingPosition = new Vector3(WaitingPosition.position.x + x, WaitingPosition.position.y + y, WaitingPosition.position.z);

            }

            _units.Add(unit);

            _elasedSpawnTime = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(WaitingPosition.position, WaitingPositionRadius);
    }
}
