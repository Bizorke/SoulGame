using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalContoller : MonoBehaviour {
	private float speed = 2f;
	private float elasticity = 0.7f;
	float xSpeed = 0;
	//float zSpeed = 0;
	float arenaWidth = 30;
	float arenaLength = 30;
	float friction = 0.2f;

	//Vector3 lastPos = new Vector3();

	// Use this for initialization
	void Start () {
		//lastPos = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 delta = Input.mousePosition - lastPos;
		//lastPos = Input.mousePosition;
		xSpeed += Input.GetAxis("Mouse X");
		float arrowKeyAcceleration = 4;
		xSpeed += Input.GetKey("left") ? -arrowKeyAcceleration : 0;
		xSpeed += Input.GetKey("right") ? arrowKeyAcceleration : 0;
		//zSpeed += Input.GetAxis("Mouse Y");

		//zSpeed *= friction;

		//this.gameObject.transform.Translate(xSpeed * Time.deltaTime * speed, 0, zSpeed * Time.deltaTime * speed);
		this.gameObject.transform.Translate(xSpeed * Time.deltaTime * speed, 0, 0);

		xSpeed *= friction;

		if (gameObject.transform.position.x < -arenaLength / 2){
			this.gameObject.transform.Translate(-gameObject.transform.position.x -arenaLength / 2, 0, 0);
			xSpeed *= -elasticity;
		}
		else if (gameObject.transform.position.x > arenaLength / 2){
			this.gameObject.transform.Translate(-gameObject.transform.position.x + arenaLength / 2, 0, 0);
			xSpeed *= -elasticity;
		}

		// if (gameObject.transform.position.z < -arenaWidth/ 2){
		// 	this.gameObject.transform.Translate(0,0,-gameObject.transform.position.z -arenaWidth / 2);
		// 	zSpeed *= -elasticity;
		// }
		// else if (gameObject.transform.position.z > arenaWidth / 2){
		// 	this.gameObject.transform.Translate(0,0,-gameObject.transform.position.z + arenaWidth / 2);
		// 	zSpeed *= -elasticity;
		// }
	}

	private void OnTriggerEnter(Collider other)
    {
		other.GetComponent<SoulController>().Save();

    }
}
