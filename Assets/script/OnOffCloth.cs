using UnityEngine;
using System.Collections;

public class OnOffCloth : MonoBehaviour {
	Cloth cloth;
	// Use this for initialization
	void Start () {
		cloth = gameObject.GetComponent<Cloth>() as Cloth;
		cloth.enabled = false;
	}
	// Update is called once per frame
	void Update () {
	
	}
	void OnBecameVisible () {
		cloth.enabled = true;
	}
	void OnBecameInvisible () {
		cloth.enabled = false;
	}
}
