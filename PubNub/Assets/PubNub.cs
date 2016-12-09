using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PubNubMessaging.Core;
using UnityEngine.UI;

public class PubNub : MonoBehaviour {

	public UnityEngine.UI.Text sent;
	public UnityEngine.UI.Text recieved;
	public UnityEngine.UI.InputField message;
	public UnityEngine.UI.InputField publishChannel;
	public UnityEngine.UI.InputField subscribeChannel;
	public UnityEngine.UI.InputField unsubscribeChannel;


	Pubnub pubnub;
	string publishKey = "pub-c-ca77dae9-1ae7-4dab-9651-5d560c37f8c7";
	string subKey = "sub-c-73c8b964-bcbf-11e6-b737-0619f8945a4f";

	// Use this for initialization
	void Start () {
		pubnub = new Pubnub( publishKey, subKey);
		//Subscribe ("test");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Publish () {
		pubnub.Publish<string>(
			publishChannel.text,
			message.text,
			DisplayReturnMessage,
			DisplayErrorMessage
		);
	}

	public void Unsubscribe(){
		pubnub.Unsubscribe<string>(
			unsubscribeChannel.text,
			DisplayReturnMessage,
			DisplaySubscribeConnectStatusMessage,
			DisplaySubscribeDisconnectStatusMessage,
			DisplayErrorMessage
		);
	}

	public void Subscribe(){
		pubnub.Subscribe<string> (
			subscribeChannel.text,
			DisplaySubscribeReturnMessage,
			DisplaySubscribeConnectStatusMessage,
			DisplayErrorMessage
		);
	}

	void DisplayReturnMessage(string result)
	{
		UnityEngine.Debug.Log("PUBLISH STATUS CALLBACK");
		UnityEngine.Debug.Log(result);
		sent.text += "\n" + result;
	}

	void DisplaySubscribeDisconnectStatusMessage(string result)
	{
		UnityEngine.Debug.Log("UNSUBSCRIBE STATUS CALLBACK");
		UnityEngine.Debug.Log(result);
		recieved.text += "\n" + "Unsubscribed to: " + unsubscribeChannel.text;
	}

	void DisplayErrorMessage(PubnubClientError pubnubError) {
		UnityEngine.Debug.Log(pubnubError.StatusCode);
	}

	void DisplaySubscribeReturnMessage(string result) {
		recieved.text += "\n" + result;

		UnityEngine.Debug.Log("SUBSCRIBE REGULAR CALLBACK:"); 
		UnityEngine.Debug.Log(result);
		if (!string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(result.Trim()))
		{
			List<object> deserializedMessage = pubnub.JsonPluggableLibrary.DeserializeToListOfObject(result);
			if (deserializedMessage != null && deserializedMessage.Count > 0)
			{
				object subscribedObject = (object)deserializedMessage[0];
				if (subscribedObject != null)
				{
					//IF CUSTOM OBJECT IS EXCEPTED, YOU CAN CAST THIS OBJECT TO YOUR CUSTOM CLASS TYPE
					string resultActualMessage = pubnub.JsonPluggableLibrary.SerializeToJsonString(subscribedObject);
				}
			}
		}
	}

	void DisplaySubscribeConnectStatusMessage(string result)
	{
		UnityEngine.Debug.Log("SUBSCRIBE CONNECT CALLBACK");
		recieved.text += "\n" + "Subscribed to: " + subscribeChannel.text;
	}

	public void Quit(){
		Application.Quit();
	}

}
