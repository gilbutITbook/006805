using UnityEngine;
using System.Collections;

public class Janken : MonoBehaviour {

	bool flgJanken = false;		//가위바위보 모드 플래그
	
	int modeJanken = 0;		//가위바위보 모드 상태

	public AudioClip voice_janken_start;
	public AudioClip voice_janken_pon;
	public AudioClip voice_janken_goo;
	public AudioClip voice_janken_choki;
	public AudioClip voice_janken_par;
	public AudioClip voice_janken_win;
	public AudioClip voice_janken_loose;
	public AudioClip voice_janken_draw;

	const int JANKEN= 0;
	const int GOO	= 1;
	const int CHOKI	= 2;
	const int PAR	= 3;
	const int DRAW	= 4;
	const int WIN	= 5;
	const int LOOSE	= 6;
	
	private Animator animator;
	
	private AudioSource univoice;
	
	int myHand;
	int unityHand;
	int flgResult;
	
	float waitTime;
	
	// Use this for initialization
	void Start () {
		
		animator = GetComponent<Animator>();
		
		univoice	= GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(flgJanken == true){
			
			switch(modeJanken){
			
			case 0:	//가위바위보 시작
				
				UnityChanAction(JANKEN);
				modeJanken ++;
				break;
			
			case 1:	//플레이어의 입력을 기다린다
				
				//애니메이션 초기화
				animator.SetBool("Janken", false);
				animator.SetBool("Aiko", false);
				animator.SetBool("Goo", false);
				animator.SetBool("Choki", false);
				animator.SetBool("Par", false);
				animator.SetBool("Win", false);
				animator.SetBool("Loose", false);
				
				break;
			
			case 2:	//판정
				
				flgResult = -1;

                //유니티짱이 무엇을 낼지 무작위로 정한다
				unityHand = Random.Range(GOO,PAR+1);

                //유니티짱의 동작
				UnityChanAction(unityHand);
				
				//판정
				if(myHand == unityHand){
					flgResult = DRAW;
				}
				else{
					switch(unityHand){
						case GOO:
							if(myHand == PAR){
								flgResult = LOOSE;
							}
						break;
						
						case CHOKI:							
							if(myHand == GOO){
								flgResult = LOOSE;
							}
						break;
						
						case PAR:							
							if(myHand == CHOKI){
								flgResult = LOOSE;
							}
						break;
					}
					
					if(flgResult != LOOSE){
						flgResult = WIN;
					}
				}
				
				modeJanken ++;
				break;
			
			case 3:	//결과
				
				//약간의 시간 간격을 둔다
				waitTime += Time.deltaTime;
				
				if(waitTime > 1.5){
					
					//유니티짱의 동작
					UnityChanAction(flgResult);
					
					waitTime = 0;
					modeJanken ++;
				}
				break;
				
			case 4:	//가위바위보 끝내기
				
				flgJanken = false;
				modeJanken = 0;
				
				break;
			}
		}
	
	}
	
	//유니티짱의 동작
	void UnityChanAction(int action){
		
		switch(action){
			
			case JANKEN:
				animator.SetBool("Janken", true);
				univoice.clip = voice_janken_start;
			break;
			
			case GOO:
				animator.SetBool("Goo", true);
				univoice.clip = voice_janken_goo;
			break;
			
			case CHOKI:
				animator.SetBool("Choki", true);
				univoice.clip = voice_janken_choki;
			break;
			
			case PAR:
				animator.SetBool("Par", true);
				univoice.clip = voice_janken_par;
			break;
			
			case DRAW:
				animator.SetBool("Aiko", true);
				univoice.clip = voice_janken_draw;
			break;
				
			case WIN:
				animator.SetBool("Win", true);
				univoice.clip = voice_janken_win;
			break;
			
			case LOOSE:
				animator.SetBool("Loose", true);
				univoice.clip = voice_janken_loose;
			break;
		}
		univoice.Play();
	}
	
	public GUIStyle btStyleMode;
	public GUIStyle btStyleGoo;
	public GUIStyle btStyleChoki;
	public GUIStyle btStylePar;
	
	void OnGUI ()
	{
		//기준 화면 폭
		const float screenWidth = 1136;
		
		//기준 크기에 대한 창 크기와 좌표
		const float buttonSize	= 200;
		const float button0PosX	= 10;
		const float button1PosX	= (screenWidth - buttonSize) / 2 - 220;
		const float button2PosX	= (screenWidth - buttonSize) / 2;
		const float button3PosX	= (screenWidth - buttonSize) / 2 + 220;
		const float buttonPosY	= 400;
		
		//현재 지정되어 있는 화면 폭과 실제 화면 폭과의 비율을 계산
		float factorSize = Screen.width / screenWidth;
		
		float btnSize	= buttonSize	* factorSize;
		float btn0PosX	= button0PosX	* factorSize;
		float btn1PosX	= button1PosX	* factorSize;
		float btn2PosX	= button2PosX	* factorSize;
		float btn3PosX	= button3PosX	* factorSize;
		float btnPosY	= buttonPosY	* factorSize;
		
		if(flgJanken == false){
			
			if(GUI.Button (new Rect (btn0PosX,btnPosY,btnSize,btnSize), "가위바위보",btStyleMode)){
				flgJanken = true;
			}
		}
		
		if(modeJanken == 1){
			if(GUI.Button (new Rect (btn1PosX,btnPosY,btnSize,btnSize), "바위",btStyleGoo)){	
				myHand = GOO;
				modeJanken ++;
			}
			
			if(GUI.Button (new Rect (btn2PosX,btnPosY,btnSize,btnSize), "가위",btStyleChoki) ){
				myHand = CHOKI;
				modeJanken ++;
			}
			
			if(GUI.Button (new Rect (btn3PosX,btnPosY,btnSize,btnSize), "보",btStylePar) ){
				myHand = PAR;
				modeJanken ++;
			}
		}
	}
}
