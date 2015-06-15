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
	Vector2 offset = new Vector2();
	public string name;
	public Vector2 frontPos, backPos;
	public float baseScale;
	// -------------------------------------------------- CLOTH
	Cloth cloth;
	public GameObject clothGO;
	GameObject main;
	//
	void Start () {
		cloth = clothGO.GetComponent<Cloth>() as Cloth;
		cloth.enabled = false;
		main = GameObject.Find ("_MAIN");
	}
	// Update is called once per frame
	void Update () {
		if (inverted) {
			invertedFloat = -1;
		} else {
			invertedFloat = 1;
		}
		//if(tempwww) {
			// de plein à un diviseur de l'image (10 / 1/10)
			mat = clothGO.gameObject.GetComponent<Renderer> ().materials [1];
			mat.SetTextureScale("_MainTex", new Vector2(size, size*invertedFloat));
			sizeReal = 1/size;
			offset = new Vector2(-size/2+0.5f+(-X)*size, -size/2+0.5f+Y*size);
			//offset.x *= invertedFloat;
			mat.SetTextureOffset("_MainTex", offset);
		//}
	}
	void setPosition(string position) {
		if (position.Equals ("Front")) {
			X = frontPos.x;
			Y = frontPos.y;
			size = baseScale;
			inverted = false;
		} else if (position.Equals ("Back")) {
			X = backPos.x;
			Y = backPos.y;
			size = baseScale;
			inverted = true;
		}
		/*X = _x;
		Y = _y;
		size = _size;
		inverted = _inverted;*/
	}
	void updatePosition(string dir) {
		if (dir.Equals ("Left")) {
			X -= 0.005f;
		} else if (dir.Equals ("Right")) {
			X += 0.005f;
		} else if (dir.Equals ("Up")) {
			Y -= 0.005f;
		} else if (dir.Equals ("Down")) {
			Y += 0.005f;
		} else if (dir.Equals ("Bigger")) {
			size -= 2f;
		} else if (dir.Equals ("Smaller")) {
			size += 2f;
		}
	}
	void changeColor(Color c) {
		//Debug.Log ("**********");
		clothGO.gameObject.GetComponent<Renderer>().materials[0].color = c;
		clothGO.gameObject.GetComponent<Renderer> ().materials [2].SetColor("_Emission", c);
	}
	void OnBecameVisible () {
		cloth.enabled = true;
		main.BroadcastMessage("setCloth", name, SendMessageOptions.DontRequireReceiver);

	}
	void changeImg(Texture2D newImg) {
		Debug.Log (clothGO.gameObject.GetComponent<Renderer> ().materials [1].name);
		clothGO.gameObject.GetComponent<Renderer> ().materials [1].SetTexture("_MainTex", newImg);

	}
	void OnBecameInvisible () {
		cloth.enabled = false;
	}
}