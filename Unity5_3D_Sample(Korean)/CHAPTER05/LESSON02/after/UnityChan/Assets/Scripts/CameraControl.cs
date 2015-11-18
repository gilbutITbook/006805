using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// 카메라 이동
		if( Input.GetMouseButton(0) ){
			
			Camera.main.transform.Translate(Input.GetAxisRaw("Mouse X") / 10, Input.GetAxisRaw("Mouse Y") / 10, 0);
		}
		
		// 카메라 회전
		if( Input.GetMouseButton(1) ){
			
			Camera.main.transform.Rotate(Input.GetAxisRaw("Mouse Y") * 10, Input.GetAxisRaw("Mouse X") * 10, 0);
		}
		
	}
	
}
