using UnityEngine;
using System.Collections;

// AudioSource가 필요하다
// 이것을 써두면 이 컴포넌트를 추가할 때
// AudioSorce 컴포넌트도 함께 추가된다
[RequireComponent(typeof(AudioSource))]

/*
 * Player 클래스
 * 
 * UnityChan2DController 이외의 기능들을 제어한다
 * 
 */
public class PlayerController : MonoBehaviour {
	
	
	public AudioClip jumpVoice;		// 점프할 때의 음성
	public AudioClip damageVoice;	// 피격을 당했을 때의 음성
	
	private AudioSource mAudio;		//AudioSource
	private UnityChan2DController mUnityChan2DController; //UnityChan2DController
	private Collider2D mCollider2D;
	
	/*
	 * 처음에 호출되는 함수
	 */
	void Start () {
        // 필요한 컴포넌트를 얻는다
		mAudio = GetComponent<AudioSource> ();
		mUnityChan2DController = GetComponent<UnityChan2DController>();
		mCollider2D = GetComponent<Collider2D> ();
	}
	
	/*
	 * 피격을 당했을 때 호출되는 함수
	 */
	void OnDamage(){
		// 음성을 재생한다
		PlayerVoice (damageVoice);
		
		// 음성을 재생한다
		mCollider2D.enabled = false;
        // 움직이지 않게 한다
		mUnityChan2DController.enabled = false;
        // 게임 전환을 게임 종료로 설정한다
		Game.instance.state = Game.STATE.GAMEOVER;
	}
	
	void Jump(){
        // 음성을 재생한다
		PlayerVoice (jumpVoice);
	}
	
	void PlayerVoice(AudioClip clip){
		// 소리를 제거한다
		mAudio.Stop ();
		// 소리를 재생한다
		mAudio.PlayOneShot (clip);
	}
}
