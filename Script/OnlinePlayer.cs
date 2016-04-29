using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.BehaviourManamger;

namespace Com.Usertype.Online {

	public interface OnlinePlayer {

		//等待安卓传入数据
		void diceReady();
		void planeReady();

		//获取骰子的点数
		int turnDice();
		
		//获取飞机的编号
		int choosePlane ();
		
	}

	public static class OnlineBuffer {
		public static bool diceDone = false;
		public static bool planeDone = false;
		public static int userNum;
		public static int planeNum;
		public static int diceNum;
	}

	public class OnlinePlayerRule : OnlinePlayer{
		
		public static OnlinePlayerRule instance;
		
		private OnlinePlayerRule() {
		}
		
		public static OnlinePlayerRule getInstance() {
			if (instance == null) {
				instance = new OnlinePlayerRule ();
			}
			return instance;
		}

		public void diceReady() {
			OnlineBuffer.diceDone = false;
		}
		public void planeReady() {
			OnlineBuffer.planeDone = false;
		}

		//返回骰子点数（1 - 6）
		public int turnDice() {
			while (!OnlineBuffer.diceDone);
			return OnlineBuffer.diceNum;
		}
		
		//返回飞机编号（0 - 3）
		public int choosePlane() {
			while(!OnlineBuffer.planeDone);
			return OnlineBuffer.planeNum;
		}


	}
	
}