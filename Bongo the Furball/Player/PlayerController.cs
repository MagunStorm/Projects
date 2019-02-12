using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Transform m_GroundPoint;
	public AudioClip m_jumpClip;
	public bool m_canFire = false;
	public int m_maxStones = 5;

	[HideInInspector] public bool lookingRight = true;		// glaga: czy futrzak patrzy w prawo. Uzywane przy odwracaniu postaci
	[HideInInspector] public bool isGrounded = false;		// flaga: czy futrzak jest na ziemi (Layer = "Ground"
	[HideInInspector] public bool isWater = false;			// flaga: czy gracz jest w wodzie
	[HideInInspector] public bool canMove;			// czy gracz może poruszać futrzakiem
	[HideInInspector] public bool m_autoMove = false;		// flaga: czy włączyć automatyczny ruch
//	[HideInInspector] 
	public int m_stones = 0;

	private float m_groundCheckRadius=0.15f;
	private Rigidbody2D rb2d;
	private Animator anim;
	private AudioSource m_playerAudio;
	private PlayerSetup m_playerSetup;
	private int m_currentJumpNo = 1;
	private float m_nextActionTime = 0f;
	private float m_bounceOfTime = 0.25f;
	private float m_bounceOfForce = 4f;
	private bool m_waterSurface = false;
	private float m_surfaceY;
	private GameObject m_OxygenUI;


	void Awake () {
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		m_playerAudio = GetComponent<AudioSource> ();
		m_playerSetup = GetComponent<PlayerSetup> ();

		//Pobranie flagi CanFire z danych zapisanych w DontDestroy
		m_canFire = GameDontDestroy.m_Instance.m_canfire;
		m_stones = GameDontDestroy.m_Instance.m_stones;

	}


	void Start(){
		m_OxygenUI = GameObject.FindGameObjectWithTag ("OxygenUI");
		if (m_OxygenUI != null) {
			m_OxygenUI.SetActive (false);
		} else {
			Debug.Log ("OxygenUI is null");
		}
	}


	void OnTriggerEnter2D(Collider2D other){
		if (LayerMask.LayerToName (other.gameObject.layer) == "Water") {
			isWater = true;
			anim.SetBool ("IsWater", isWater);
			m_surfaceY = other.gameObject.transform.position.y + other.gameObject.GetComponent<BuoyancyEffector2D> ().surfaceLevel;
			m_OxygenUI.SetActive (true);
		}

		if (other.CompareTag ("WaterSurface")) {
			m_waterSurface = true;
		} 
	}


	void OnTriggerExit2D(Collider2D other){
		if (LayerMask.LayerToName (other.gameObject.layer) == "Water") {
			isWater = false;
			anim.SetBool ("IsWater", isWater);
			m_OxygenUI.SetActive (false);
		}

		if (other.CompareTag ("WaterSurface")) {
			m_waterSurface = false;
		} 
	}


	void Update () {
		// sprawdzenie, czy futrzak dotyka ziemi
		bool checkGround = Physics2D.OverlapCircle (m_GroundPoint.position, m_groundCheckRadius, GameController.m_Instance.m_GroundLayer);
		if (!isGrounded && checkGround) {
			m_currentJumpNo = 1;
		}
		isGrounded = checkGround;
		if (m_waterSurface) {
			m_currentJumpNo = 1;
		}
		anim.SetBool ("IsGrounded", isGrounded);

		if (Input.GetButtonDown ("Jump") ) {
			Jump ();
		}

		// jeśli gracz jest poza wodą lub przy powierzchni, uzupełnij wskaźnik oddechu
//		if ((!isWater || m_waterSurface) && m_playerSetup.m_currentOxygen < m_playerSetup.m_maxOxygen && Time.time > m_nextActionTime) {
//			m_nextActionTime = Time.time + 0.5f;
//			m_playerSetup.UpdateOxygen (1);
//		}

		if (Input.GetButtonUp ("Fire1") ) {
			Fire (HUDController.m_Instance.m_slingShotSlider.value);
		}

	}


	void FixedUpdate()	{
		// automatyczny ruch w kierunku X. Uzywane przy animacji kończenia lub rozpoczęcia poziomu
		if (m_autoMove) {
			anim.SetFloat ("hSpeed", 1f);
			rb2d.velocity = new Vector2 (m_playerSetup.m_maxXSpeed*0.5f, rb2d.velocity.y);
		}
			
		if (!canMove) {
			return;
		}

		float hor = Input.GetAxis ("Horizontal");
		float ver = Input.GetAxis ("Vertical");


		/////////////////////////////////
		if (HUDController.m_Instance.m_Vjoystick.m_InputDirection.magnitude > 0.1f) {
			hor = HUDController.m_Instance.m_Vjoystick.m_InputDirection.x;
			ver = HUDController.m_Instance.m_Vjoystick.m_InputDirection.z;
		}
		if (HUDController.m_Instance.m_touchInput.m_InputDirection.magnitude > 0.1f) {
			hor = HUDController.m_Instance.m_touchInput.m_InputDirection.x;
			ver = HUDController.m_Instance.m_touchInput.m_InputDirection.y;
		}
		/////////////////////////////////


		if ((hor > 0 && !lookingRight) || (hor < 0 && lookingRight)) {
			Flip ();
		}
		float y = rb2d.velocity.y;
		if (isWater && Mathf.Abs (ver) > 0.1f) {
			y = ver * m_playerSetup.m_maxSwimSpeed;
			if (gameObject.transform.position.y >= m_surfaceY) {
				y *= 2;
			}
		} 
		rb2d.velocity = new Vector2 (Mathf.Clamp(hor + rb2d.velocity.x,-m_playerSetup.m_maxXSpeed,m_playerSetup.m_maxXSpeed), 
			Mathf.Clamp (y, -m_playerSetup.m_maxYSpeed, m_playerSetup.m_maxYSpeed));

		anim.SetFloat ("hSpeed", Mathf.Abs (hor));
		anim.SetFloat ("vSpeed", rb2d.velocity.y);
		anim.SetFloat ("vAbsSpeed", Mathf.Abs (ver));
	}


	public void Jump(){
		if (canMove && m_currentJumpNo <= m_playerSetup.m_MaxJumps) {
			rb2d.velocity = new Vector2 (rb2d.velocity.x, m_playerSetup.m_jumpForce);
			SetAudio (m_jumpClip);
			m_currentJumpNo++;
		}
	}


	public void Fire(float fireForce){
		if (m_canFire && m_stones>0 && GetComponent<ProjectileController> () != null) {
			ProjectileController pc = GetComponent<ProjectileController> ();
			if (canMove && !pc.m_isAutoFire && Time.time > pc.m_fireDelay) {
				pc.m_fireDelay = Time.time + pc.m_fireRate;
				m_stones--;
				pc.FireWithForce(fireForce);
			}
		}
	}


	public void StopMoving(){
		rb2d.velocity = Vector2.zero;
	}

	
	public void Flip(){
		lookingRight = !lookingRight;
		transform.Rotate (0f, 180f, 0f);
	}


	public void SetAudio(AudioClip clip){
		if (clip == null) {
			return;
		}
		m_playerAudio.clip = clip;
		m_playerAudio.Play ();
	}
		

	public void BounceOff(bool side){
		rb2d.velocity = Vector2.zero;
		canMove = false;
		// wektor odrzutu przeciwny do ruchu gracza
		Vector2 recoilVector = side ? Vector2.right : Vector2.left;
		recoilVector *= m_bounceOfForce;
		rb2d.AddForce(recoilVector,ForceMode2D.Impulse);
		StartCoroutine (CoolDown ());
	}


	IEnumerator CoolDown(){
		yield return new WaitForSeconds (m_bounceOfTime);
		canMove = true;
	}

}
