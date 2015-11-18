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
	
	
	public AudioClip jumpVoice;  // 점프할 때의 음성
	public AudioClip damageVoice; // 피격을 당했을 때의 음성
	
	private AudioSource mAudio;  // AudioSource
	
	/*
    * 처음에 호출되는 함수
    */
	void Start () {
		mAudio = GetComponent<AudioSource> ();
	}
	
	/*
     * 피격을 당했을 때 호출되는 함수
     */
	void OnDamage(){
		// 음성을 재생한다
		PlayerVoice (damageVoice);
	}
	
	void Jump(){
		// 음성을 재생한다
		PlayerVoice (jumpVoice);
	}
	
	void PlayerVoice(AudioClip clip){
        // 소리를 없앤다
		mAudio.Stop ();
		// 소리를 재생한다
		mAudio.PlayOneShot (clip);
	}
}