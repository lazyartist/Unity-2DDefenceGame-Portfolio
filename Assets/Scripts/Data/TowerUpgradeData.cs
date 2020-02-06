using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerUpgradeData_", menuName = "SO/Create TowerUpgradeData")]
public class TowerUpgradeData : ScriptableObject
{
    public string Name;
    public float Cost;
    public Sprite IconSprite;
    public Unit UnitPrefab;
    public TowerUpgradeData[] TowerUpgradeDatas;
}
