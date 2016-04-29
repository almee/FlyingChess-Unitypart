using UnityEngine;
using System.Collections;
using Com.BehaviourManamger;
using Com.ModelManamger;
using Com.Controller;
using Com.Usertype.Manual;


public class testGeneral : MonoBehaviour {
	public ManualInput test_ba = ManualInput.getInstance();
	// Use this for initialization
	void Start () {
		
	}
	
	
	// Update is called once per frame
	void Update () {
			if (Input.GetMouseButtonDown (0)) {
				CheckTouch (Input.mousePosition, "began");
			}
			
			if (Input.GetMouseButtonUp (0)) {
				CheckTouch (Input.mousePosition, "ended");
			}
		
	}
	
	void CheckTouch(Vector3 pos, string phase) {
		/* Get the screen point where the user is touching */
		Vector3 wp =  Camera.main.ScreenToWorldPoint(pos);
		Vector2 touchPos = new Vector2(wp.x, wp.y);
		Collider2D hit = Physics2D.OverlapPoint(touchPos);
		if (hit.gameObject.name == this.gameObject.name && hit && phase == "began") {
			print ( this.gameObject.name);
			test_ba.inputPlane(Game_object.Name_Get_Number[this.gameObject.name].userNum, Game_object.Name_Get_Number[this.gameObject.name].planeNum);
		}
	}
}
