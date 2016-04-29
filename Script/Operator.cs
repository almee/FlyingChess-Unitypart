using UnityEngine;
using System.Collections;
using Com.Controller;
public class Operator : MonoBehaviour {
	private Controller controller;
	// Use this for initialization
	void Start () {
		controller = Controller.GetInstance ();
	}
	
	// Update is called once per frame
	void Update () {
		controller.run ();
	}
}
