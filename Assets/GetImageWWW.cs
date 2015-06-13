using UnityEngine;
using System.Collections;

public class GetImageWWW : MonoBehaviour {

	// Continuously get the latest webcam shot from outside "Friday's" in Times Square
	// and DXT compress them at runtime
	string url = "http://www.fragment.in/unity/img/cff.png";



	void Start(){

		// Start a download of the given URL
		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www));


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator WaitForRequest(WWW wwwimg2)
	{

		// Create a texture in DXT1 format
		GetComponent<Renderer>().material.mainTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);

		while(true) {

			
			// wait until the download is done
			yield return wwwimg2;
			
			// assign the downloaded image to the main texture of the object
			wwwimg2.LoadImageIntoTexture(GetComponent<Renderer>().material.mainTexture as Texture2D);
		}

	}

	

}
