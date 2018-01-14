using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurFirstSharedScript : MonoBehaviour {

	private Transform cubeTransform;

	// Use this for initialization
	void Start () {
		cubeTransform = GetComponent<Transform> ();
	}

	// Update is called once per frame
	void Update () {
		cubeTransform.position = new Vector3 (cubeTransform.position.x + .01f, cubeTransform.position.y, cubeTransform.position.z);
	}
}
