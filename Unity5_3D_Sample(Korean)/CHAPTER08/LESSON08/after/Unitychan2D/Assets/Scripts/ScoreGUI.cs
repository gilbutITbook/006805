using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreGUI : MonoBehaviour {
	
	private Text mText;
	
	// Use this for initialization
	void Start () {
		mText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
        // Score 클래스에서 score를 얻는다
		int score = Score.instance.score;
        // 세 자리수가 되도록 0을 추가한다
		string scoreAddZero = score.ToString ("000");
        // 텍스트를 GUI로 표시한다
		mText.text = "Score:" + scoreAddZero;
	}
}