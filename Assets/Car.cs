using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Car : MonoBehaviour {

	//Raycasts for input
	public GameObject emptyRaycast;
	private RaycastDistance raycastDist;

	//Neural Net Stuff
	private NeuralNetwork net;
	public bool init = false;

	//Used for fitness
	public long currentTime;

	//Used for stopping car
	private Rigidbody carRigid;
	//Used for turning car
	Transform carTransform;

	// Use this for initialization
	void Start () {
		raycastDist = emptyRaycast.GetComponent<RaycastDistance> ();
		currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		carRigid = GetComponent<Rigidbody> ();
		carTransform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (init) {
			//What goes into the net
			float[] inputs = new float[3];
			inputs [0] = raycastDist.distanceFromWallCenter;
			inputs [1] = raycastDist.distanceFromWallLeft;
			inputs [2] = raycastDist.distanceFromWallRight;
			float[] output = net.FeedForward (inputs);
			//Rotate the car
			rotateCar(output);
			//Set fitness
			net.setFitness ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - currentTime);
			//print (output[0] + ", " + output[1]);
		}
	}

	//Sets fitness and stops car on collision
	void OnTriggerEnter(Collider other) {
		if (other.name == "Track") {
			//Freeze the car
			carRigid.constraints = RigidbodyConstraints.FreezeAll;
			print ("Carcrash: " + net.getFitness());
			init = false;
		}
	}

	//Rotates the car using NN output
	void rotateCar(float[] NNoutput)
	{
		if (NNoutput [0] < 0 || NNoutput [1] < 0) {
			//Turn right
			if (NNoutput [0] > 0) {
				carTransform.Rotate (Vector3.forward);
			} else if (NNoutput [1] > 0) {
				carTransform.Rotate (Vector3.back);
			}
		}
	}

	//Start the net
	public void initNet(NeuralNetwork net)
	{
		this.net = net;
		init = true;
	}
}