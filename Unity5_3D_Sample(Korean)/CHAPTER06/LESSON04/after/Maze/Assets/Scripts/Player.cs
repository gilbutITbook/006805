using UnityEngine;
using System.Collections;




/*
 *	플레이어 클래스
 *	Maruchu
 *
 *	캐릭터 이동, 메카님(모션) 제어
 */
public		class		Player				: MonoBehaviour {
	
	
	
	public							GameObject	playerObject			= null;		//움직일 대상 모델
	public							GameObject	bulletObject			= null;		//총알 프리팹
	
	public							Transform	bulletStartPosition		= null;		//총알 발사 위치를 얻는 본
	
	
	
	private		static readonly		float		MOVE_Z_FRONT			=  5.0f;	//전진 속도
	private		static readonly		float		MOVE_Z_BACK				= -2.0f;	//후퇴 속도
	
	private		static readonly		float		ROTATION_Y_KEY			= 360.0f;	//회전 속도(키보드)
	private		static readonly		float		ROTATION_Y_MOUSE		= 720.0f;	//회전 속도(마우스)
	
	private							float		m_rotationY				= 0.0f;		//플레이어의 회전 각도
	
	private							bool		m_mouseLockFlag			= true;		//마우스를 고정하는 기능
	
	
	
	
	/*
	 *	매 프레임마다 호출되는 함수
	 */
	private		void	Update() {
		
		//스테이지가 종료되면 조작을 무시한다
		if( Game.IsStageCleared()) {
			return;
		}
		
		//마우스 잠금 처리
		CheckMouseLock();
		
		//이동 처리
		CheckMove();
	}
	
	
	/*
	 *	마우스 잠금 처리 검사
	 */
	private		void	CheckMouseLock() {
		
		//Esc 키를 눌렀을 때의 동작
		if( Input.GetKeyDown( KeyCode.Escape)) {
			//플래그를 반전시킨다
			m_mouseLockFlag	= !m_mouseLockFlag;
		}
		
		//마우스가 잠겨있는지 검사
		if( m_mouseLockFlag) {
			//잠겨 있으면 잠금 해제
			Screen.lockCursor	= true;
			Cursor.visible	= false;
		} else {
			//잠금이 해제되어 있다면 잠금
			Screen.lockCursor	= false;
			Cursor.visible	= true;
		}
	}
	/*
 *	이동 처리 검사
 */
	private		void	CheckMove() {
		
		//회전
		{
			//이 프레임에서 움직이는 회전량
			float	addRotationY	= 0.0f;
						
			//키 조작으로 회전
			if( Input.GetKey( KeyCode.Q)) {
				addRotationY		= -ROTATION_Y_KEY;
			} else
			if( Input.GetKey( KeyCode.E)) {
				addRotationY		=  ROTATION_Y_KEY;
			}

            //마우스 이동량에 의한 회전
			if( m_mouseLockFlag) {
				//이동량을 얻어서 각도 처리로 넘겨준다
				addRotationY		+= (Input.GetAxis( "Mouse X")	*ROTATION_Y_MOUSE);
			}
			
			//현재 각도에 더한다
			m_rotationY			+= (addRotationY	*Time.deltaTime);
            /* 
             * 이동량, 회전량에는 Time.deltaTime을 곱해서 실행 환경(기기 성능에 의해 발생되는 프레임 수의 차이)에 따라 차이가 없게 한다
             */


            //오일러 각으로 입력한다
            transform.rotation = Quaternion.Euler(0, m_rotationY, 0);		//Y축 회전으로 캐릭터 방향을 옆으로 바꾼다
		}
		
		//이동
		Vector3	addPosition	= Vector3.zero;		//이동량(z 값은 메카님에도 넘겨준다)
		{
            /*
                Vector3.zero는 new Vector3(0f, 0f, 0f)와 같습니다
                그 밖의 사항은 아래에 표시한 웹 페이지에서 참조하기 바람
                http://docs.unity3d.com/kr/current/ScriptReference/Vector3.html
             */

            //키 조작에서 이동할 양을 얻는다
            Vector3 vecInput = new Vector3(0f, 0, Input.GetAxisRaw("Vertical"));		// 앞뒤 방향 입력 값을 받아 Z에 넣는다([W], [S] 게임 패드 입력 등)

            //Z에 값이 입력되어 있는지 확인한다
			if( vecInput.z > 0) {
				//전진
				addPosition.z		= MOVE_Z_FRONT;
			} else
			if( vecInput.z < 0) {
				//후퇴
				addPosition.z		= MOVE_Z_BACK;
			}
			
			//이동량을 Transform에 넘겨주어 이동시킨다
			transform.position	+= ((transform.rotation	 	*addPosition)		*Time.deltaTime);
            /*
                Vector3 형식으로 transform.rotation을 곱하면 그 방향으로 꺾어진다
                이 때 Vector3는 Z+ 방향을 정면으로 여긴다
             */
        }
		
		//사격
		bool	shootFlag;
		{
			//사격 버튼(클릭)을 눌렀는지 확인한다
			if( Input.GetButtonDown( "Fire1")) {
				//사격 처리
				shootFlag	= true;
				
				//총알을 발사할 위치가 지정되어 있는지 여부를 검사
				if( null!=bulletStartPosition) {
					//총알을 생성할 위치 지정
					Vector3 vecBulletPos	= bulletStartPosition.position;
					//전진하는 방향으로 조금 진행한다
					vecBulletPos += (transform.rotation	*Vector3.forward);
					//Y 높이를 적당히 올린다
					vecBulletPos.y = 1.0f;
					
					//총알을 생성한다
					Instantiate( bulletObject, vecBulletPos, transform.rotation);
				}
			} else {
				//발사하지 않는다
				shootFlag	= false;
			}
		}
		
		
		// 메카님(모션)
		{
			// 애니메이터를 얻는다
			Animator	animator	= playerObject.GetComponent<Animator>();
			
			// 설정한 값을 Animator로 넘겨준다
			animator.SetFloat(	"SpeedZ",	addPosition.z);	// Z(앞뒤 방향 이동량)
			animator.SetBool(	"Shoot",	shootFlag);		// 사격 플래그
		}
	}
	
	
	
	
}
