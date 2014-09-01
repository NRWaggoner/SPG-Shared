using UnityEngine;
using System.Collections;

public class TestSceneController : MonoBehaviour {

	public GameObject loginMenu;

	private GameObject _localMenu;
	private LoginMenuController _loginMenuCntroller = null;
	// Use this for initialization
	void Start () {

		//loginMenu.SetActive(false);
		_localMenu = (GameObject)Instantiate(loginMenu);
		_loginMenuCntroller = (LoginMenuController)_localMenu.GetComponent<LoginMenuController>();
		_loginMenuCntroller.HideLoginMenu();



	
	}
	
	// Update is called once per frame
	void Update () {

		if (RemindLaterManager.HasRemindLaterTimeElapsed())
			_loginMenuCntroller.ShowLoginMenu();
	
	}


	void OnGUI()
	{
		if (GUI.Button(new Rect(10,10,120,30), "Show Sign-up menu"))
		{
			if (_loginMenuCntroller != null)
			{
				//loginMenu.SetActive(true);
				 
				_loginMenuCntroller.ShowLoginMenu();
			}
		}

	}
}
