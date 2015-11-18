using UnityEngine;
using System.Collections;




/*
 *	게임 정보를 관리하는 클래스
 *	Maruchu
 */
public		class		Game				: MonoBehaviour {
	
	
	
	
	private	static		bool		m_stageClearFlag		= false;	//이것이 true라면 스테이지가 종료된 것이라고 정한다
	
	/*
	 *	스테이지가 종료되면 호출된다
	 */
	public	static		void		SetStageClear() {
		//스테이지가 종료됐음을 표시한다
		m_stageClearFlag		= true;
	}
	
	/*
	 *	스테이지가 종료됐는지 확인한다
	 */
	public	static		bool		IsStageCleared() {
		return	m_stageClearFlag;
	}
	
	
	
	
}
