using UnityEngine;
using System.Collections;


/*
 *	종료 지점 클래스
 *	Maruchu
 *
 *	Player클래스를 포함한 GameObject가 닿으면 스테이지가 종료된다
 */
public		class		Goal				: MonoBehaviour {




    /*
     * Collider가 무언가에 닿으면 호출되는 함수
     *
     * 자신의 GameObject에 Collider(IsTrigger를 ON으로 지정)와 Rigidbody를 적용한
       상태에서만 호출된다
     */

	private void OnTriggerEnter(Collider hitCollider) {

        // 상대의 GameObject 가져오기
		GameObject	hitObject = hitCollider.gameObject;

        // Player 클래스를 포함한 GameObject가 접촉하면 게임이 종료된다
		if( null==hitObject.GetComponent<Player>()) {
            // 플레이어가 아니었으므로 무시
			return;
		}

        // 스테이지를 종료한다
		Game.SetStageClear();
	}
	
	
	
	
}
