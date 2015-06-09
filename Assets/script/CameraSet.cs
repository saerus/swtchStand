using UnityEngine;
using System.Collections;

public class CameraSet : MonoBehaviour {




	// Use this for initialization
	void Start () {

		WebCamDevice[] devices = WebCamTexture.devices;
		for( var i = 0 ; i < devices.Length ; i++ ){
			Debug.Log(devices[i].name);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}


