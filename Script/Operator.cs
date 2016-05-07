using UnityEngine;
using System.Collections;
using Com.Controller;
using Com.ModelManamger;
using Com.Usertype.Online;
public class Operator : MonoBehaviour {
	private Controller controller;
	// Use this for initialization
	void Start () {
		this.name = "Manager";
		controller = Controller.GetInstance ();

		/*AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("curentActivity");
		jo.Call ("ready");*/

	}
	
	// Update is called once per frame
	void Update () {
		//controller.run ();
        StartCoroutine(controller.run());
	}

	bool startGame(string input) {
		if (Game_object.started) return false;
		string[] users = input.Split (';');
		int user1 = int.Parse (users [0]);
		int user2 = int.Parse (users [1]);
		int user3 = int.Parse (users [2]);
		int user4 = int.Parse (users [3]);

		if (user4 == 2 || user2 == 2 || user3 == 2 || user4 == 2) Game_object.online = true;

		if (user1 == 1) Game_object.whoAmI = user1;
		if (user2 == 1) Game_object.whoAmI = user2;
		if (user3 == 1) Game_object.whoAmI = user3;
		if (user4 == 1) Game_object.whoAmI = user4;

		Game_object.started = true;
		Game_object.userType [0] = user1;
		Game_object.userType [1] = user2;
		Game_object.userType [2] = user3;
		Game_object.userType [3] = user4;
		return true;
	}

	bool SetNext(string input) {
		string[] data = input.Split (';');
		int thisUser = int.Parse (data [0]);
		int thisPlane = int.Parse (data [1]);
		int thisDice = int.Parse (data [2]);
		if (OnlineBuffer.userNum != thisUser) return false;
		while (OnlineBuffer.diceDone);
		OnlineBuffer.diceNum = thisDice;
		OnlineBuffer.diceDone = true;
		OnlineBuffer.planeNum = thisPlane;
		OnlineBuffer.planeDone = true;
		return true;
	}

	bool dropOut(int thisUser) {
		if (Game_object.userType [thisUser] == 1) return false;
		Game_object.userType [thisUser] = 3;
		return true;
	}
}
