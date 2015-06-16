using System.Collections;
using UnityEngine;
using SocketIO;
using System.IO;
//using System;

public class SendSocketIO : MonoBehaviour
{
	private SocketIOComponent socket;
	JSONObject test = new JSONObject();

	//*IMPORTANT* A remplacer par le bon objet !
	public Color color;

	//*IMPORTANT* A remplacer par le bon objet ! 
	public GameObject textureHolder;
	Texture2D textureKeeper;

	GameObject currentCloth;
	public GameObject[] cloths;
	//Coroutine pour charger un logo
	IEnumerator LoadImg;
	//
	public void Start() 
	{
		//GameObject go = GameObject.Find("SocketIO");
		socket = GetComponent<SocketIOComponent>();
		
		socket.On("open", onOpen);
		socket.On("connect", onConnect);
		socket.On("message", onMessage);

		socket.On("error", onError);
		socket.On("close", onClose);
		
		//StartCoroutine("BeepBoop");
		test.AddField("cloth", "London");
		Debug.Log ("test "+ test);
	}

	void setCloth(string cloth) {
		JSONObject message = new JSONObject();
		message.AddField("client","webinterface");
		message.AddField("action","changeCloth");
		message.AddField("cloth", cloth);
		socket.Emit("message", message);
	}
	/*private IEnumerator BeepBoop()
	{
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
	}*/

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
				string baseUrl = "localhost:8888/";
				string imgUrl = e.data.GetField("url").str;

				WWW www = new WWW(baseUrl+imgUrl);

				Debug.Log ("Started change IMG : "+imgUrl);

				LoadImg = DownloadImage(www);
				StartCoroutine(LoadImg);
			} else if (action.Equals("changeColor")){

				//*IMPORTANT* A remplacer par le bon objet !
				color = ParseColor(e.data.GetField("color").str);
				foreach (GameObject go in cloths) {
					go.BroadcastMessage("changeColor", color, SendMessageOptions.DontRequireReceiver);
				}
				//Debug.Log("***********- "+color);

			/*Set position first once */
			} else if(action.Equals("changeCloth")) {

				string position = e.data.GetField("name").str;
				//Debug.Log("%%% CHANGE CLOTHHRTHH  "+position);
				foreach (GameObject go in cloths) {
					go.SetActive(false);
				}
				if(position.Equals("Bob")) {
					cloths[0].SetActive(true);
					cloths[5].SetActive(true);
				} else if (position.Equals("Steve")) {
					cloths[1].SetActive(true);
					cloths[6].SetActive(true);

				} else if(position.Equals("Victor")) {
					cloths[2].SetActive(true);
					cloths[7].SetActive(true);

				} else if(position.Equals("John")) {
					cloths[3].SetActive(true);
					cloths[8].SetActive(true);
					
				} else if(position.Equals("London")) {
					cloths[4].SetActive(true);
					cloths[9].SetActive(true);
					
				}
			} else if(action.Equals("setPosition")){


				string position = e.data.GetField("position").str;
				foreach (GameObject go in cloths) {
					go.BroadcastMessage("setPosition", position, SendMessageOptions.DontRequireReceiver);
				}
				//*IMPORTANT* A remplacer par le bon objet !
				/*float x = float.Parse(e.data.GetField("x").str);
				float y = float.Parse(e.data.GetField("y").str);
				float size = float.Parse(e.data.GetField("size").str);
				bool inverted = bool.Parse(e.data.GetField("inverted").str);

				Debug.Log(" x : "+x+" / y : "+y+" / size : "+size+" / inverted : "+inverted);*/
			}
			/*Update position after */
			else if(action.Equals("updatePosition")){

				//*IMPORTANT* A remplacer par le bon objet !
				string position = e.data.GetField("position").str; 
				Debug.Log("position : "+position);
				// SEND
				foreach (GameObject go in cloths) {
					go.BroadcastMessage("updatePosition", position, SendMessageOptions.DontRequireReceiver);
				}
			}
			/*Take screenshot*/
			else if(action.Equals("takeSnapshot")){
				
				//*IMPORTANT* A remplacer par le bon objet !
				Debug.Log("take screenshot YEAH");

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
				string uniqueName = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
				Debug.Log(uniqueName);
				string imgPath = "contacts/"+uniqueName+".png";
				//Debug.Log("$$$ "+Application.dataPath);
				Application.CaptureScreenshot(imgPath);
				//
				//
				string fileName = "contacts/contacts.txt";
				//File.WriteAllLines();
				string entreprise = e.data.GetField("entreprise").str; 
				string nom = e.data.GetField("nom").str; 
				string prenom = e.data.GetField("prenom").str; 
				string email = e.data.GetField("email").str;

				File.AppendAllText(fileName, uniqueName+"*#*"+entreprise+"*#*"+nom+"*#*"+prenom+"*#*"+email+"\n");

				//Debug.Log("#########: "+Application.dataPath);
				// SEND TO SERVER AND MAIL !!


				StartCoroutine(UploadPNG(nom, prenom, entreprise, email, uniqueName));
				/*string persistentDataPath = null;
				if (persistentDataPath == null)
					persistentDataPath = Application.persistentDataPath;        
				Debug.Log("Data Path =  " + persistentDataPath); // If you want to easily see where that is.
				Application.CaptureScreenshot(persistentDataPath+"Screenshot.png");
				Debug.Log("screenshot taken ");*/

			} else if(action.Equals("changeCloth")){
				
				//*IMPORTANT* A remplacer par le bon objet !
				string name = e.data.GetField("name").str; 
				Debug.Log("name : "+name);

				if(currentCloth) {
					currentCloth.SetActive(false);
				}

				//currentCloth = GameObject.Find(name+"Target") as GameObject;
				int r = Random.Range(0, cloths.Length);
				currentCloth = cloths[r];
				Debug.Log("***CURRENT: "+currentCloth);
				currentCloth.SetActive(true);
				//GameObject cloth = 
			}
		}
	}
	IEnumerator UploadPNG(string nom, string prenom, string entreprise, string email, string uniqueName) {
		// We should only read the screen after all rendering is complete
		yield return new WaitForEndOfFrame();
		
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		var tex = new Texture2D(width, height, TextureFormat.RGB24, false );
		
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Destroy( tex );
		
		// Create a Web Form
		WWWForm form = new WWWForm();
		//form.AddField("frameCount", Time.frameCount.ToString());
		form.AddField ("nom", nom);
		form.AddField ("prenom", prenom);
		form.AddField ("entreprise", entreprise);
		form.AddField ("email", email);
		form.AddField ("uniqueName", uniqueName);
		form.AddBinaryData("image", bytes, "screenShot.png", "image/png");
		
		// Upload to a cgi script
		WWW w = new WWW("http://www.fragment.in/wp/switcher/mail.php", form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			print(w.error);
		}
		else {
			print("Finished Uploading Screenshot");
			Debug.Log("!!!!! "+w.text);
		}
	}
	IEnumerator DownloadImage(WWW wwwimg2){

		Debug.Log ("Started DownloadImage ");
		
		// Create a texture in DXT1 format

		//textureHolder.GetComponent<Renderer>().material.mainTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);
		textureKeeper = new Texture2D(4, 4, TextureFormat.DXT1, false);

		// wait until the download is done
		yield return wwwimg2;
			
			// assign the downloaded image to the main texture of the object
		wwwimg2.LoadImageIntoTexture(textureKeeper);
		textureKeeper.wrapMode = TextureWrapMode.Clamp;
		textureKeeper.alphaIsTransparency = true;


			foreach (GameObject go in cloths) {
				go.BroadcastMessage("changeImg", textureKeeper, SendMessageOptions.DontRequireReceiver);
			}
			Debug.Log ("Image replaced");
			//yield return null;
			StopCoroutine(LoadImg);

		
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
