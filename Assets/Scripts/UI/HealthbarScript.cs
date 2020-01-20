using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour {
    public Image HealthBarFillingImage;
    public Text HealthText;

	void Awake () {
        PlayerManager.Inst.Event_Health_Changed.AddListener(OnHealthChanged);
    }

    void OnDisable()
    {
        //PlayerManager.Inst.Event_Health_Changed.RemoveListener(OnHealthChanged);
    }

    void OnHealthChanged()
    {
        HealthBarFillingImage.fillAmount = (float)PlayerManager.Inst.Health  / Values.Inst.player_init_health;
        HealthText.text = PlayerManager.Inst.Health.ToString() + " / " + Values.Inst.player_init_health.ToString();

        if (HealthBarFillingImage.fillAmount < 0.2f)
        {
            HealthBarFillingImage.color = Color.red;
        } else if (HealthBarFillingImage.fillAmount < 0.4f)
        {
            HealthBarFillingImage.color = Color.yellow;
        } else
        {
            HealthBarFillingImage.color = Color.white;
        }
    }
}
