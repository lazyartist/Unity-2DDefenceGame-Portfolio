using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TradeCupcakeTowers_Buying : TradeCupcakeTowers {
    public CupcakeTowerScript CupcakeTowerPrefab;
    public Text CostText;

    private int _cost;
    private Button _button;
    private CupcakeTowerScript _draftCupcakeTower;

    private void Awake()
    {
        _button = GetComponent<Button>();

        PlayerManager.Inst.Event_Sugar_Changed.AddListener(OnSugarChanged);
    }

    void OnSugarChanged()
    {
        _button.interactable = _cost <= PlayerManager.Inst.Sugar;
    }

    protected override void Start()
    {
        base.Start();

        _cost = CupcakeTowerPrefab.initialCost;
        CostText.text = _cost.ToString();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if(_draftCupcakeTower == null)
        {
            if(_cost <= PlayerManager.Inst.Sugar)
            {
                _draftCupcakeTower = Instantiate<CupcakeTowerScript>(CupcakeTowerPrefab, _gameManager.CupcakeTowerContainer.transform);
                _draftCupcakeTower.SetDraft(true);
                _draftCupcakeTower.enabled = false;

                _gameManager.DrawAllowedArea(true);
            }
        } else
        {
            Destroy(_draftCupcakeTower.gameObject);
            _draftCupcakeTower = null;
            _gameManager.DrawAllowedArea(false);
        }
    }

    void Update()
    {
        if(_draftCupcakeTower != null)
        {
            bool isPointerOnAllowedArea = _gameManager.IsPointerOnAllowedArea();
            SpriteRenderer sr = _draftCupcakeTower.GetComponent<SpriteRenderer>();

            if (isPointerOnAllowedArea)
            {
                sr.color = Color.green;
            }
            else
            {
                sr.color = Color.red;
            }


            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;

            _draftCupcakeTower.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x, y, -Camera.main.transform.position.z));

            if (Input.GetMouseButtonUp(0) && _gameManager.IsPointerOnAllowedArea())
            {
                PlayerManager.Inst.ChangeSugar(-_draftCupcakeTower.initialCost);

                sr.color = Color.white;

                _draftCupcakeTower.SetDraft(false);
                _draftCupcakeTower.enabled = true;
                BoxCollider2D collider = _draftCupcakeTower.gameObject.AddComponent<BoxCollider2D>();
                collider.offset = new Vector2(0f, -1.4f);
                collider.size = new Vector2(5f, 4.5f);

                _draftCupcakeTower = null;
                _gameManager.DrawAllowedArea(false);
            }
        }
    }
}
