//
// Mecanim의 애니메이션 데이터가 원점에서 이동하지 않을 경우에 적용 할 Rigidbody가 첨가 된 컨트롤러
// 예제
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;

namespace UnityChan
{
    // 필요한 컴포넌트 열거
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Rigidbody))]

	public class UnityChanControlScriptWithRgidBody : MonoBehaviour
	{

        public float animSpeed = 1.5f;				// 애니메이션 재생 속도 설정
		public float lookSmoother = 3.0f;			// a smoothing setting for camera motion
        public bool useCurves = true;				// 메카님 커브 조정을 사용하거나 설정한다
        // 이 스위치가 ON 상태가 아니면 곡선은 사용되지 않는다
        public float useCurvesHeight = 0.5f;		// 커브 보정 유효 높이(지상을 빠져나오기 쉬울 때에는 확대한다)

        // 다음 캐릭터 컨트롤러를 위한 파라미터
        // 전진 속도
		public float forwardSpeed = 7.0f;
        // 후퇴 속도
		public float backwardSpeed = 2.0f;
        // 회전 속도
		public float rotateSpeed = 2.0f;
        // 점프 위력
		public float jumpPower = 3.0f; 
        // 캐릭터 컨트롤러(캡슐 콜라이더) 참조
		private CapsuleCollider col;
		private Rigidbody rb;
        // 캐릭터 컨트롤러(캡슐 콜라이더) 이동량
		private Vector3 velocity;
        // CapsuleCollider로 설정되어 있는 콜라이더의 Height, Center 초깃값을 담을 변수
		private float orgColHight;
		private Vector3 orgVectColCenter;
        private Animator anim;							// 캐릭터에 적용되는 애니메이터로의 참조
        private AnimatorStateInfo currentBaseState;			// base layer에서 사용되는 애니메이터의 현재 상태 참조

        private GameObject cameraObject;	// 메인 카메라로의 참조
		
        // 애니메이터 각 스테이트로의 참조
		static int idleState = Animator.StringToHash ("Base Layer.Idle");
		static int locoState = Animator.StringToHash ("Base Layer.Locomotion");
		static int jumpState = Animator.StringToHash ("Base Layer.Jump");
		static int restState = Animator.StringToHash ("Base Layer.Rest");

        // 초기화
		void Start ()
		{
            // Animator 컴포넌트를 얻는다
			anim = GetComponent<Animator> ();
            // CapsuleCollider 컴포넌트 얻기(캡슐형 콜리젼)
			col = GetComponent<CapsuleCollider> ();
			rb = GetComponent<Rigidbody> ();
            //메인 카메라를 얻는다
			cameraObject = GameObject.FindWithTag ("MainCamera");
            // CapsuleCollider 컴포넌트의 Height, Center의 초깃값을 저장한다
			orgColHight = col.height;
			orgVectColCenter = col.center;
		}
	
	
		// 주된 처리이다. 리지드 바디와 엮이기 때문에 FixedUpdate에서 처리한다.
		void FixedUpdate ()
		{
			float h = Input.GetAxis ("Horizontal");				// 입력 장치의 수평축을 h로 정의
			float v = Input.GetAxis ("Vertical");				// 입력 장치의 수직축을 v로 정의
			anim.SetFloat ("Speed", v);							// Animator 쪽에서 설정하는 'Speed' 파라미터에 v를 전달한다
			anim.SetFloat ("Direction", h); 						// Animator 쪽에서 설정하는 'Direction' 파라미터에 h를 전달한다
			anim.speed = animSpeed;								// Animator 모션 재생 속도에 animSpeed을 설정한다
			currentBaseState = anim.GetCurrentAnimatorStateInfo (0);	// 참조를 위한 스테이트 변수에 Base Layer(0)의 현재 스테이트를 설정한다
			rb.useGravity = true;//점프 도중에 중력을 거스르므로 그 이외의 경우에는 중력의 영향을 받도록 한다
		
		
		
			// 이하는 캐릭터의 이동 처리이다
			velocity = new Vector3 (0, 0, v);		// 위아래 방향 키 입력에서 Z축 방향 이동량을 얻는다
			// 캐릭터의 로컬 공간 방향으로 변환
			velocity = transform.TransformDirection (velocity);
            //다음 v의 임계 값은 Mecanim 쪽 트랜지션과 함께 조정한다
			if (v > 0.1) {
                velocity *= forwardSpeed;		// 이동 속도를 곱한다
			} else if (v < -0.1) {
                velocity *= backwardSpeed;	// 이동 속도를 곱한다
			}
		
			if (Input.GetButtonDown ("Jump")) {	// 스페이스 키를 입력하면

                //애니메이션 스테이트가 Locomotion 도중에만 점프할 수 있다
				if (currentBaseState.nameHash == locoState) {
                    //스테이트 이행 중이 아니면 점프할 수 있다
					if (!anim.IsInTransition (0)) {
						rb.AddForce (Vector3.up * jumpPower, ForceMode.VelocityChange);
						anim.SetBool ("Jump", true);		// 점프로 전환하는 플래그를 Animator로 보낸다
					}
				}
			}
		

			// 위아래 방향 키 입력으로 캐릭터를 이동시킨다
			transform.localPosition += velocity * Time.fixedDeltaTime;

			// 좌우 방향 키 입력으로 캐릭터를 Y축 중심으로 회전시킨다
			transform.Rotate (0, h * rotateSpeed, 0);	
	

			// 이하는 Animator의 각 스테이트 안에서 실시하는 처리
			// Locomotion 중
			// 현재의 베이스 레이어가 locoState일 때
			if (currentBaseState.nameHash == locoState) {
				//곡선으로 콜라이더를 조정하고 있을 때는 만일을 위해 초기화한다
				if (useCurves) {
					resetCollider ();
				}
			}
        // JUMP 도중에서의 처리
        // 현재 베이스 레이어가 jumpState일 때
		else if (currentBaseState.nameHash == jumpState) {
				cameraObject.SendMessage ("setCameraPositionJumpView");	// 점프 도중에 사용하는 카메라로 변경한다
                // 스테이트가 트랜지션 도중이 않을 경우
				if (!anim.IsInTransition (0)) {

                    // 이하는 커브를 조정할 경우의 처리이다
					if (useCurves) {
                        // JUMP00 애니메이션에 적용되어 있는 곡선 JumpHeight과 GravityControl
                        // JumpHeight: JUMP00에서 점프 높이(0-1)
                        // GravityControl: 1⇒ 점프 중(중력 무효화) 0⇒ 중력 활성화
						float jumpHeight = anim.GetFloat ("JumpHeight");
						float gravityControl = anim.GetFloat ("GravityControl"); 
						if (gravityControl > 0)
							rb.useGravity = false;	//점프 도중에는 중력을 무시한다
										
						// 레이 캐스트를 캐릭터의 중심보다 아래쪽으로 설정한다
						Ray ray = new Ray (transform.position + Vector3.up, -Vector3.up);
						RaycastHit hitInfo = new RaycastHit ();
                        // 높이가 useCurvesHeight 이상일 때에만 콜라이더의 높이와 중심을 JUMP00 애니메이션에 적용된 곡선으로 조정
						if (Physics.Raycast (ray, out hitInfo)) {
							if (hitInfo.distance > useCurvesHeight) {
								col.height = orgColHight - jumpHeight;			// 조정된 콜라이더의 높이
								float adjCenterY = orgVectColCenter.y + jumpHeight;
								col.center = new Vector3 (0, adjCenterY, 0);	// 조정된 콜라이더의 중심
							} else {
								// 임계 값보다 낮을 경우에는 초깃값으로 되돌린다(만일을 위해)					
								resetCollider ();
							}
						}
					}
					// Jump bool 값을 초기화한다(루프하지 않도록 한다)				
					anim.SetBool ("Jump", false);
				}
			}
		// IDLE일 때의 처리
		// 현재의 베이스 레이어가 idleState일 때
		else if (currentBaseState.nameHash == idleState) {
				//곡선으로 콜라이더를 조정할 때에는 만일을 위해 초기화한다
				if (useCurves) {
					resetCollider ();
				}
				// 스페이스 키를 입력하면 Rest 상태가 된다
				if (Input.GetButtonDown ("Jump")) {
					anim.SetBool ("Rest", true);
				}
			}
		// REST 도중에 수행할 처리
		// 현재의 베이스 레이어가 restState일 때
		else if (currentBaseState.nameHash == restState) {
				//cameraObject.SendMessage("setCameraPositionFrontView");		// 카메라를 정면을 향하도록 변경한다
				// 스테이트가 이행 중이 아닐 경우 Rest bool 값을 초기화한다(루프하지 않도록 한다)
				if (!anim.IsInTransition (0)) {
					anim.SetBool ("Rest", false);
				}
			}
		}

		void OnGUI ()
		{
			GUI.Box (new Rect (Screen.width - 260, 10, 250, 150), "Interaction");
			GUI.Label (new Rect (Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
			GUI.Label (new Rect (Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Turn Left/Turn Right");
			GUI.Label (new Rect (Screen.width - 245, 70, 250, 30), "Hit Space key while Running : Jump");
			GUI.Label (new Rect (Screen.width - 245, 90, 250, 30), "Hit Spase key while Stopping : Rest");
			GUI.Label (new Rect (Screen.width - 245, 110, 250, 30), "Left Control : Front Camera");
			GUI.Label (new Rect (Screen.width - 245, 130, 250, 30), "Alt : LookAt Camera");
		}


		// 캐릭터의 콜라이더 크기를 초기화하는 함수
		void resetCollider ()
		{
			// 컴포넌트의 Height, Center의 초깃값을 되돌린다
			col.height = orgColHight;
			col.center = orgVectColCenter;
		}
	}
}