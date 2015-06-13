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

public class SendSocketIO : MonoBehaviour
{
	private SocketIOComponent socket;
	JSONObject test = new JSONObject();

	public Color color;
	
	public void Start() 
	{
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		
		socket.On("open", Open);
		socket.On("connect", Connect);
		socket.On("message", onMessage);

		socket.On("updateCloth", TestUpdateCloth);
		socket.On("setColor", SetColor);
		socket.On("error", TestError);
		socket.On("close", TestClose);
		
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
		message.AddField("cloth", "London");
		socket.Emit("message", message);

		yield return new WaitForSeconds(3);

		JSONObject message2 = new JSONObject();
		message2.AddField("client","webinterface");
		message2.AddField("action","changeCloth");
		message2.AddField("cloth", "Bob");
		socket.Emit("message", message2);
//		
//		// wait ONE FRAME and continue
				yield return null;
//		
//		socket.Emit("beep", test);
//		socket.Emit("beep", test);
	}

	public void Connect(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Connect received: " + e.name + " " + e.data);
		JSONObject connectDebug = new JSONObject();
		connectDebug.AddField("client", "unity");
		socket.Emit("connected", connectDebug);
	}
	
	public void Open(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}

	public void onMessage(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Message received");
		Debug.Log("[SocketIO] Message received: " + e.name + " " + e.data);
		string client = e.data.GetField("client").str;
		if(client.Equals("unity")){
			string action = e.data.GetField("action").str;
			if(action.Equals("changeImg")){



			}
		}



		string url = "http://www.fragment.in/unity/img/cff.png";
	}
	
	public void TestUpdateCloth(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Message received: " + e.name + " " + e.data);
		
		if (e.data == null) { return; }

		string client = e.data.GetField("client").str;
		if(client.Equals("unity")){
			Debug.Log ("unity is for me!!");
		}
		Debug.Log ("color : "+e.data.GetField("color").str);
		//Debug.Log ("brand : "+e.data.GetField("brand").str);
		color = ParseColor(e.data.GetField("color").str); 
		Debug.Log (color);


		
		
		
		
	}


	public Color ParseColor(string ColorToParse){
		//Takes strings formatted with rgb(red, green, blue);
		Debug.Log ("try to split");
		string[] isolate = ColorToParse.Split('(');
		string[] isolate2 = isolate[1].Split(')');
		Debug.Log ("isolate "+isolate2[0]);
		string[] colors = isolate2[0].Split(',');
//		int green = int.Parse(isolate.Split(","[1] ));
//      	int blue = int.Parse(isolate.Split(","[2] ));
//		

		//Color output = new Color(red,green,blue);
		Color output = new Color(int.Parse(colors[0]),int.Parse(colors[1]),int.Parse(colors[2]));
		Debug.Log ("Decoded : color  "+output);
		return output;
	}
	
	public void SetColor(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Message received: " + e.name + " " + e.data);
		
		if (e.data == null) { return; }

		string client = e.data.GetField("client").str;
		if(client.Equals("unity")){
			Debug.Log ("unity is for me!!");
			Debug.Log ("color : "+e.data.GetField("color").str);
		}


	}
	
	public void TestError(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e)
	{	
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}
}
