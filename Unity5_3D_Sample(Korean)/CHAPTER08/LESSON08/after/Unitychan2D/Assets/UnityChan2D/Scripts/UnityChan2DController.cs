using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class UnityChan2DController : MonoBehaviour
{
	public float maxSpeed = 10f;
	// public float jumpPower = 1000f;
	
	// 점프할 때 Y축 방향 속도
	public float jumpSpeed = 30f;
	
	public Vector2 backwardForce = new Vector2(-4.5f, 5.4f);
	
	public LayerMask whatIsGround;
	
	private Animator m_animator;
	private BoxCollider2D m_boxcollier2D;
	private Rigidbody2D m_rigidbody2D;
	private bool m_isGround;
	
	// 2단 점프한다
	private bool mCanDoubleJump = false;
	
	private const float m_centerY = 1.5f;
	
	private State m_state = State.Normal;
	
	void Reset()
	{
		Awake();
		
		// UnityChan2DController
		maxSpeed = 10f;
		// jumpPower = 1000;
		// 점프할 때 Y축 방향 속도
		jumpSpeed = 30f;
		backwardForce = new Vector2(-4.5f, 5.4f);
		whatIsGround = 1 << LayerMask.NameToLayer("Ground");
		
		// Transform
		transform.localScale = new Vector3(1, 1, 1);
		
		// Rigidbody2D
		m_rigidbody2D.gravityScale = 3.5f;
		m_rigidbody2D.fixedAngle = true;
		
		// BoxCollider2D
		m_boxcollier2D.size = new Vector2(1, 2.5f);
		m_boxcollier2D.offset = new Vector2(0, -0.25f);
		
		// Animator
		m_animator.applyRootMotion = false;
	}
	
	void Awake()
	{
		m_animator = GetComponent<Animator>();
		m_boxcollier2D = GetComponent<BoxCollider2D>();
		m_rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	void Update()
	{
		if (m_state != State.Damaged)
		{
			float x = Input.GetAxis("Horizontal");
			bool jump = Input.GetButtonDown("Jump");
			Move(x, jump);
		}
	}
	
	void Move(float move, bool jump)
	{
		if (Mathf.Abs(move) > 0)
		{
			Quaternion rot = transform.rotation;
			transform.rotation = Quaternion.Euler(rot.x, Mathf.Sign(move) == 1 ? 0 : 180, rot.z);
		}
		
		m_rigidbody2D.velocity = new Vector2(move * maxSpeed, m_rigidbody2D.velocity.y);
		
		m_animator.SetFloat("Horizontal", move);
		m_animator.SetFloat("Vertical", m_rigidbody2D.velocity.y);
		m_animator.SetBool("isGround", m_isGround);

        // 점프 버튼이 눌렸으며 지면에 접지한 상태인지 검사
		if (jump && m_isGround){
			// 점프한다
			JumpAction();
			// 2단 점프할 수 있게 한다
			mCanDoubleJump = true;

            // 점프 버튼이 눌렸으며 지면에 접지한 상태인지 검사
		} else if(jump && mCanDoubleJump){
			
			// 점프한다
			JumpAction();
            // 2단 점프를 할 수 없게 설정한다
			mCanDoubleJump = false;
		}
	}
	
	/*
    *점프하는 함수
    */
	void JumpAction(){
		m_animator.SetTrigger("Jump");
		SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
		// m_rigidbody2D.AddForce(Vector2.up * jumpPower);
		
		// 현재 속도를 일시적으로 유지
		Vector3 tmpVelocity = m_rigidbody2D.velocity;
		// Y축 방향만 값을 대입한다
		tmpVelocity.y = jumpSpeed;
        // 새로운 속도를 저장한다
		m_rigidbody2D.velocity = tmpVelocity;
	}
	
	void FixedUpdate()
	{
		Vector2 pos = transform.position;
		Vector2 groundCheck = new Vector2(pos.x, pos.y - (m_centerY * transform.localScale.y));
		Vector2 groundArea = new Vector2(m_boxcollier2D.size.x * 0.49f, 0.05f);
		
		m_isGround = Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, whatIsGround);
		m_animator.SetBool("isGround", m_isGround);
	}
	
	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "DamageObject" && m_state == State.Normal)
		{
			m_state = State.Damaged;
			StartCoroutine(INTERNAL_OnDamage());
		}
	}
	
	IEnumerator INTERNAL_OnDamage()
	{
		m_animator.Play(m_isGround ? "Damage" : "AirDamage");
		m_animator.Play("Idle");
		
		SendMessage("OnDamage", SendMessageOptions.DontRequireReceiver);
		
		m_rigidbody2D.velocity = new Vector2(transform.right.x * backwardForce.x, transform.up.y * backwardForce.y);
		
		yield return new WaitForSeconds(.2f);
		
		while (m_isGround == false)
		{
			yield return new WaitForFixedUpdate();
		}
		m_animator.SetTrigger("Invincible Mode");
		m_state = State.Invincible;
	}
	
	void OnFinishedInvincibleMode()
	{
		m_state = State.Normal;
	}
	
	enum State
	{
		Normal,
		Damaged,
		Invincible,
	}
}
