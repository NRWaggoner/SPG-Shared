using UnityEngine;
using System;
using System.Collections;

public enum LoginMenuWindow
{
	Main,
	Login,
	SignUp,
	RemindLater

}

public class LoginMenuController : MonoBehaviour {

	public LoginMenuWindow loginMenuWindow;

	//For testing 
	public bool userIsParent = true;
	public int remindMeLaterMinutes;

	public Rect mainMenuWindowRect;
	public Rect logInRect;
	public Rect signUpRect;
	public Rect remindMeLaterRect;

	private string  m_username = "";
	private string  m_password = "";
	private bool 	m_isAuthenticated = false;

	/// <summary>
	/// Shows the login menu.
	/// </summary>
	public void ShowLoginMenu()
	{
		this.enabled = true;
	}

	/// <summary>
	/// Hides the login menu.
	/// </summary>
	public void HideLoginMenu()
	{
		this.enabled = false;
	}

	public int GetRemindMeLterMinutes()
	{
		return remindMeLaterMinutes;
	}

	// Use this for initialization
	void Start () {
		//Show main menu	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if (loginMenuWindow == LoginMenuWindow.Main)
		{
			GUI.Window(0,mainMenuWindowRect, MainWindow, "Main Menu");
		}
		else if (loginMenuWindow == LoginMenuWindow.Login)
		{
			GUI.Window(1,mainMenuWindowRect, LoginWindow, "Login Screen");
		}
		else if (loginMenuWindow == LoginMenuWindow.SignUp)
		{
			GUI.Window(2,mainMenuWindowRect, SignUpWindow, "Sign-up Screen");
		}
	}

	void MainWindow(int windowID)
	{
		if (GUI.Button(logInRect, "Log-In"))
		{
			if (VerifyUserIsParent())
				loginMenuWindow = LoginMenuWindow.Login;
		}
		
		
		if (GUI.Button(signUpRect, "Sign-Up"))
		{
			if (VerifyUserIsParent())
				loginMenuWindow = LoginMenuWindow.SignUp;			
		}
		
		if (GUI.Button(remindMeLaterRect, "Remind Me Later"))
		{
			//hide form
			//set remind timer to zero
			RemindLaterManager.RemindLater = DateTime.UtcNow;
			this.enabled = false;
		}
	}


	void LoginWindow(int windowID)
	{
		GUI.Label(new Rect(10,20,80,30),"Username: ");
		m_username = GUI.TextField(new Rect(110,20,100,30), m_username);

		GUI.Label(new Rect(10,60,80,30),"Password: ");
		m_password = GUI.TextField(new Rect(110,60,100,30), m_password);

		if (GUI.Button(new Rect(110,120,60,30), "Log-In"))
		{
			Authenticate();

		}

		if (GUI.Button(new Rect(10,160,100,30), "Forgot password"))
		{
			//password retrieval
		}

		if (GUI.Button(new Rect(120,160,100,30), "Cancel"))
		{
			loginMenuWindow = LoginMenuWindow.Main;
		}
	}

	void SignUpWindow(int windowID)
	{
		GUI.Label(new Rect(10,20,80,30),"Email: ");
		m_username = GUI.TextField(new Rect(110,20,100,30), m_username);
		
		GUI.Label(new Rect(10,60,80,30),"Password: ");
		m_password = GUI.TextField(new Rect(110,60,100,30), m_password);

		if (GUI.Button(new Rect(110,120,60,30), "Sign-Up"))
		{
			//sign-up process
		}
	}

	bool VerifyUserIsParent()
	{
		//Fro testing just return bool available in editor
		return userIsParent;
	}

	void Authenticate()
	{
		//authenticate
		//TODO - perform login authentication
		
		//if success:
		//m_isAuthenticated = true;
		//else
		//m_isAuthenticated = false;
		
		//TODO Place in callback method?
		if (m_isAuthenticated)
			UnlockProtectedContent();
		
	}

	void UnlockProtectedContent()
	{

	}


}
