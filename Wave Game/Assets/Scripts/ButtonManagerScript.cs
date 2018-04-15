using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManagerScript : MonoBehaviour {

    public Button[] buttons;
    public Button button_battle;
    public Button button_quit;
    public Button button_help;
    public Button button_credits;
    
    void Start () {
        buttons = GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(levelOne);
        buttons[1].onClick.AddListener(helpScreen);
        buttons[2].onClick.AddListener(credits);
        buttons[3].onClick.AddListener(quit);
    }

    void levelOne()
    {
        SceneManager.LoadScene(4);
    }

    void helpScreen()
    {
        SceneManager.LoadScene(2);
    }

    void credits()
    {
        SceneManager.LoadScene(3);
    }

    void quit()
    {
        Application.Quit();
    }

}
