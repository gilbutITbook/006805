using UnityEngine;
using System.Collections;




/*
 *	지정된 시간이 지나면 사라지는 오브젝트를 구현한 스크립트
 *	Maruchu
 */
public		class		Timer				: MonoBehaviour {




public							float		fTimeLimit				= 1f;						//각 프리팹의 생존 시간

private							float		m_fTimeLeft				= 0f;						//남은 생존 시간



/*
 *	시작 시에 호출되는 함수
 */
private		void	Awake() {
	//제한 시간을 지정한다
	m_fTimeLeft		= fTimeLimit;
}


/*
 *	매 프레임마다 호출되는 함수
 */
private		void	Update() {

	//지정된 시간동안 기다린다
	m_fTimeLeft		-= Time.deltaTime;
	if( m_fTimeLeft < 0f) {
		//시간 만료

		//이 GameObject를 Hierarchy에서 삭제한다
		Destroy( gameObject);
	}
}




}
