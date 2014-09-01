using UnityEngine;
using System;
using System.Collections;

public class RemindLaterManager  {

	public static DateTime RemindLater {get;set;}

	public static bool HasRemindLaterTimeElapsed()
	{
		GameObject menu = GameObject.FindGameObjectWithTag("LoginMenu");
		LoginMenuController loginMenuController = (LoginMenuController)menu.GetComponent<LoginMenuController>();
		int remindMinutesDuration = loginMenuController.GetRemindMeLterMinutes();

		if (RemindLater == null)
			return false;

		TimeSpan timeElapsed = DateTime.UtcNow - RemindLater;

		if (timeElapsed.TotalMinutes >= remindMinutesDuration)
			return true;

		return false;

	}
}
