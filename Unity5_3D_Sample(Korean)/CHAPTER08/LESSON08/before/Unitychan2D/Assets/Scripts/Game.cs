using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/*
*게임의 전환을 관리하는 클래스
*/
public class Game : MonoBehaviour {
    // 하나뿐인 Game 클래스의 인스턴스
	private static Game mInstance;
    /* Game 인스턴스를 반환하는 publc 함수
     * (static이고 public이므로 다른 모든 소스에서 호출할 수 있다
     */
    public static Game instance{
		
		get{
			// 인스턴스가 참조되고 있는지 검사
			if(mInstance == null){
				// 인스턴스를 찾아 참조한다
				mInstance = FindObjectOfType<Game>();
			}
			// 인스턴스를 반환한다
			return mInstance;
		}
	}
	// 게임의 현재 상태
	public enum STATE{
        NONE, // 아무것도 하지 않는 상태
		START, // 시작 상태
		MOVE, // 게임 중인 상태
		GAMEOVER // 게임 종료 상태
	};
	// 게임의 상태
	public STATE state{
		get;
		set;
	}
	
	private Text mText;
	
	/*
	 * 처음에 호출되는 함수
	 */
	void Start () {
		
		mText = GetComponent<Text> ();

        // 게임 상태를 시작 상태로 설정한다
		state = STATE.START;
		// StartCountDown을 호출한다
		StartCoroutine("StartCountDown");
	}

    /*
     * 프레임마다 호출되는 함수
     */
    void Update () {
		switch(state){
		case STATE.START:
			break;
		case STATE.MOVE:
			break;
		case STATE.GAMEOVER:
			// GUI에 Game Over라고 표시한다
			mText.text = "Game Over";
            // Jump 버튼이 눌렸는지 검사
			if(Input.GetButtonDown ("Jump")){
                // 현재 플레이 중인 씬
				int currentScene = Application.loadedLevel;
                // 현재 플레이 중인 씬을 다시 처음부터 불러온다
				Application.LoadLevel (currentScene);
			}
			break;
		}
	}
	
	IEnumerator StartCountDown(){
        // GUI에 3을 표시한다
		mText.text = "3";
		// 1초 대기
		yield return new WaitForSeconds(1.0f);
        // GUI에 2를 표시한다
		mText.text = "2";
		// 1초 대기
		yield return new WaitForSeconds(1.0f);
		// GUI에 1을 표시한다
		mText.text = "1";
		// 1초 대기
		yield return new WaitForSeconds(1.0f);
		// GUI에 아무것도 표시하지 않는다
		mText.text = "";
        // 게임 상태를 게임 중인 상태로 설정한다
		state = STATE.MOVE;
	}
}