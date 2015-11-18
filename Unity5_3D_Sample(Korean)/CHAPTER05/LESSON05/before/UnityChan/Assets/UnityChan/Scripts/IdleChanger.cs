using UnityEngine;
using System.Collections;

namespace UnityChan
{
//
// ↑↓ 키로 루프 애니메이션을 전환하는 스크립트(무작위 전환 기능 추가됨)Ver.3
// 2014/04/03 N.Kobayashi
//

// Require these components when using this script
	[RequireComponent(typeof(Animator))]



	public class IdleChanger : MonoBehaviour
	{
	
		private Animator anim;						// Animator로의 참조
		private AnimatorStateInfo currentState;		// 현재 스테이트 상태를 저장하는 참조
		private AnimatorStateInfo previousState;	// 바로 전 스테이트 상태를 저장하는 참조
		public bool _random = false;				// 무작위 판정 스타트 스위치
		public float _threshold = 0.5f;				// 무작위 판정 임계 값
		public float _interval = 10f;				// 무작위 판정 간격
		//private float _seed = 0.0f;					// 무작위 판정용 시드
	


		// Use this for initialization
		void Start ()
		{
			// 각 참조 초기화
			anim = GetComponent<Animator> ();
			currentState = anim.GetCurrentAnimatorStateInfo (0);
			previousState = currentState;
			// 무작위 판정용 함수 시작
			StartCoroutine ("RandomChange");
		}
	
		// Update is called once per frame
		void  Update ()
		{
			// ↑ 키/스페이스 키가 눌리면 스테이트를 다음으로 이행하는 처리
			if (Input.GetKeyDown ("up") || Input.GetButton ("Jump")) {
				// Boolean Next를 true로 지정한다
				anim.SetBool ("Next", true);
			}
		
			// ↓ 키가 눌리면 스테이트를 이전으로 되돌리는 처리
			if (Input.GetKeyDown ("down")) {
				// Boolean Back을 true로 지정한다
				anim.SetBool ("Back", true);
			}
		
			// "Next"플래그가 true일 때의 처리
			if (anim.GetBool ("Next")) {
				// 현재 스테이트를 검사하여 스테이트 이름이 틀렸으면 Boolean을 false로 되돌린다
				currentState = anim.GetCurrentAnimatorStateInfo (0);
				if (previousState.nameHash != currentState.nameHash) {
					anim.SetBool ("Next", false);
					previousState = currentState;				
				}
			}
		
			// "Back"플래그가 true일 때 처리
			if (anim.GetBool ("Back")) {
				// 현재 스테이트를 검사하여 스테이트 이름이 틀렸으면 Boolean을 false로 되돌린다
				currentState = anim.GetCurrentAnimatorStateInfo (0);
				if (previousState.nameHash != currentState.nameHash) {
					anim.SetBool ("Back", false);
					previousState = currentState;
				}
			}
		}

		void OnGUI ()
		{
			GUI.Box (new Rect (Screen.width - 110, 10, 100, 90), "Change Motion");
			if (GUI.Button (new Rect (Screen.width - 100, 40, 80, 20), "Next"))
				anim.SetBool ("Next", true);
			if (GUI.Button (new Rect (Screen.width - 100, 70, 80, 20), "Back"))
				anim.SetBool ("Back", true);
		}


		// 무작위 판정용 함수
		IEnumerator RandomChange ()
		{
			// 무한루프 시작
			while (true) {
				//무작위 판정 스위치가 ON일 때
				if (_random) {
					// 무작위 시드를 꺼내서 그 값에 따라 플래그를 설정한다
					float _seed = Random.Range (0.0f, 1.0f);
					if (_seed < _threshold) {
						anim.SetBool ("Back", true);
					} else if (_seed >= _threshold) {
						anim.SetBool ("Next", true);
					}
				}
				// 다음 판정까지 간격을 둔다
				yield return new WaitForSeconds (_interval);
			}

		}

	}
}
