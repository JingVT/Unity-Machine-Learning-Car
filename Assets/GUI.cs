using UnityEngine.UI;
using UnityEngine;

public class GUI : MonoBehaviour {
	public Text genText;
	public Text fitText;
	public Text lastDisText;
	public Text currDisText;

	//get data
	public GameObject nnManageGO;
	private NNManager nnManager;

	void Start() {
		nnManager = nnManageGO.GetComponent<NNManager> ();
	}

	void Update () {
		setGenText ();
		setFitText ();
		setLastDisText ();
		setCurrDisText ();
	}

	void setGenText() {
		int gen = nnManager.getGenerations ();
		genText.text = "Generation: " + gen.ToString ();
	}
	void setFitText() {
		float fit = nnManager.getLastFitness ();
		fitText.text = "Last Fitness: " + fit.ToString ();
	}
	void setLastDisText() {
		float lastDis = nnManager.getLastDistance ();
		lastDisText.text = "Last Distance Travelled: " + lastDis.ToString ();
	}
	void setCurrDisText() {
		float currDis = nnManager.getCurrentDistance ();
		currDisText.text = "Current Distance Travelled: " + currDis.ToString ();
	}
}
