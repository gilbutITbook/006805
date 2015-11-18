using UnityEngine;
using System.Collections;
/*
* 점수를 관리하는 클래스
*/
public class Score : MonoBehaviour {
    // Score 클래스의 유일한 인스턴스
	private static Score mInstance;
	
    /*
     * Score 인스턴스를 반환하는 함수
     * (static에 public으로 지정되어 있으므로 모든 소스 코드에서 호출할 수 있다)
     */
	public static Score instance{
		get{
            // 인스턴스가 참조되었는지 검사
			if(mInstance == null){
                // 인스턴스를 찾고 그 인스턴스를 참조한다
				mInstance = FindObjectOfType<Score>();
			}
            // 인스턴스를 반환한다
			return mInstance;
		}
	}

    /*
     *  처음에 호출되는 함수
     */
    public void Start(){
        // 인스턴스가 이 클래스 자신이 아니면 삭제한다
		if (this != instance) {
			Destroy(this);
		}
	}

    // 점수
	public int score{
		get;
        // set은 이 소스 코드에서만 호출되도록 한다
		private set;
	}
    /*
    * 스코어에 1을 더하는 함수
    */
	public void Add(){
		score++;
	}
    /*
    * 스코어를 0으로 되돌리는 함수
    */
	public void Reset(){
		score = 0;
	}
}