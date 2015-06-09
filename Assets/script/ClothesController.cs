using UnityEngine;
using System.Collections;

public class ClothesController : MonoBehaviour {

	public string[] clothes;
	public int activeCloth = 0;
	// Use this for initialization
	void Start () {

		Debug.Log ("ActiveCloth = "+clothes[activeCloth]);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
