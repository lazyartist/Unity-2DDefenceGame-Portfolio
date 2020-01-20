using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunctionalities : MonoBehaviour {
    public void OnClick_NewGame()
    {
        SceneManager.LoadScene("Level_01");
        //SceneManager.LoadScene("Level_01", LoadSceneMode.Additive);
    }

    public void OnClick_Settings()
    {
        // 
    }

    public void OnClick_Quit()
    {
        Application.Quit();
    }
}
