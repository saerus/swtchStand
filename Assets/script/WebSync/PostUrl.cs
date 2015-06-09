using UnityEngine;
using System.Collections;

public class PostUrl : MonoBehaviour {

	public string url = "http://example.com/script.php";

	void Start () {
		

		
		WWWForm form = new WWWForm();
		form.AddField("var1", "value1");
		form.AddField("var2", "value2");
		WWW www = new WWW(url, form);
		
		StartCoroutine(WaitForRequest(www));
	}
	
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
			
			// check for errors
			if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.data);
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}   
}


