using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashController : MonoBehaviour {

	private float ttl = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//time goes down
		ttl -= Time.deltaTime;
		//time goes to zero
		if (ttl <= 0) {
			//flash goes away
			Destroy(gameObject);
		}
	}
}
