using System.Collections.Generic; //Gives list
using System;
public class NeuralNetwork : IComparable<NeuralNetwork> {

	private int[] layers;
	private float[][] neurons;
	private float[][][] weights;
	private float fitness;

	//public int randomSeed;

	private Random random;

	//The constuctor requires layer information
	//to create the neural network
	public NeuralNetwork(int[] layers, int randomSeed)
	{
		this.layers = new int[layers.Length];
		for (int i = 0; i < layers.Length; i++) {
			this.layers [i] = layers [i];
		}

		random = new Random(randomSeed);

		createNeurons ();
		createWeights ();
	}

	//This constuctor creates a deep copy
	public NeuralNetwork (NeuralNetwork copyNetwork, int randomSeed)
	{
		this.layers = new int[copyNetwork.layers.Length];
		for (int i = 0; i < copyNetwork.layers.Length; i++)
		{
			this.layers [i] = copyNetwork.layers [i];
		}

		random = new Random(randomSeed);

		createNeurons ();
		createWeights ();
		copyWeights (copyNetwork.weights);
	}

	//Used in deep copy
	private void copyWeights (float[][][] copyWeights)
	{
		//For every layer with weights
		for (int i = 0; i < weights.Length;i++)
		{
			//For every neuron in layer
			for (int j = 0; j < weights[i].Length; j++)
			{
				//For every weight in a neuron.
				for (int k = 0; k < weights[i][j].Length; k++)
				{
					weights [i] [j] [k] = copyWeights [i] [j] [k];
				}
			}
		}
	}

	//Creates all of the neurons in layers
	private void createNeurons()
	{
		//A list of a array is a jagged array
		List<float[]> neuronList = new List<float[]> ();

		//layers.Length is the total number of layers in the array
		for (int i = 0; i < layers.Length; i++) {
			//<[],[1],[1,2],[1,2,3]
			neuronList.Add (new float[layers [i]]);
		}

		//Set up the global jagged array
		neurons = neuronList.ToArray ();

	}

	//Creates all the weights per neuron.
	private void createWeights()
	{
		//This list is all of the layers
		List<float[][]> weightsList = new List<float[][]> ();
		//Note we start on i=1 since the input layer doesn't have weights
		for (int i = 1; i < layers.Length; i++) {
			//This list is all of the neurons
			List<float[]> layerWeightList = new List<float[]> ();
			//Note that each neuron gets weights from prevous layer
			int neuronsInPreviousLayer = layers [i - 1];

			//Randomly set weights between 1,-1
			for (int j = 0; j < neurons[i].Length; j++) {
				//This float array is all of the weights on a single neuron
				float[] neuronWeights = new float[neuronsInPreviousLayer];

				//This fills the weights with random data
				for (int k = 0; k < neuronsInPreviousLayer; k++) {
					//Note that we want between -.5 and .5, Thats why we have -.5
					neuronWeights [k] = (float) random.NextDouble () - 0.5f;
				}

				//layerWeightsList contains all of the neurons. Adding one neuron worth of weights
				layerWeightList.Add (neuronWeights);
			}
			//weightsList is the list of all the layers. Adding one layer worth.
			//Also note that we are adding the array version
			weightsList.Add (layerWeightList.ToArray ());
		}
		//weights is global for all layers. Note the 3D j array
		weights = weightsList.ToArray();
	}

	public float[] FeedForward(float[] inputs)
	{
		for (int i = 0; i < inputs.Length; i++) {
			//Setting the first layer equal to the input.
			neurons [0] [i] = inputs [i];
		}
		//Note that we look over all layers with weights
		for (int i = 1; i < layers.Length; i++) {
			//Now get every neuron in layer
			for (int j = 0; j < neurons [i].Length; j++) {
				//Temp, Will be: N1*W1+N2*W2+N3*W3
				float value = 0f;
				//Now we iterate over weights, Amount of weights based on nodes in previous layer.
				for (int k = 0; k < neurons [i - 1].Length; k++) {
					//Note we get the weight matrix at i-1 because it is one shorter
					value = value + weights[i-1][j][k] * neurons[i-1][k];
				}
				//Activation function. update neuron value
				neurons[i][j] = (float) Math.Tanh(value);
			}
		}

		//This returns the final output layer.
		return neurons[neurons.Length -1];
	}

	public void Mutate()
	{
		//For every layer with weights
		for (int i = 0; i < weights.Length; i++) {
			//For every neuron in the layer
			for (int j = 0; j < weights [i].Length; j++) {
				//For every weight
				for (int k = 0; k < weights [i] [j].Length; k++) {
					//Gets single weight
					float weight = weights [i] [j] [k];
					//Gets random 1-1000
					float randomNumber = (float)random.NextDouble () * 1000f;
					//Random variations
					if (randomNumber <= 2f) {
						weight = weight * -1f;
					} else if (randomNumber <= 4f) {
						weight = (float)random.NextDouble () - .5f;
					} else if (randomNumber <= 6f) {
						float factor = ((float)random.NextDouble ()) + 1f;
						weight = weight * factor;
					} else if (randomNumber <= 8f) {
						float factor = (float)random.NextDouble ();
						weight = weight * factor;
					}
					//Re-apply to weights matrix
					weights [i] [j] [k] = weight;
				}
			}
		}
	}

	//Fitness modifires
	public void addFitness (float fit)
	{
		fitness = fitness + fit;
	}

	public void setFitness (float fit)
	{
		fitness = fit;
	}

	public float getFitness ()
	{
		return fitness;
	}

	public int CompareTo(NeuralNetwork other)
	{
		if (other == null) {
			return 1;
		}
		if (fitness > other.fitness) {
			return 1;
		} else if (fitness < other.fitness) {
			return -1;
		} else {
			return 0;
		}
	}
}