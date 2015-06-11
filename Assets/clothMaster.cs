using UnityEngine;
using System.Collections;
//
public class clothMaster : MonoBehaviour {
	[Range(-3, 3)]
	public float X;
	[Range(-3, 3)]
	public float Y;
	[Range(100.0f, 1.0f)]
	public float size;
	public bool inverted;
	float invertedFloat = 1;
	float sizeReal;
	public Material mat;
	Vector2 imgSize = new Vector2();
	Texture tempwww;
	Vector2 offset = new Vector2();
	//
	//
	void Start () {
		StartCoroutine(LoadFromURL());

	}
	
	// Update is called once per frame
	void Update () {
		if (inverted) {
			invertedFloat = -1;
		} else {
			invertedFloat = 1;
		}
		if(tempwww) {
			// de plein à un diviseur de l'image (10 / 1/10)
			mat.SetTextureScale("_MainTex", new Vector2(size, size*invertedFloat));
			sizeReal = 1/size;
			offset = new Vector2(-size/2+0.5f+(-X)*size, -size/2+0.5f+Y*size);
			//offset.x *= invertedFloat;
			mat.SetTextureOffset("_MainTex", offset);
		}
	}
	IEnumerator LoadFromURL() {
		// Start a download of the given URL
		WWW www = new WWW("http://www.saerus.ch/logoonlinetest.png");
		
		// Wait for download to complete
		yield return www;
		tempwww = www.texture;
		imgSize.x = tempwww.width;
		imgSize.y = tempwww.height;
		Debug.Log (imgSize.x);
		
		// assign texture
		tempwww.wrapMode = TextureWrapMode.Clamp;
		Renderer renderer = GetComponent<Renderer>();
		mat.mainTexture = tempwww;
	}
}