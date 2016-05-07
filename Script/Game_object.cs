using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Com.ModelManamger {
	public struct Number {
		public int userNum;
		public int planeNum;
		public Number(int userNum_, int planeNum_) {
			userNum = userNum_;
			planeNum = planeNum_;
		}
	}

	public class Game_object : MonoBehaviour{
		//public static Game_object Game_object_instance;
		//use to save the plane
		public string[] user = new string[] {"red", "blue", "yellow", "green"};
		public string[] plane = new string[] {"r1", "r2", "r3", "r4", "b1", "b2", "b3", "b4", "y1", "y2", "y3","y4",
		"g1", "g2", "g3", "g4"};


		//在线游戏标记本机的号码
		public static int whoAmI = -1;

		//标记游戏是否结束
		public static bool over = false;

		//标记是否联机
		public static bool online = false;

		//标记是否已经开始游戏
		public static bool started = false;

		//标记用户类型
		public static int[] userType = new int[] {1 ,1 ,1 ,1};

		//用于存储每个对象所处的位置的坐标，避免每次遍历的麻烦；
		public static int[] subscript = new int[] {57, 58, 59, 60, 57, 58, 59, 60, 57, 58, 59, 60, 57, 58, 59, 60};

		//储存绝对位置，从红色方门口为1，绕一圈累加；其它位置都为-1
		public static int[] global = new int[] {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 ,-1 ,-1 };

		//记录飞机是否已经到达
		public static bool[] done = new bool[] {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};

		//用来根据名字获取用户编号与飞机编号
		public static Dictionary<string, Number> Name_Get_Number = new Dictionary<string, Number>();

		public Game_object() {
			if (Name_Get_Number.Count == 0) {
								Name_Get_Number.Add ("r1", new Number (0, 0));
								Name_Get_Number.Add ("r2", new Number (0, 1));
								Name_Get_Number.Add ("r3", new Number (0, 2));
								Name_Get_Number.Add ("r4", new Number (0, 3));
			
								Name_Get_Number.Add ("b1", new Number (1, 0));
								Name_Get_Number.Add ("b2", new Number (1, 1));
								Name_Get_Number.Add ("b3", new Number (1, 2));
								Name_Get_Number.Add ("b4", new Number (1, 3));
			
								Name_Get_Number.Add ("y1", new Number (2, 0));
								Name_Get_Number.Add ("y2", new Number (2, 1));
								Name_Get_Number.Add ("y3", new Number (2, 2));
								Name_Get_Number.Add ("y4", new Number (2, 3));
			
								Name_Get_Number.Add ("g1", new Number (3, 0));
								Name_Get_Number.Add ("g2", new Number (3, 1));
								Name_Get_Number.Add ("g3", new Number (3, 2));
								Name_Get_Number.Add ("g4", new Number (3, 3));
						}
		}
		//use to save the dice;
		public string[] dice = new string[]{"dice_1", "dice_2", "dice_3", "dice_4", "dice_5", "dice_6", "dice_action_0", "dice_action_1",
			"dice_action_2", "dice_action_3"};
 }

	public class Chess_Position {
		//use to save the position
		public Vector2[,] position = new Vector2[, ]{{new Vector2(0.6f, 5.7f),new Vector2(0f, 3.8f), new Vector2(0f, 3.2f), new Vector2(0f, 2.7f), new Vector2(0f, 2f),
				new Vector2(0.37f, 1.66f), new Vector2(1f, 1.86f), new Vector2(1.57f, 1.88f), new Vector2(2.12f, 1.68f), new Vector2(2.3f, 1f), new Vector2(2.3f, 0.56f),
				new Vector2(2.3f, 0f), new Vector2(2.3f, -0.56f), new Vector2(2.3f, -1f), new Vector2(2.12f, -1.68f), new Vector2(1.57f, -1.88f), new Vector2(1f, -1.86f), 
				new Vector2(0.37f, -1.66f), new Vector2(0f, -2f), new Vector2(0f, -2.7f), new Vector2(0f, -3.2f), /*负号*/new Vector2(0f, -3.8f), new Vector2(-0.6f, -4f), new Vector2(-1.1f, -4f),
				new Vector2(-1.6f, -4f), new Vector2(-2f, -4f), new Vector2(-2.7f, -4f), new Vector2(-3.4f, -3.8f), new Vector2(-3.4f, -3.2f), new Vector2(-3.4f, -2.7f), 
				new Vector2 (-3.3f, -2f), new Vector2 (-3.7f, -1.7f), new Vector2 (-4.3f, -1.9f), new Vector2 (-4.8f, -1.9f), new Vector2 (-5.5f, -1.7f), new Vector2 (-5.6f, -1f),
				new Vector2 (-5.7f, -0.5f), new Vector2 (-5.7f, 0f), new Vector2 (-5.7f, 0.5f), new Vector2 (-5.7f, 1f), new Vector2 (-5.5f, 1.7f), new Vector2 (-4.8f, 1.9f), 
				new Vector2 (-4.3f, 1.9f), new Vector2 (-3.7f, 1.7f), new Vector2 (-3.3f, 2f), new Vector2 (-3.4f, 2.7f), new Vector2 (-3.5f, 3.2f), new Vector2 (-3.4f, 3.85f),
				new Vector2 (-2.7f, 4f), new Vector2 (-2.9f, 4f), new Vector2 (-1.6f, 4f),     new Vector2 (-1.6f, 3.2f), new Vector2 (-1.6f, 2.6f), new Vector2 (-1.6f, 2f), new Vector2 (-1.6f, 1.6f),
				new Vector2 (-1.6f, 1f), new Vector2 (-1.6f, 0.4f), new Vector2 (2.3f, 4.05f), new Vector2 (1.4f, 4f), new Vector2 (1.39f, 3.1f), new Vector2 (2.39f, 3.1f)},
			{new Vector2 (2.5f, -2.2f),  new Vector2(2.12f, -1.68f), new Vector2(1.57f, -1.88f), new Vector2(1f, -1.86f), new Vector2(0.37f, -1.66f), new Vector2(0f, -2f), new Vector2(0f, -2.7f), new Vector2(0f, -3.2f), new Vector2(0f, -3.8f), new Vector2(-0.6f, -4f), new Vector2(-1.1f, -4f),
				new Vector2(-1.6f, -4f), new Vector2(-2f, -4f), new Vector2(-2.7f, -4f), new Vector2(-3.4f, -3.8f), new Vector2(-3.4f, -3.2f), new Vector2(-3.4f, -2.7f), 
				new Vector2 (-3.3f, -2f), new Vector2 (-3.7f, -1.7f), new Vector2 (-4.3f, -1.9f), new Vector2 (-4.8f, -1.9f), new Vector2 (-5.5f, -1.7f), new Vector2 (-5.6f, -1f),
				new Vector2 (-5.7f, -0.5f), new Vector2 (-5.7f, 0f), new Vector2 (-5.7f, 0.5f), new Vector2 (-5.7f, 1f), new Vector2 (-5.5f, 1.7f), new Vector2 (-4.8f, 1.9f), 
				new Vector2 (-4.3f, 1.9f), new Vector2 (-3.7f, 1.7f), new Vector2 (-3.3f, 2f), new Vector2 (-3.4f, 2.7f), new Vector2 (-3.5f, 3.2f), new Vector2 (-3.4f, 3.85f),
				new Vector2 (-2.7f, 4f), new Vector2 (-2.9f, 4f), new Vector2 (-1.6f, 4f), new Vector2 (-1.1f, 4.1f), new Vector2 (-0.6f, 4.1f),new Vector2(0f, 3.8f), new Vector2(0f, 3.2f), new Vector2(0f, 2.7f), new Vector2(0f, 2f),
				new Vector2(0.37f, 1.66f), new Vector2(1f, 1.86f), new Vector2(1.57f, 1.88f), new Vector2(2.12f, 1.68f), new Vector2(2.3f, 1f), new Vector2(2.3f, 0.56f),
				new Vector2(2.3f, 0f), new Vector2 (1.4f, 0f), new Vector2 (0.9f, 0f), new Vector2 (0.42f, 0f), new Vector2 (0f, 0f), new Vector2 (-0.57f, 0f), new Vector2(-1.1f, 0f),
				new Vector2 (2.3f, -3f), new Vector2 (1.4f, -3.0f), new Vector2 (1.4f, -3.9f), new Vector2 (2.4f, -4.0f)}, 
			{new Vector2 (-3.8f, -4.3f), new Vector2(-3.4f, -3.8f), new Vector2(-3.4f, -3.2f), new Vector2(-3.4f, -2.7f), 
				new Vector2 (-3.3f, -2f), new Vector2 (-3.7f, -1.7f), new Vector2 (-4.3f, -1.9f), new Vector2 (-4.8f, -1.9f), new Vector2 (-5.5f, -1.7f), new Vector2 (-5.6f, -1f),
				new Vector2 (-5.7f, -0.5f), new Vector2 (-5.7f, 0f), new Vector2 (-5.7f, 0.5f), new Vector2 (-5.7f, 1f), new Vector2 (-5.5f, 1.7f), new Vector2 (-4.8f, 1.9f), 
				new Vector2 (-4.3f, 1.9f), new Vector2 (-3.7f, 1.7f), new Vector2 (-3.3f, 2f), new Vector2 (-3.4f, 2.7f), new Vector2 (-3.5f, 3.2f), new Vector2 (-3.4f, 3.85f),
				new Vector2 (-2.7f, 4f), new Vector2 (-2.9f, 4f), new Vector2 (-1.6f, 4f),new Vector2 (-1.1f, 4.1f), new Vector2 (-0.6f, 4.1f),/*负号*/new Vector2(0f, -3.8f), new Vector2(0f, 3.2f), new Vector2(0f, 2.7f), new Vector2(0f, 2f),
				new Vector2(0.37f, 1.66f), new Vector2(1f, 1.86f), new Vector2(1.57f, 1.88f), new Vector2(2.12f, 1.68f), new Vector2(2.3f, 1f), new Vector2(2.3f, 0.56f),
				new Vector2(2.3f, 0f), new Vector2(2.3f, -0.56f), new Vector2(2.3f, -1f), new Vector2(2.12f, -1.68f), new Vector2(1.57f, -1.88f), new Vector2(1f, -1.86f), 
				new Vector2(0.37f, -1.66f), new Vector2(0f, -2f), new Vector2(0f, -2.7f), new Vector2(0f, -3.2f), new Vector2(0f, -3.8f), new Vector2(-0.6f, -4f), new Vector2(-1.1f, -4f),
				new Vector2(-1.6f, -4f), new Vector2 (-1.6f, -3.2f), new Vector2 (-1.6f, -2.68f), new Vector2 (-1.6f, -2.0f), new Vector2 (-1.6f, -1.6f), new Vector2 (-1.6f, 1f),
				new Vector2 (-1.6f, -0.4f), new Vector2 (-5.7f, -3.0f), new Vector2 (-4.7f, -3.0f), new Vector2 (-5.7f, -4f), new Vector2 (-4.7f, -4f)}, 
			{new Vector2 (-5.8f, 2.26f), new Vector2 (-5.5f, 1.7f), new Vector2 (-4.8f, 1.9f), 
				new Vector2 (-4.3f, 1.9f), new Vector2 (-3.7f, 1.7f), new Vector2 (-3.3f, 2f), new Vector2 (-3.4f, 2.7f), new Vector2 (-3.5f, 3.2f), new Vector2 (-3.4f, 3.85f),
				new Vector2 (-2.7f, 4f), new Vector2 (-2.9f, 4f), new Vector2 (-1.6f, 4f),new Vector2 (-1.1f, 4.1f), new Vector2 (-0.6f, 4.1f),new Vector2(0f, 3.8f), new Vector2(0f, 3.2f), new Vector2(0f, 2.7f), new Vector2(0f, 2f),
				new Vector2(0.37f, 1.66f), new Vector2(1f, 1.86f), new Vector2(1.57f, 1.88f), new Vector2(2.12f, 1.68f), new Vector2(2.3f, 1f), new Vector2(2.3f, 0.56f),
				new Vector2(2.3f, 0f), new Vector2(2.3f, -0.56f), new Vector2(2.3f, -1f), new Vector2(2.12f, -1.68f), new Vector2(1.57f, -1.88f), new Vector2(1f, -1.86f), 
				new Vector2(0.37f, -1.66f), new Vector2(0f, -2f), new Vector2(0f, -2.7f), new Vector2(0f, -3.2f),/*负号*/new Vector2(0f, -3.8f), new Vector2(-0.6f, -4f), new Vector2(-1.1f, -4f),
				new Vector2(-1.6f, -4f),new Vector2(-1.6f, -4f), new Vector2(-2f, -4f), new Vector2(-2.7f, -4f), new Vector2(-3.4f, -3.8f), new Vector2(-3.4f, -3.2f), new Vector2(-3.4f, -2.7f), 
				new Vector2 (-3.3f, -2f), new Vector2 (-3.7f, -1.7f), new Vector2 (-4.3f, -1.9f), new Vector2 (-4.8f, -1.9f), new Vector2 (-5.5f, -1.7f), new Vector2 (-5.6f, -1f),
				new Vector2 (-5.7f, -0.5f), new Vector2 (-5.7f, 0f), new Vector2 (-4.9f, 0f), new Vector2 (-4.3f, 0f), new Vector2 (-3.7f, 0f), new Vector2 (-3.2f, 0f),
				new Vector2 (-2.7f, 0f), new Vector2 (-2.2f, 0f), new Vector2 (-5.7f, 4.0f), new Vector2 (-4.7f, 4.0f), new Vector2 (-5.7f, 3.1f), new Vector2(-4.7f, 3.1f)}};
	}
}
