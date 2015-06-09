using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GetUrl : MonoBehaviour {

	public string url = "http://fragment.in/unity/switcher.php?action=GetInfo";//?var1=value2&amp;var2=value2

	void Start () {

		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www));
	}
	
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);

			var N = JSONNode.Parse(www.text);

			Debug.Log("parsedColor : " + N["color"]);
			Debug.Log("parsedBrand : " + N["brand"]);


			if(string.Equals(N["color"], "red")){
				Debug.Log("found RED");

			}else{
				Debug.Log("not found");
			}

		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}
}


