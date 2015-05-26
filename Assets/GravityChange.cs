using UnityEngine;
using System.Collections;

public class GravityChange : MonoBehaviour {
	float time = 0;
	Vector3 wind;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		time ++;
		wind.x = Mathf.Sin(time/30f)*5;
		wind.z = -9.81f;
		wind.y = Mathf.Cos(time/30f)*5;
		Physics.gravity = wind;
	}
}
