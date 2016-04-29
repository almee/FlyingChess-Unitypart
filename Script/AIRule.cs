using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.BehaviourManamger;
using Com.ModelManamger;

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
			if (diceNum == 6) {  // 如果投到6，优先起飞一架飞机
				for (int i = 0; i < 4; i++) {
					if (!action.planeDone(thisUser, i) && action.planeAtHome(thisUser, i)) {
						return i;
					}
				}
			}

			// 如果投的不是6，或者飞机都起飞了
			int[] priority = new int[4];
			for (int i = 0; i < 4; i++) {
				if (action.planeAtHome(thisUser, i) || action.planeDone(thisUser, i)) {
					// 无法移动的话优先级为-1
					priority[i] = -1;
					continue;
				}

				int globalDes = Game_object.global[4 * thisUser + i] + diceNum;
				int subscriptDes = Game_object.subscript[4 * thisUser + i] + diceNum;

				// 能飞的话优先级加4
				if (subscriptDes == 14 || subscriptDes == 18) priority[i] += 4;

				for (int j = 0; j < 16; j++) {
					if (4 * thisUser <= j && j < 4 * thisUser + 4)
						continue;
					if (Game_object.global[j] != -1 && globalDes == Game_object.global[j]) {
						// 能吃对手飞机的话优先级加3
						priority[i] += 3;
						break;
					}
				}

				// 能跳的话优先级加2
				if (subscriptDes % 4 == 2 && (subscriptDes != 14 && subscriptDes != 18)) priority[i] += 2;
			}

			int maxPrio = -1, choice = -1;
			for (int i = 0; i < 4; i++) {
				if (maxPrio < priority[i]) {
					maxPrio = priority[i];
					choice = i;
				}
			}
			 
			return choice;
		}
	}

}