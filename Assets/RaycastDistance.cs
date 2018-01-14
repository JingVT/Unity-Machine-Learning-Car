using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDistance : MonoBehaviour {

	public float distanceFromWallCenter;
	public float distanceFromWallRight;
	public float distanceFromWallLeft;

	private RaycastHit hit;

	private Vector3 sideShift;
	private Quaternion spreadAngleLeft;
	private Quaternion spreadAngleRight;

	void Start()
	{
		sideShift = new Vector3 (.6f, 0, 0);
		spreadAngleLeft = Quaternion.AngleAxis(-45, new Vector3(0, 1, 0));
		spreadAngleRight = Quaternion.AngleAxis(45, new Vector3(0, 1, 0));
	}

	//Casts a raycast every fixed update to get position data
	void FixedUpdate()
	{
		//Get angles
		Vector3 noAngle = transform.up;
		Vector3 leftAngle = spreadAngleLeft * noAngle;
		Vector3 rightAngle = spreadAngleRight * noAngle;
		//Get Center
		Physics.Raycast (transform.position, noAngle, out hit);
		distanceFromWallCenter = hit.distance;
		//Get Right
		Physics.Raycast (transform.position - sideShift, rightAngle, out hit);
		distanceFromWallRight = hit.distance;
		//Get Left
		Physics.Raycast (transform.position + sideShift, leftAngle, out hit);
		distanceFromWallLeft = hit.distance;
		//Debug
		Debug.DrawRay (transform.position, transform.up);
		Debug.DrawRay (transform.position + (-1f * transform.right) * .6f, rightAngle);
		Debug.DrawRay (transform.position + (transform.right) * .6f, leftAngle);
	}
}