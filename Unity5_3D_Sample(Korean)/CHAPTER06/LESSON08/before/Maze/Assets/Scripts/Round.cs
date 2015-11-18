using UnityEngine;
using System.Collections;




/*
 *	회전하는 오브젝트를 위한 스크립트
 *	Maruchu
 */
public		class		Round				: MonoBehaviour {



	private							float		rotationNow				= 0f;						//회전량 로그
	
	public							float		rotationAdd				= 90f;						//1초 동안 회전하는 양
	
	
	
	/*
	 *	매 프레임마다 호출되는 함수
	 */
	private		void	Update() {
		
		// 회전량을 더한다
		rotationNow			+= (rotationAdd	*Time.deltaTime);
		/*
         * 이동량, 회전량은 Time.deltaTime을 곱하여 실행 환경(프레임률)에 의한 
           차이가 생기지 않도록 한다
         */

		// 오일러 각으로 넣는다
		transform.rotation	= Quaternion.Euler( 0, rotationNow, 0);		
		// Y축 회전으로 오브젝트의 방향을 돌린다
	}
	
	
	
	
}
