using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopupController : MonoBehaviour {

	private float displayCountDown = 0;
	private Text text;

	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<Text>();
	}
	
	public void DisplayMessage(string message, float time = 2)
	{
		text.text = message;
		displayCountDown = time;
		text.enabled = true;
	}

	// Update is called once per frame
	void Update () {
		if(displayCountDown > 0)
		{
			displayCountDown -= Time.deltaTime;
			if(displayCountDown <= 0)
			{
				text.enabled = false;
			}
		}
	}
}
