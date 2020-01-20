using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TradeCupcakeTowers : MonoBehaviour, IPointerClickHandler {
    protected static SugarMeterScript sugarMeter;
    protected static CupcakeTowerScript currentActiveTower;
    protected static GameManager _gameManager;


    public abstract void OnPointerClick(PointerEventData eventData);

    // Use this for initialization
    virtual protected void Start () {
		if(sugarMeter == null)
        {
            sugarMeter = FindObjectOfType<SugarMeterScript>();
        }

        //if(_gameManager == null)
        //      {
        //          _gameManager = FindObjectOfType<GameManager>();
        //      }
        _gameManager = GameManager.Inst;
	}
	
	public static void setActiveTower(CupcakeTowerScript cupcakeTower)
    {
        currentActiveTower = cupcakeTower;
    }
}
