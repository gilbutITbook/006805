using UnityEngine;
using System.Collections;
// AudioSource가 필요하다
// 이것을 써두면 이 컴포넌트를 추가할 때
// AudioSorce 컴포넌트도 함께 추가된다
[RequireComponent(typeof(AudioSource))]

/*
 * Coin 클래스
 * 
 * Player에 닿았을 때 소리를 낸다
 * 
 */      
public class Coin : MonoBehaviour {
	
	
	private AudioSource mAudio;
	private Renderer mRenderer;
	private Collider2D mCollider2D;
	
	/*
     * 처음에 호출되는 함수
     */
	void Start () {
		// 필요한 컴포넌트를 얻는다
		mAudio = GetComponent<AudioSource> ();
		mRenderer = GetComponent<Renderer> ();
		mCollider2D = GetComponent<Collider2D>();
	}
	
	void OnTriggerEnter2D(Collider2D other){
        // 만일 접촉한 오브젝트의 태그가 Player이면
		if (other.tag == "Player") {
            // 점수에 1을 더한다
			Score.instance.Add ();

            // 렌더링하지 않는다
			mRenderer.enabled = false;
            // 충돌 판정을 하지 않는다
			mCollider2D.enabled = false;

            // 소리를 재생한다
			mAudio.Play();
            // 소리 재생이 끝나면 오브젝트가 사라진다
			Destroy(gameObject,mAudio.clip.length);
		}
	}
	
}
