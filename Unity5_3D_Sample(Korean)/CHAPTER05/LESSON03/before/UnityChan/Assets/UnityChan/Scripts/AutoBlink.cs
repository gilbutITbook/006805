//
//AutoBlink.cs
//オート目パチスクリプト
//2014/06/23 N.Kobayashi
//
using UnityEngine;
using System.Collections;
using System.Security.Policy;

namespace UnityChan
{
	public class AutoBlink : MonoBehaviour
	{

		public bool isActive = true;				//자동 눈 깜박임 활성화
		public SkinnedMeshRenderer ref_SMR_EYE_DEF;	//EYE_DEF로 참조
		public SkinnedMeshRenderer ref_SMR_EL_DEF;	//EL_DEF로 참조
		public float ratio_Close = 85.0f;			//눈 감음 블렌드 셰이프 비율
		public float ratio_HalfClose = 20.0f;		//눈을 반만 감는 것에 대한 블렌드 셰이프 비율
		[HideInInspector]
		public float
			ratio_Open = 0.0f;
		private bool timerStarted = false;			//타이머 스타트 관리용
		private bool isBlink = false;				//눈 깜박임 관리용

		public float timeBlink = 0.4f;				//눈 깜박임 시간
		private float timeRemining = 0.0f;			//타이머 남은 시간

		public float threshold = 0.3f;				// 무작위 판정 임계 값
		public float interval = 3.0f;				// 무작위 판정 간격



		enum Status
		{
			Close,
			HalfClose,
			Open	//눈 깜박임 상태
		}


		private Status eyeStatus;	//현재 눈 깜박임 상태

		void Awake ()
		{
			//ref_SMR_EYE_DEF = GameObject.Find("EYE_DEF").GetComponent<SkinnedMeshRenderer>();
			//ref_SMR_EL_DEF = GameObject.Find("EL_DEF").GetComponent<SkinnedMeshRenderer>();
		}



		// Use this for initialization
		void Start ()
		{
			ResetTimer ();
			// 무작위 판정용 함수 시작
			StartCoroutine ("RandomChange");
		}

		//타이머 초기화
		void ResetTimer ()
		{
			timeRemining = timeBlink;
			timerStarted = false;
		}

		// Update is called once per frame
		void Update ()
		{
			if (!timerStarted) {
				eyeStatus = Status.Close;
				timerStarted = true;
			}
			if (timerStarted) {
				timeRemining -= Time.deltaTime;
				if (timeRemining <= 0.0f) {
					eyeStatus = Status.Open;
					ResetTimer ();
				} else if (timeRemining <= timeBlink * 0.3f) {
					eyeStatus = Status.HalfClose;
				}
			}
		}

		void LateUpdate ()
		{
			if (isActive) {
				if (isBlink) {
					switch (eyeStatus) {
					case Status.Close:
						SetCloseEyes ();
						break;
					case Status.HalfClose:
						SetHalfCloseEyes ();
						break;
					case Status.Open:
						SetOpenEyes ();
						isBlink = false;
						break;
					}
					//Debug.Log(eyeStatus);
				}
			}
		}

		void SetCloseEyes ()
		{
			ref_SMR_EYE_DEF.SetBlendShapeWeight (6, ratio_Close);
			ref_SMR_EL_DEF.SetBlendShapeWeight (6, ratio_Close);
		}

		void SetHalfCloseEyes ()
		{
			ref_SMR_EYE_DEF.SetBlendShapeWeight (6, ratio_HalfClose);
			ref_SMR_EL_DEF.SetBlendShapeWeight (6, ratio_HalfClose);
		}

		void SetOpenEyes ()
		{
			ref_SMR_EYE_DEF.SetBlendShapeWeight (6, ratio_Open);
			ref_SMR_EL_DEF.SetBlendShapeWeight (6, ratio_Open);
		}
		
		// 무작위 판정용 함수
		IEnumerator RandomChange ()
		{
			// 무한루프 시작
			while (true) {
				//무작위 판정용 시드 발생
				float _seed = Random.Range (0.0f, 1.0f);
				if (!isBlink) {
					if (_seed > threshold) {
						isBlink = true;
					}
				}
				// 다음 판정까지 간격을 둔다
				yield return new WaitForSeconds (interval);
			}
		}
	}
}