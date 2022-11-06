using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	public float speed = 1;
	public bool right;

	// Update is called once per frame
	void Update () {
		if (right) {
			transform.Rotate (Vector3.right * Time.deltaTime * speed);
		} else {
			transform.Rotate (Vector3.up * Time.deltaTime * speed);
		}
	}
}
