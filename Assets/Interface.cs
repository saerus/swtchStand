using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour {
	// Use this for initialization
	void Start () {
		StartCoroutine(SendTof());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator SendTof() {
		while (true) {
			yield return new WaitForSeconds (5f);
			//Application.CaptureScreenshot(Application.persistentDataPath + "\\screen.png");
			Application.CaptureScreenshot ("screen.png");
			//Application.OpenURL("mailto:marc@fragment.in?subject=Tof&body=body&Attachment="+Application.persistentDataPath+ "\\screen.png"); 
			//Application.OpenURL("mailto:marc@fragment.in?subject=Tof&body=body");
			//Application.absoluteURL
		}
	}
}