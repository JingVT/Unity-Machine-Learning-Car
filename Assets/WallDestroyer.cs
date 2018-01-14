using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroyer : MonoBehaviour {

	void OnCollisionEnter (Collision col)
	{
		Destroy (col.gameObject);
	}
}
