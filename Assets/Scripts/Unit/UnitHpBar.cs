using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHpBar : MonoBehaviour {
    public Unit Unit;
    public SpriteRenderer HpBarBgSR;
    public SpriteRenderer HpBarGaugeSR;

    void Start () {
        // 팀 컬러 설정
        HpBarGaugeSR.color = Unit.TeamData.TeamColors[(int)Unit.TeamType];
    }
	
	// Update is called once per frame
	void Update () {
        HpBarGaugeSR.size = new Vector2(HpBarBgSR.size.x * Unit.Health / Unit.UnitData.Health, HpBarGaugeSR.size.y);

        if(Unit.Health <= 0f)
        {
            this.gameObject.SetActive(false);
        }
    }
}
