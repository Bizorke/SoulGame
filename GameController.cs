using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	
	public GameObject Soul;
	public GameObject BadSoul;

	public GameObject PlayButton;

	public GameObject ScoreDisplay;

	public GameObject Portal;

	public GameObject ProgressBar;

	public GameObject NotificationPopupController;

	public GameObject MusicLevelOne;

	private float score = 0;

	private float maxScore = 20;

	private bool gameActive = false;

	private float arenaWidth = 25;

	private LevelController LevelController;

	public void GivePoint()
	{
		score = score + 1;
		ProgressBar.GetComponent<Slider>().value = score / maxScore;
		ScoreDisplay.GetComponent<Text>().text = "Score: " + score;

		LevelController.GivePoint();
	}

	// Use this for initialization
	void Start () {
		LevelController = new LevelController(this, Soul, BadSoul);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameActive){
			LevelController.Update();
		}
		
	}

	public void PlayButtonClick()
	{
		gameActive = true;
		PlayButton.SetActive(false);
		Portal.SetActive(true);
		ProgressBar.SetActive(true);
		ScoreDisplay.SetActive(true);
		ScoreDisplay.GetComponent<Text>().text = "Score: 0";
		MusicLevelOne.GetComponent<AudioSource>().Play();
		Notify("Level 1");
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void Notify(string text)
	{
		NotificationPopupController.GetComponent<NotificationPopupController>().DisplayMessage(text);
	}
}
