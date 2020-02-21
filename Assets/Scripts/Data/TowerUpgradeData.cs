using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerUpgradeData_", menuName = "SO/Create TowerUpgradeData")]
public class TowerUpgradeData : ScriptableObject
{
    public string Name;
    public int GoldCost;
    public Sprite IconSprite;
    public Sprite UnitSprite;
    public Unit UnitPrefab;
    public TowerUpgradeData[] TowerUpgradeDatas;
}
