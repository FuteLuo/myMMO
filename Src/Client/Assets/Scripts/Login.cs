using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Network.NetClient.Instance.Init("127.0.0.1", 8000);
        Network.NetClient.Instance.Connect();

        SkillBridge.Message.NetMessage message = new SkillBridge.Message.NetMessage();
        message.Request = new SkillBridge.Message.NetMessageRequest();
        message.Request.firstRequest = new SkillBridge.Message.FirstTestRequest();
        message.Request.firstRequest.HelloWorld = "Hello World";
        Network.NetClient.Instance.SendMessage(message);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
