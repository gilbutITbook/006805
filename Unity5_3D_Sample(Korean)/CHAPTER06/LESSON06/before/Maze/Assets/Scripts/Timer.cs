using UnityEngine;
using System.Collections;




/*
 *	지정된 시간에 사라지는 오브젝트를 위한 스크립트
 *	Maruchu
 */
public		class		Timer				: MonoBehaviour {
	
	
	
	
	public							float		fTimeLimit				= 1f;						//각 프리팹 생존 시간
	
	private							float		m_fTimeLeft				= 0f;						//남은 생존 시간
	
	
	
	/*
	 *	시작할 때 호출되는 함수
	 */
	private		void	Awake() {
		//제한 시간을 지정
		m_fTimeLeft		= fTimeLimit;
	}
	
	
	/*
	 *	매 프레임 호출되는 함수
	 */
	private		void	Update() {
		
		//지정된 시간 동안 대기
		m_fTimeLeft		-= Time.deltaTime;
		if( m_fTimeLeft < 0f) {
			//시간 만료
			
			//이 GameObject를 Hierarchy에서 삭제
			Destroy( gameObject);
		}
	}
	
	
	
	
}
