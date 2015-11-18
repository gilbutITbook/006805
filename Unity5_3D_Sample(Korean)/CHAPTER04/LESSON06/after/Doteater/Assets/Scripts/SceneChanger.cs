using UnityEngine;
using System.Collections;

public class SceneChanger : MonoBehaviour {

	public string nextSceneName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Submit")) {
			Application.LoadLevel(nextSceneName);
		}
	}
}
