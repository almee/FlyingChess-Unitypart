using UnityEngine;
using Com.ModelManamger;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	float q = (float)(Screen.width * 0.3);
	float w = (float)(Screen.height * 0.3);
	float e = (float)(Screen.width * 0.4);
	float r = (float)(Screen.height * 0.4);
 
	float a = (float)(Screen.width * 0.4 * 0.3);
	float s = (float)(Screen.height * 0.3 * 0.4);
	float d = (float)(Screen.width * 0.4 * 0.4);
	float f = (float)(Screen.height * 0.4 * 0.4);

	bool showed = false;
	void over(int id) {
		if (GUI.Button(new Rect(a, s, d, f), "退出游戏")) {

			AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("curentActivity");
			jo.Call ("finishGame");
		}
	}
	void start(int id) {
		if (GUI.Button(new Rect(a, s, d, f), "开始游戏")) {
			showed = true;
		}
	}

	void OnGUI()
	{
		if(!showed) {
			GUI.Window (0, new Rect (q, w, e, r), start, "游戏要开始了", );
		}
		if (Game_object.over) {
			GUI.Window (0, new Rect (q, w, e, r), over, "游戏结束");
		}
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
