using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.Controller;
using Com.BehaviourManamger;

namespace Com.Usertype.Manual {
	public interface Manual {

		//等待用户选择
		void diceReady();
		void planeReady(int thisUser);

		//转色子,返回点数(1 - 6)
		int turnDice();
		
		//选择飞机,返回飞机编号(0 - 3)
		int choosePlane ();
		
	}

	public static class ManualBuffer {
		public static bool diceDone = false;
		public static bool planeDone = false;
		public static int userNum;
		public static int planeNum;
		public static int diceNum;
	}

	public class ManualRule : Manual{
		
		public static ManualRule instance = null;
		public static The_Action action;
		
		private ManualRule() {
			action = new Behaviour_manager ();
		}
		
		public static ManualRule getInstance() {
			if (instance == null) {
				instance = new ManualRule ();
			}
			return instance;
		}

		public void diceReady() {
			ManualBuffer.diceDone = false;
		}
		public void planeReady(int thisUser) {
			ManualBuffer.userNum = thisUser;
			ManualBuffer.planeDone = false;
		}

		//返回骰子点数（1 - 6）
		public int turnDice() {
			return ManualBuffer.diceNum;
			
		}
		
		//返回飞机编号（0 - 3）
		public int choosePlane() {
			return ManualBuffer.planeNum;
		}
	}

	public class ManualInput {
		public static ManualInput instance = null;
		private static The_Action action;
		private ManualInput() {
			action = new Behaviour_manager ();
		}

		public static ManualInput getInstance() {
			if (instance == null) {
				instance = new ManualInput();
			}
			return instance;
		}

		public void inputDice() {
			if (ManualBuffer.diceDone == false) {
				ManualBuffer.diceNum = action.turn_dice();
				ManualBuffer.diceDone = true;
			}
		}

		public void inputPlane(int thisUser, int thisPlane) {
			if (ManualBuffer.planeDone == false) {
				if (ManualBuffer.userNum == thisUser) {
					if (ManualBuffer.diceNum != 6 && action.planeAtHome(thisUser, thisPlane))
						return;
					ManualBuffer.planeNum = thisPlane;
					ManualBuffer.planeDone = true;
				}
			}
		}
	}
	
}