using UnityEngine;
using System.Collections;

/*
 * 등록해둔 프리팹을 일정한 간격으로 표시하는 클래스
 * 
 * 이 스크립트가 적용된 오브젝트의 위치에 추가한다.
 */
public class Creator : MonoBehaviour {

    // 표시할 프리팹
	public GameObject prefab;
    // 생성 위치는 무작위로 생성된 수만큼의 거리에 있다
	public Vector3 randomPosRange = Vector3.up;
    // 생성되었을 때의 속도
	public Vector3 velocity = Vector3.left;

    // 처음 생성할 때까지 걸리는 시간
	public float offsetTime = 0f;
    // 생성하는 타이밍
	public float intervalTime = 3f;
    // 생성된 오브젝트가 사라질 때까지 걸리는 시간
	public float leftTime = 5f;

    // 이 클래스가 관리하는 시간
	private float mTime = 0f;

    /*
     * 처음에 호출되는 함수
     */
    void Start () {
        // offsetTime을 뺀 값으로 시간을 지정한다
		mTime = -offsetTime;
	}

    /*
     * 프레임마다 호출되는 함수
     */
    void Update () {
        // 게임 런타임 중일 때만 생성한다
		if(Game.instance.state != Game.STATE.MOVE){
			return;
		}

        // 프레임 사이의 시간을 더한다
		mTime += Time.deltaTime;

        /*
         * 시간이 0 이하이면 생성하지 않는다
         * 
         * Start함수에서 mTime을 0 offset으로 지정했으므로
	     * mTime이 음수일 때는 움직이지 않도록 한다
	     * 
	     * 프레임마다 mTime가 증가되며 0을 넘으면 움직인다
         */
        if (mTime < 0f) {
			return;
		}

        // 생성할 시간이 되었는지 검사
		if (mTime >= intervalTime) {
            // 무작위 위치를 결정한다
			Vector3 randomPos = Vector3.one;
			randomPos.x = Random.Range(-randomPosRange.x,randomPosRange.x);
			randomPos.y = Random.Range(-randomPosRange.y,randomPosRange.y);

            // 지금 위치에 무작위 위치를 더한다
			Vector3 pos = transform.position + randomPos;

            // pos에 prefab 오브젝트를 생성한다
			GameObject obj = Instantiate(prefab,pos,transform.rotation) as GameObject;

            // 속도를 대입한다
			obj.GetComponent<Rigidbody2D>().velocity = velocity;

            // 시간이 지나면 사라지게 한다
			Destroy (obj,leftTime);

            // 시간을 0으로 되돌린다
			mTime = 0f;
		}
	}
}
