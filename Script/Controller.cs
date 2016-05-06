using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.Usertype.AI;
using Com.Usertype.Online;
using Com.Usertype.Manual;
using Com.ModelManamger;
using Com.Windows;
using Com.BehaviourManamger;
namespace Com.Controller {
	public class Controller {
		
		private int diceNum;
		private int userNum;
		private int planeNum;

		//标志位
		private bool running = false;
		private bool hasDice = false;
		private bool hasPlane = false;

		//外部方法
		private Game_object object_manager;
		private Chess_Position pos;
		private The_Action Action = new Behaviour_manager();
		private AI AI_instance;
		private Manual Manual_instance;
		private OnlinePlayer OnlinePlayer_instance;

		//单例
		public static Controller Controller_instance;

		/*
		 * 获取骰子点数
		 * 如果人工和网络的情况下，数据未就位返回false，不赋值
		 * 获取成功返回true，diceNum被赋值
		 * */
		private bool getDice() {
			switch (Game_object.userType [userNum]) {
			case 0:
				diceNum = AI_instance.turnDice();
				//在机器人投了骰子之后,选飞机之前如果取消了机器人,需要保存数据到人工选择上
				ManualBuffer.diceNum = diceNum; 
				break;
				
			case 1:
				if (ManualBuffer.diceDone == false) return false;
				diceNum = Manual_instance.turnDice();
					break;
				
			case 2:
				if (OnlineBuffer.diceDone == false) return false;
				diceNum = OnlinePlayer_instance.turnDice();
				break;
				
			case 3:
				//空白 跳过
				diceNum = -1;
				break;
				
			default:
				diceNum = -1;
				break;
			}
			hasDice = true;
			return true;
		}

		/*
		 * 获取飞机的编号
		 * 如果人工和网络的情况下，数据未就位返回false，不赋值
		 * 获取成功返回true，planeNum被赋值
		 * */
		private bool getPlane() {
			switch (Game_object.userType [userNum]) {
			case 0:
				planeNum = AI_instance.choosePlane(userNum, diceNum);
				break;
				
			case 1:
				if (ManualBuffer.planeDone == false) return false;
				planeNum = Manual_instance.choosePlane();
				break;
				
			case 2:
				if (OnlineBuffer.planeDone == false) return false;
				planeNum = OnlinePlayer_instance.choosePlane();
				break;
				
			case 3:
				//永远不会被执行
				break;
				
			default:
				//永远不会被执行
				break;
			}
			hasPlane = true;
			return true;
		}

		//飞机的移动
		private void action() {
			Debug.Log ("userNum:" + userNum);
			Debug.Log ("planeNum:" + planeNum);
			if (diceNum == 6) {
				//可以起飞
				if (Action.planeAtHome (userNum, planeNum)) {
					Action.Out_Home (4 * userNum + planeNum);
				} else {
					//飞机移动
					Action.Move (4 * userNum + planeNum, diceNum);
					reaction (userNum, planeNum);
				}
			} else {
				//飞机移动
				Action.Move (4 * userNum + planeNum, diceNum);
				reaction (userNum, planeNum);
			}
		}

		/*
		 * 统一：重置控制器内的标志位
		 *       重置骰子图案
		 *       给安卓传输这一步的数据
		 * AI： 不需要处理
		 * 本地： 将diceDone与planeDone标志位置为false
		 * 网络： 将doceDone与planeDone标志位置为false
		 * 空白： 不需要处理
		 * */
		private void ready(int thisUser) {
			if (Game_object.online) {
				AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
				AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("curentActivity");
				jo.Call ("SentNext", userNum, planeNum, diceNum);
			}

			switch (Game_object.userType [thisUser]) {
			case 0:
				break;

			case 1:
				ManualBuffer.diceDone = false;
				ManualBuffer.planeDone = false;
				break;

			case 2:
				OnlineBuffer.diceDone = false;
				OnlineBuffer.planeDone = false;
				break;

			case 3:
				//永远不会被执行
				break;

			default:
				//永远不会被执行
				break;
			}
			hasDice = false;
			hasPlane = false;
			resetDice ();
		}

		//重置骰子图案
		private void resetDice() {
			Action.reset_dice ();
		}

		//主程序， update中无限跑
		public void run() {
			if (!Game_object.started || running) return;
			running = true;
			if (hasDice == false) {
				if (getDice () == false) {
					running = false;
					return;
				}
			}
			if (diceNum == -1 || (diceNum != 6 && Action.allAtHome(userNum))) {
				nextUser();
				running = false;
			} else {
				if (hasPlane == false) {
					if (getPlane() == false) {
						running = false;
						return;
					}
				}
				if (planeNum == -2) {
					running = false;
					return;
				}
				action();
				if (diceNum != 6) {
					nextUser();
				} else {
					ready(userNum);
				}
				running = false;
			}
		}
		
		//切换用户
		private void nextUser() {
			Debug.Log ("nextUser!");
			ready(userNum);
			userNum++;
			if (userNum >= 4)
				userNum = 0;
			changePlane ();
			ManualBuffer.userNum = userNum;
			OnlineBuffer.userNum = userNum;
			if (Game_object.online && userNum == Game_object.whoAmI) {
				NewBehaviourScript.timeToEmit = 10;
				NewBehaviourScript.judging = true;
			} else {
				NewBehaviourScript.judging = false;
			}
			//resetDice();
		}

		//切换显示当前用户颜色使用的飞机
		private void changePlane() {
				Game_object object_manager = new Game_object ();
				for (int i = 0; i < 4; i++) {
					GameObject temp = GameObject.Find(object_manager.user[i]);
					temp.renderer.enabled = false;	
				}
				GameObject temp1 = GameObject.Find(object_manager.user[userNum]);
				temp1.renderer.enabled = true;	
		}

		private void reaction(int thisUser, int thisPlane) {
			Debug.Log ("thisuser:" + thisUser);
			Debug.Log ("thisPlane:" + thisPlane);
			Debug.Log ("pos:" +Game_object.subscript[4 * thisUser + thisPlane]);
			Debug.Log("global:" + Game_object.global[4 * thisUser + thisPlane]);
			reaction_Enemy (thisUser, thisPlane);
			if (reaction_Fly(thisUser, thisPlane)) {
				reaction_Enemy (thisUser, thisPlane);
				reaction_SomeColor (thisUser, thisPlane);
				reaction_Enemy (thisUser, thisPlane);
			} else if (reaction_SomeColor(thisUser, thisPlane)) {
				reaction_Enemy (thisUser, thisPlane);
				reaction_Fly (thisUser, thisPlane);
				reaction_Enemy (thisUser, thisPlane);
			}
			reaction_Done (thisUser, thisPlane);
		}
		
		private void reaction_Enemy(int thisUser, int thisPlane) {

			int here = Game_object.global[4 * thisUser + thisPlane];
			for (int i = 0; i < 16; i++) {
				if (4 * thisUser <= i && i < 4 * thisUser + 4)
					continue;
				if (Game_object.global[i] != -1 && here == Game_object.global[i]) {
					//打飞机
					Action.Back_Home(i);
					Debug.Log ("Enemy");
				}
			}
		}
		
		private bool reaction_Fly(int thisUser, int thisPlane)  {
			int here = Game_object.subscript[4 * thisUser + thisPlane];
			if (here == 18) {
				Debug.Log ("fly");
				Action.Fly (4 * thisUser + thisPlane);
				return true;
			}
			return false;
		}
		
		private bool reaction_SomeColor(int thisUser, int thisPlane) {
			int here = Game_object.subscript [4 * thisUser + thisPlane];
			if (here >= 50 || here == 0)
				return false;
			here -= 2;
			if (here % 4 == 0) {
				Debug.Log ("SomeColor");
				Action.Move (4 * thisUser + thisPlane, 4);
				return true;
			}
			return false;
		}
		
		private void reaction_Done(int thisUser, int thisPlane) {
			if (Game_object.subscript[4 * thisUser + thisPlane] == 56) {
				//到达终点
				Action.Done(4 * thisUser + thisPlane);
				Debug.Log ("done");
			}
		}
		
		public static Controller GetInstance() {
			if (Controller_instance == null) {
				Controller_instance = new Controller();

			}
			return Controller_instance;
		}
		
		private Controller() {
			userNum = 0;
			pos = new Chess_Position ();
			object_manager = new Game_object ();
			AI_instance = AIRule.getInstance();
			Manual_instance = ManualRule.getInstance ();
			OnlinePlayer_instance = OnlinePlayerRule.getInstance();
		}
	}
}