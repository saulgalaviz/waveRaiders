using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{

    public Button[] buttons;
    public Button button_menu;

    void Start ()
    {
        buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(menuScreen);
    }
	
	void menuScreen ()
    {
        SceneManager.LoadScene(1);
    }
}
