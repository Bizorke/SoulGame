using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour {

	public GameObject [] AniFrames;

	public GameObject SaveFlash;

	public GameObject FireFlash;
	public GameObject FramesContainer;

	public GameController GameController;

	private int currentFrame = 0;
	private int aniDirection = 1;
	private float aniTimer;

	private float rotationSpeed = 0;

	float fallSpeed = 5f;

	public void Save()
	{
		Instantiate(SaveFlash, transform.position, Quaternion.identity);
		Destroy(gameObject);
		GameController.GivePoint();
	}

	// Use this for initialization
	void Start () {
		aniTimer = Random.value * 1.5f;
		float fallSpeed = 360;
		rotationSpeed = Random.value * fallSpeed - fallSpeed/2;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0, -fallSpeed * Time.deltaTime, 0);
		if (transform.position.y < -3)
		{
			Instantiate(FireFlash, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}

		aniTimer -= Time.deltaTime;
		if (aniTimer < 0)
		{
			aniTimer += 0.5f;

			AniFrames [currentFrame].SetActive(false);
			currentFrame += aniDirection;
			AniFrames [currentFrame].SetActive(true);

			if (currentFrame == 0 || currentFrame == 2)
			{
				aniDirection = -aniDirection;
			}
		}
	
		FramesContainer.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
	
	}

}
