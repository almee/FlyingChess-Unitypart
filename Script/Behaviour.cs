using UnityEngine;
using System.Collections;
using Com.ModelManamger;

namespace Com.BehaviourManamger {

	public interface The_Action {
		//此函数用于起飞，将飞机放置在起点, i 代表是哪一只飞机
		void Out_Home(int i);
		/*i 代表哪一个飞机，num代表移动的步数， 返回值是新的位置的下标，第一次运行后可以判断该值，首先是必须小于50， 如果该下标是18， 那么位于可飞跃区，可调用我的fly函数，
		 * 如果（返回值+2）%4 == 0， 证明处于同色区，可再一次调用move函数
		 * */
		int Move(int i, int num);
		//该函数用于那四条特殊轨道的飞行
		void Fly (int i);
		/*
		 、其他功能函数暂时没写，随时可以在此处补充
		 */
		//转色子，返回数字
		int turn_dice();
		//胜利回家  ps: 我怎么觉得被撞回家也是这个函数
		void Back_Home(int i);

		//胜利回家
		void Done(int i);

		bool allAtHome (int thisUser);

		bool planeAtHome(int thisUser, int thisPlane);

		bool planeDone(int thisUser, int thisPlane);

		void reset_dice();
	}

	public class Behaviour_manager : The_Action {

		//public Game_object object_manager = Game_object.GetInstance();
		Game_object object_manager = new Game_object ();


		public void Out_Home(int i) {
			GameObject this_one = GameObject.Find( object_manager.plane [i]);
			int temp = i / 4;
			Chess_Position pos = new Chess_Position ();
			Vector2 te = new Vector2 (0.6f, 4.2f);
			if (temp == 0) {
				//print("good");
				this_one.transform.position = te;
			} else {
				this_one.transform.position = pos.position [temp, 0];
			}
			Game_object.subscript [i] = 0;
			Game_object.global [i] = -1;
			//System.Console.WriteLine (" " + pos.position [temp, 0].x + " " + pos.position [temp, 0].y);
		}

		public int Move(int i, int num) {
			GameObject this_one = GameObject.Find( object_manager.plane [i]);
			int low_pos = Game_object.subscript[i];
			int new_pos = low_pos + num;
			Chess_Position pos = new Chess_Position ();
			if (new_pos > 56) {
					Vector2 temp = pos.position [i/4, 56];
					float x = temp.x;
					float y = temp.y;
					Vector3 temp1 = new Vector3 (x, y, 0f);
					iTween.MoveTo (this_one, temp1, (56 - low_pos) / 2);
					int turn_num = 56 - (new_pos - 56);
					temp1.x = pos.position [i, turn_num].x;
					temp1.y = pos.position [i, turn_num].y;
					iTween.MoveTo (this_one, temp1, (new_pos - 56) / 2);
					Debug.Log ("newPos:" + turn_num);
					return turn_num;
				} else {
					Vector2 temp = pos.position [i/4, new_pos];
					float x = temp.x;
					float y = temp.y;
					Vector3 temp1 = new Vector3 (x, y, 0f);
					iTween.MoveTo (this_one, temp1, (new_pos - low_pos) / 2);
					//return new_pos;
					Game_object.subscript[i] = new_pos;

				if (new_pos < 51)
				    	Game_object.global[i] = (new_pos + 13 * (i / 4)) % 52;
					else
						Game_object.global[i] = -1;
					return new_pos;
			}
		}

		public void Fly (int i) {
			GameObject this_one = GameObject.Find( object_manager.plane [i]);
			Chess_Position pos = new Chess_Position ();
			Vector2 temp = pos.position [i/4, 30];
			float x = temp.x;
			float y = temp.y;
			Vector3 temp1 = new Vector3 (x, y, 0f);
			iTween.MoveTo (this_one, temp1, 2);
			Game_object.subscript[i] = 30;
			Game_object.global[i] = (30 + 13 * (i / 4)) % 52;
		}

		public void reset_dice() {

			Game_object object_manager = new Game_object ();
			for (int i = 0; i < 10; i++) {
				GameObject temp = GameObject.Find(object_manager.dice[i]);
				temp.renderer.enabled = false;	
			}
			
			for (int i = 0; i <= 9; i++) {
				GameObject temp;
				if(i > 0) {
					temp = GameObject.Find(object_manager.dice[i-1]);
					temp.renderer.enabled = false;
				}
				temp = GameObject.Find(object_manager.dice[i]);
				temp.renderer.enabled = true;	
			}
			GameObject temp1 = GameObject.Find(object_manager.dice[9]);
			temp1.renderer.enabled = false;	
			temp1 = GameObject.Find(object_manager.dice[6]);
			temp1.renderer.enabled = true;	
		}

		public int turn_dice() {
			System.Random ran=new System.Random();
			int turn_num=ran.Next(0,6);
			/*Random ran=new Random();
			//Random.Range(1,7);
			int turn_num = */
			//GameObject temp = GameObject.Find("dice_1");
			//print ("dices" + turn_num);
			Game_object object_manager = new Game_object ();
			for (int i = 0; i < 10; i++) {
				GameObject temp = GameObject.Find(object_manager.dice[i]);
				temp.renderer.enabled = false;	
				//object_manager.dice[i].renderer.enabled = false;	
			}
			
			for (int i = 0; i <= 9; i++) {
				GameObject temp;
				if(i > 0) {
					temp = GameObject.Find(object_manager.dice[i-1]);
					temp.renderer.enabled = false;
					//System.Threading.Thread.Sleep(5);
				}
				temp = GameObject.Find(object_manager.dice[i]);
				temp.renderer.enabled = true;	
				//timeDelay(50);
				//Thread.Sleep(1000);
				//object_manager.dice[i].renderer.enabled = true;
			}
			GameObject temp1 = GameObject.Find(object_manager.dice[9]);
			temp1.renderer.enabled = false;	
			temp1 = GameObject.Find(object_manager.dice[turn_num]);
			temp1.renderer.enabled = true;	
			//object_manager.dice[9].renderer.enabled = false;
			//object_manager.dice[turn_num].renderer.enabled = true;
			return turn_num + 1;
		}



		public void Back_Home(int i) {
			GameObject this_one = GameObject.Find( object_manager.plane [i]);
			int temp = i / 4;
			int temp1 = i % 4;
			Chess_Position pos = new Chess_Position ();
			//Vector2 te = new Vector2 (0.6f, 4.2f);

			Vector2 new_p =  pos.position [temp, 57+temp1];
			float x = new_p.x;
			float y = new_p.y;
			Vector3 temp_ = new Vector3 (x, y, 0f);
			iTween.MoveTo (this_one, temp_, 2);
			Game_object.subscript [i] = 57+temp1;
			Game_object.global [i] = -1;
		}

		public void Done(int i) {
				Back_Home (i);
				Game_object.done [i] = true;

				GameObject this_one = GameObject.Find( object_manager.plane [i]);
			    this_one.transform.Rotate(new Vector3(180, 180, 0));

		}

		//判断是否所有的飞机都在家
		public bool allAtHome(int thisUser) {
			return Game_object.subscript[4 * thisUser + 0] == (57 + 0)
				&& Game_object.subscript[4 * thisUser + 1] == (57 + 1)
					&& Game_object.subscript[4 * thisUser + 2] == (57 + 2)
					&& Game_object.subscript[4 * thisUser + 3] == (57 + 3);
		}

		//确认选择的飞机是否在家
		public bool planeAtHome(int thisUser, int thisPlane) {
			for (int i = 57; i < 61; i++) {
				if (Game_object.subscript[4 * thisUser + thisPlane] == (57 + thisPlane))
					return true;
			}
			return false;
		}
		//判断飞机是否已经完成
		public bool planeDone(int thisUser, int thisPlane) {
			return Game_object.done [thisUser * 4 + thisPlane];
		}

	}
}
