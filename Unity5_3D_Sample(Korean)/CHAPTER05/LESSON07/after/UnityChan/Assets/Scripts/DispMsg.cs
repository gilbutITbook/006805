using UnityEngine;
using System.Collections;

public class DispMsg : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public static int		lengthMsg;			//표시할 문자열 길이
	public static bool		flgDisp = false;	//표시 플래그
	public static float		waitTime = 0;
	
	float nextTime = 0;
	
	// Update is called once per frame
	void Update () {
		
		//메시지가 표시된 상태에서만 처리가 수행된다
		if(flgDisp == true){
			
			if(Time.time > nextTime) {			     //다음 문자 표시 시간을 넘었을 때
			
				if(lengthMsg < dispMsg.Length){	     //문자열 끝까지 오지 않았을 때
				
					lengthMsg ++;					 //다음 문자
				}			
			
				nextTime = Time.time + 0.01f;	     //다음 문자 표시까지의 시간 간격
			}
			
			if(lengthMsg >= dispMsg.Length){		//메시지를 전부 표시했다면
			
				waitTime += Time.deltaTime;
					
				if(waitTime > dispMsg.Length / 4) {	//잠시 기다린다.　※메시지 길이에 따라 다르게 적용한다
				
					flgDisp = false;		        //표시하지 않는다
				}
			}
		}
	
	}
	
	public static string	dispMsg;
	
	public static void dispMessage (string msg) {
		
		dispMsg = msg;		//메시지를 대입한다
		lengthMsg = 0;		//0번째 문자로 초기화한다
		flgDisp = true;		//표시
		waitTime = 0;
	}
	
	public GUIStyle msgWnd;
	
	void OnGUI () {
	
		//기준 화면폭
		const float screenWidth = 1136;
		
		//기준 크기에 대한 창 크기와 좌표
		const float msgwWidth	= 800;
		const float msgwHeight	= 200;
		const float msgwPosX	= (screenWidth - msgwWidth) / 2;
		const float msgwPosY	= 390;
		
		//화면 폭으로부터 1픽셀을 계산한다
		float factorSize = Screen.width / screenWidth;
		
		float msgwX;
		float msgwY;
		float msgwW = msgwWidth * factorSize;
		float msgwH = msgwHeight * factorSize;

        //폰트 스타일
		GUIStyle myStyle = new GUIStyle();
		myStyle.fontSize = (int)(30 * factorSize);
		
		//메시지 표시
		if(flgDisp == true){
			
			//창
			msgwX = msgwPosX * factorSize;
			msgwY = msgwPosY * factorSize;
			GUI.Box(new Rect(msgwX,msgwY,msgwW,msgwH),"창",msgWnd);
			
			//메시지 그림자
			myStyle.normal.textColor = Color.black;
			
			msgwX = (msgwPosX + 22) * factorSize;
			msgwY = (msgwPosY + 22) * factorSize;
			GUI.Label(new Rect(msgwX,msgwY,msgwW,msgwH),dispMsg.Substring(0, lengthMsg),myStyle);
			
			//메시지
			myStyle.normal.textColor = Color.white;
			
			msgwX = (msgwPosX + 20) * factorSize;
			msgwY = (msgwPosY + 20) * factorSize;
			GUI.Label(new Rect(msgwX,msgwY,msgwW,msgwH),dispMsg.Substring(0, lengthMsg),myStyle);
		}
		
	}
}
