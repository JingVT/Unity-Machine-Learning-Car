using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NNManager : MonoBehaviour {

	public GameObject carPrefab;

	private bool isTraning = false;
	private int populationSize = 10;
	private int generationNumber = 0;
	private int[] layers = new int[] { 3, 10, 10, 2 }; //3 input and 2 output
	private List<NeuralNetwork> nets;
	private List<Car> carList = null;
	private int RANDOM_SEED = 12342;
	//623743

	//Used for fitness
	private long currentTime;
	private float lastFitness = 0f;

	private bool checkTraining()
	{
		print (carList == null);
		for (int i = 0; i < carList.Count; i++) {
			if (carList [i].init == true) {
				return true;
			}
		}
		//All cars stop
		lastFitness = ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - currentTime);
		return false;
	}

	void Start()
	{
		currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isTraning == false) {
			//If no networks exist
			if (generationNumber == 0) {
				InitCarNeuralNetworks ();
			}
			else
			{
				nets.Sort();
				for (int i = 0; i < populationSize / 2; i++)
				{
					nets[i] = new NeuralNetwork(nets[i+(populationSize / 2)], RANDOM_SEED);
					RANDOM_SEED = RANDOM_SEED + 1;
					nets[i].Mutate();

					nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)], RANDOM_SEED); //reset neuron matrix values method, just going to make a deepcopy lol
					RANDOM_SEED = RANDOM_SEED + 1;
				}

				for (int i = 0; i < populationSize; i++)
				{
					nets[i].setFitness(0f);
				}
				generationNumber++;
				currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			}

			if (generationNumber == 0) {
				generationNumber++;
			}
				
			createCarBodies();

		}

		if (generationNumber > 0) {
			isTraning = checkTraining ();
		}

	}

	private void createCarBodies()
	{
		if (carList != null)
		{
			for (int i = 0; i < carList.Count; i++)
			{
				GameObject.Destroy(carList[i].gameObject);
			}

		}

		carList = new List<Car>();

		for (int i = 0; i < populationSize; i++)
		{
			Car car = ((GameObject)Instantiate(carPrefab, new Vector3(8.5f, 1f, 20f), Quaternion.Euler(carPrefab.transform.rotation.eulerAngles + new Vector3(0,-35,0)))).GetComponent<Car>();
			car.initNet(nets[i]);
			carList.Add(car);
		}

	}

	void InitCarNeuralNetworks()
	{
		//population must be even, just setting it to 20 incase it's not
		if (populationSize % 2 != 0)
		{
			populationSize = 20; 
		}

		nets = new List<NeuralNetwork>();

		for (int i = 0; i < populationSize; i++)
		{
			NeuralNetwork net = new NeuralNetwork(layers, RANDOM_SEED);
			RANDOM_SEED = RANDOM_SEED + 1;
			net.Mutate();
			nets.Add(net);
		}
	}

	public int getGenerations()
	{
		return generationNumber;
	}

	public float getLastFitness()
	{
		return lastFitness;
	}

	public float getLastDistance()
	{
		return (lastFitness / 1000) * 6f;
	} 

	public float getCurrentDistance ()
	{
		//print ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - currentTime);
		return ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - currentTime) / 1000f * 6f;
	}
}