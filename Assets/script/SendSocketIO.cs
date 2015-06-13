#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections;
using UnityEngine;
using SocketIO;
using System;

public class SendSocketIO : MonoBehaviour
{
	private SocketIOComponent socket;
	JSONObject test = new JSONObject();

	//*IMPORTANT* A remplacer par le bon objet !
	public Color color;

	//*IMPORTANT* A remplacer par le bon objet ! 
	public GameObject textureHolder;

	//Coroutine pour charger un logo
	IEnumerator LoadImg;
	
	public void Start() 
	{
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		
		socket.On("open", onOpen);
		socket.On("connect", onConnect);
		socket.On("message", onMessage);

		socket.On("error", onError);
		socket.On("close", onClose);
		
		StartCoroutine("BeepBoop");
		test.AddField("cloth", "London");
		Debug.Log ("test "+ test);
	}
	
	private IEnumerator BeepBoop()
	{
		// wait 1 seconds and continue
//		yield return new WaitForSeconds(1);
//		
//		socket.Emit("beep", test);
//		
//		// wait 3 seconds and continue
//		yield return new WaitForSeconds(3);
//		
//		socket.Emit("beep", test);
//		
//		// wait 2 seconds and continue
//		yield return new WaitForSeconds(2);
//		
//		socket.Emit("beep", test);
		yield return new WaitForSeconds(1);



		JSONObject message = new JSONObject();
		message.AddField("client","webinterface");
		message.AddField("action","changeCloth");
		message.AddField("cloth", "Steve");
		socket.Emit("message", message);

		yield return new WaitForSeconds(3);

		JSONObject message2 = new JSONObject();
		message2.AddField("client","webinterface");
		message2.AddField("action","changeCloth");
		message2.AddField("cloth", "Victor");
		socket.Emit("message", message2);
//		
//		// wait ONE FRAME and continue
				yield return null;
//		
//		socket.Emit("beep", test);
//		socket.Emit("beep", test);
	}

	public void onConnect(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Connect received: " + e.name + " " + e.data);
		JSONObject connectDebug = new JSONObject();
		connectDebug.AddField("client", "unity");
		socket.Emit("connected", connectDebug);
	}
	
	public void onOpen(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}

	public void onMessage(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Message received: " + e.name + " " + e.data);

		if (e.data == null) {
			Debug.Log("[SocketIO] Message contains no data / function stopped");
			return; 
		}

		string client = e.data.GetField("client").str;

		/*Self assigned destination*/
		if(client.Equals("unity")){
			string action = e.data.GetField("action").str;
			Debug.Log ("Action is  :  "+action);

			if(action.Equals("changeImg")){

				//string url = "http://www.fragment.in/unity/img/cff.png";
				//*IMPORTANT* A remplacer par le bon url !
				string baseUrl = "localhost:8888/switcher/server/";
				string imgUrl = e.data.GetField("url").str;

				WWW www = new WWW(baseUrl+imgUrl);

				Debug.Log ("Started change IMG : "+imgUrl);

				LoadImg = DownloadImage(www);
				StartCoroutine(LoadImg);
			}else if(action.Equals("changeColor")){

				//*IMPORTANT* A remplacer par le bon objet !
				color = ParseColor(e.data.GetField("color").str); 
				//Debug.Log(color);

			/*Set position first once */
			}else if(action.Equals("setPosition")){

				//*IMPORTANT* A remplacer par le bon objet !
				float x = float.Parse(e.data.GetField("x").str);
				float y = float.Parse(e.data.GetField("y").str);
				float size = float.Parse(e.data.GetField("size").str);
				bool inverted = bool.Parse(e.data.GetField("inverted").str);

				Debug.Log(" x : "+x+" / y : "+y+" / size : "+size+" / inverted : "+inverted);
			}
			/*Update position after */
			else if(action.Equals("updatePosition")){

				//*IMPORTANT* A remplacer par le bon objet !
				string position = e.data.GetField("position").str; 
				Debug.Log("position : "+position);
			}
			/*Take screenshot*/
			else if(action.Equals("takeScreenshot")){
				
				//*IMPORTANT* A remplacer par le bon objet !
				Debug.Log("take screenshot ");

				//Try and catch doesn't work properly
//				try{
//					Application.CaptureScreenshot("Resources/Screenshot.png");
//				}
//				catch (Exception en) {
//					print("catch an error : "+en);
//					string persistentDataPath = null;
//					if (persistentDataPath == null)
//						persistentDataPath = Application.persistentDataPath;        
//					Debug.Log("Data Path =  " + persistentDataPath); // If you want to easily see where that is.
//					print("changed data path");
//					Application.CaptureScreenshot(persistentDataPath+"Screenshot.png");
//				} 
//				Debug.Log("screenshot taken ");

				//save to persistent data path otherwise unity throw an error / because it is IOS ?
				string persistentDataPath = null;
				if (persistentDataPath == null)
					persistentDataPath = Application.persistentDataPath;        
				Debug.Log("Data Path =  " + persistentDataPath); // If you want to easily see where that is.
				Application.CaptureScreenshot(persistentDataPath+"Screenshot.png");
				Debug.Log("screenshot taken ");

			}
		}
	}

	IEnumerator DownloadImage(WWW wwwimg2){

		Debug.Log ("Started DownloadImage ");
		
		// Create a texture in DXT1 format
		textureHolder.GetComponent<Renderer>().material.mainTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);
		
		while(true) {

			// wait until the download is done
			yield return wwwimg2;
			
			// assign the downloaded image to the main texture of the object
			wwwimg2.LoadImageIntoTexture(textureHolder.GetComponent<Renderer>().material.mainTexture as Texture2D);

			Debug.Log ("Image replaced");
			//yield return null;
			StopCoroutine(LoadImg);
		}
		
	}
	


	static public float map(float value, float istart, float istop, float ostart, float ostop) {
		return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
	}
	
	
	static public Color ParseColor(string ColorToParse){
		//Takes strings formatted with rgb(red, green, blue);

		string[] isolate = ColorToParse.Split('(');
		string[] isolate2 = isolate[1].Split(')');

		string[] colors = isolate2[0].Split(',');

		//Debug.Log ("before Decoded slow : color  "+colors[0]+" , "+colors[1]+" , "+colors[2]);


		float red = map(float.Parse(colors[0]),0.0f,255.0f,0.0f,1.0f);
		float green = map(float.Parse(colors[1]),0.0f,255.0f,0.0f,1.0f);
		float blue = map(float.Parse(colors[2]),0.0f,255.0f,0.0f,1.0f);

		//Debug.Log ("Decoded slow : color  "+red+" , "+green+" , "+blue);

		//Color output = new Color(red,green,blue);
		Color output = new Color(red,green,blue);
		//Debug.Log ("Decoded : color  "+output);
		return output;
	}

	
	public void onError(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}
	
	public void onClose(SocketIOEvent e)
	{	
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}


	
}
