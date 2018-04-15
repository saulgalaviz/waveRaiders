using UnityEngine;
using UnityEngine.UI;

public class MenuBattle : MonoBehaviour {
    public Button yourButton;

	// Use this for initialization
	void Start () {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }
	
	// Update is called once per frame
	void TaskOnClick () {
        Debug.Log("You have clicked the button!");
    }
}
