using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpeed : MonoBehaviour {

	public float carSpeed;

	public Rigidbody carRigid;

	// Use this for initialization
	void Start () {
		carRigid = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (carRigid.velocity.magnitude < carSpeed) {
			carRigid.AddForce (10f*transform.up);
		}
	}
}