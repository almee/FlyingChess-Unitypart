using UnityEngine;
using UnityEngine.UI;
using Com.ModelManamger;
using System.Collections;

namespace Com.Windows {
public class NewBehaviourScript : MonoBehaviour {

	float q = (float)(Screen.width * 0.3);
	float w = (float)(Screen.height * 0.3);
	float e = (float)(Screen.width * 0.4);
	float r = (float)(Screen.height * 0.4);
 
	float a = (float)(Screen.width * 0.4 * 0.3);
	float s = (float)(Screen.height * 0.3 * 0.4);
	float d = (float)(Screen.width * 0.4 * 0.4);
	float f = (float)(Screen.height * 0.4 * 0.4);

	
	static public bool judging = false;
	static public float timeToEmit = -1;


	private bool roboting = false;
	public long startTime;
	private long fixedTime = 10;
	private long nowTime;

	bool showed = false;
	
	static GameObject text1, text2;

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
	void cancel(int id) {
		if (GUI.Button(new Rect(a, s, d, f), "老子自己来")) {
			roboting = false;
			Game_object.userType[Game_object.whoAmI] = 1;
		}
	}

	void OnGUI()
	{
		if (roboting) {
			GUI.Window (0, new Rect (q, w, e, r), cancel, "阿法狗正在辛劳地帮你玩飞行棋");
		}
		if(!showed) {
			GUI.Window (1, new Rect (q, w, e, r), start, "游戏要开始了");
		}
		if (Game_object.over) {
			GUI.Window (2, new Rect (q, w, e, r), over, "游戏结束");
		}
	}
	// Use this for initialization
	void Start () {

			text1 = GameObject.Find ("Text");
			text2 = GameObject.Find ("Text2");
			text1.SetActive(false);
			text2.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		if (judging) {
			text1.SetActive(true);
			text2.SetActive(true);
			if (timeToEmit > 0) {
				timeToEmit -= Time.deltaTime;
				Text[] a = text2.GetComponents<Text>();
				a[0].text = timeToEmit.ToString();
			} else {
				judging = false;
				Game_object.userType[Game_object.whoAmI] = 0;
				roboting = true;
			}
		} else {
			text1.SetActive(false);
			text2.SetActive(false);
		}
	}
}
}