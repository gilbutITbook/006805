using UnityEngine;
using System.Collections;

public class Touch : MonoBehaviour {
	
	public AudioClip voice_01;
	public AudioClip voice_02;
	
	private Animator animator;
	
	private AudioSource univoice;
	
	private int motionIdol = Animator.StringToHash("Base Layer.Idol");

	// Use this for initialization
	void Start () {
	
		animator	= GetComponent<Animator> ();
		
		univoice	= GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		animator.SetBool("Touch", false);
		animator.SetBool("TouchHead", false);
		
		Ray ray;
		RaycastHit hit;
	  
		GameObject hitObject;

        // 재생 도중인 애니메이션이 대기 중인 애니메이션인지 검사
		if( animator.GetCurrentAnimatorStateInfo(0).nameHash == motionIdol){
			animator.SetBool("Motion_Idle", true);
		}
		else{
			animator.SetBool("Motion_Idle", false);
		}
		
		if( Input.GetMouseButtonDown(0) ){
			
			//마우스 커서 위치에서 카메라 화면 안쪽을 향해 레이를 쏜다
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(ray,out hit, 100)) {
				
				hitObject = hit.collider.gameObject;
				
				if(hitObject.gameObject.tag == "Head"){
					
					animator.SetBool("TouchHead", true);
					
					univoice.clip = voice_01;
					univoice.Play();
					
					animator.SetBool("Face_Happy", true);
					animator.SetBool("Face_Angry", false);
					
					DispMsg.dispMessage("안녕!\n오늘도 힘차게 시작해보자!");
				}
				else if(hitObject.gameObject.tag == "Breast"){
					
					animator.SetBool("Touch", true);
					
					univoice.clip = voice_02;
					univoice.Play();
					
					animator.SetBool("Face_Happy", false);
					animator.SetBool("Face_Angry", true);
					
					DispMsg.dispMessage("꺅!");
				}
				
			}
			
		}
	
	}
}
