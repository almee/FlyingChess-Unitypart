using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.BehaviourManamger;

namespace Com.Usertype.AI {
	public interface AI {

		//转色子,返回点数(1 - 6)
		int turnDice();

		//选择飞机,返回飞机编号(0 - 3)
		int choosePlane (int thisUser, int diceNum);

	}

	public class AIRule : AI{

		public static AIRule instance;
		public static The_Action action;

		private AIRule() {
			action = new Behaviour_manager ();
		}

		public static AIRule getInstance() {
			if (instance == null) {
				instance = new AIRule ();
			}
			return instance;
		}

		/*
		 * 返回骰子点数（1 - 6）
		 * */
		public int turnDice() {
			return action.turn_dice ();
		}

		/* 选择一个可以移动的飞机
		 * 返回飞机编号（0 - 3）
		 * 如果没有飞机可以移动
		 * 返回 -1
		 * */
		public int choosePlane(int thisUser, int diceNum) {
			int index = 4 * thisUser;
			if (diceNum == 6) {
				for (int i = 0; i < 4; i++) {
					if (!action.planeDone(thisUser, i)) {
							return i;		//要的是planeNum
					}
				}
				return -1;
			} else {
				for (int i = 0; i < 4; i++) {
					if (!action.planeAtHome(thisUser, i) && !action.planeDone(thisUser, i)) {
						return i;
					}
				}
				return -1;
			}
		}
	}

}