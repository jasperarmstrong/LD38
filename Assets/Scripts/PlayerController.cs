using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	[SerializeField] private float movementSpeed = 5f;
	[SerializeField] private float jumpForce = 20f;
	[SerializeField] private float animWalkSpeedMultiplier = 1f;
	
	[SerializeField] private int bulletDamage = 500;
	[SerializeField] private int laserDamage = 100;
	[SerializeField] private int ufoHitDamage = 250;

	public int maxHealth = 1000;
	private int _health;
	public int health {
		get {
			return _health;
		}
		set {
			_health = value;
			if (_health > maxHealth) {
				_health = maxHealth;
			} else if (_health < 1) {
				_health = 0;
				Die();
			}
			GameManager.ui.UpdateUI("PlayerHealth", (float)_health, (float)maxHealth);
		}
	}

	public bool isDead = false;
	private int _kills = 0;
	public int kills {
		get {
			return _kills;
		}
		set {
			_kills = value;
			if (_kills > GameManager.highScore) {
				GameManager.highScore = _kills;
			}
			GameManager.ui.UpdateUI("Score", _kills);
		}
	}

	private int _scrap;
	public int scrap {
		get {
			return _scrap;
		}
		set {
			_scrap = value;
			GameManager.ui.UpdateUI("ScrapAmount", _scrap);
		}
	}

	private bool shouldJump = false;
	public int maxJumps = 1;
	public int numJumps;
	
	private Rigidbody2D rb;
	private Animator anim;
	private GravityReceiver gr;

	[SerializeField] private GameObject armL;
	[SerializeField] private GameObject handL;
	private Quaternion armLInitRotation;
	[SerializeField] private GameObject armR;
	[SerializeField] private GameObject handR;
	private Quaternion armRInitRotation;

	[SerializeField] private GameObject gunPrefab;
	public GameObject gun;

	float mouseXRelToUp;

	void Start() {
		GameManager.SetPlayer(this);
		GameManager.LoadHighScore();
		GameManager.ui.UpdateUI("GameOver", false);

		numJumps = maxJumps;
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		gr = GetComponent<GravityReceiver>();

		armLInitRotation = armL.transform.localRotation;
		armRInitRotation = armR.transform.localRotation;

		GunController gunCont = ((GameObject)Instantiate(gunPrefab, handL.transform.position, Quaternion.identity, handL.transform)).GetComponent<GunController>();
		gunCont.Initialize(this);
		gun = gunCont.gameObject;

		health = maxHealth;
		kills = 0;
		scrap = 0;
	}

	void RotateArm() {
		if (mouseXRelToUp < -0.01f) {
			armR.transform.right = -(GameManager.mousePos - armR.transform.position);
		} else if (mouseXRelToUp > 0.01f) {
			armL.transform.right = -(GameManager.mousePos - armL.transform.position);
		}
	}

	void RotatePlayer() {
		mouseXRelToUp = -Vector3.Cross(transform.up, GameManager.mousePos - transform.position).z;
		if (mouseXRelToUp < -0.01f) {
			if (transform.localScale.x > 0) {
				transform.localScale = new Vector3(-1, 1, 1);
			}

			armL.GetComponent<ArmLayerController>().SetLayer("PlayerForeground");
			armR.GetComponent<ArmLayerController>().SetLayer("PlayerBackground");
			gun.GetComponent<SpriteRenderer>().sortingLayerName = "PlayerBackground";
			
			gun.transform.parent = handR.transform;
			gun.transform.localPosition = Vector3.zero;
			gun.transform.right = -(GameManager.mousePos - transform.position);

			anim.SetBool("isFacingRight", false);
		} else if (mouseXRelToUp > 0.01f) {
			if (transform.localScale.x < 0) {
				transform.localScale = new Vector3(1, 1, 1);
			}

			armL.GetComponent<ArmLayerController>().SetLayer("PlayerForeground");
			armR.GetComponent<ArmLayerController>().SetLayer("PlayerBackground");
			gun.GetComponent<SpriteRenderer>().sortingLayerName = "PlayerForeground";

			gun.transform.parent = handL.transform;
			gun.transform.localPosition = Vector3.zero;
			gun.transform.right = GameManager.mousePos - transform.position;

			anim.SetBool("isFacingRight", true);
		}
	}

	void Update() {
		if (isDead || GameManager.isPaused) {
			return;
		}

		if (GameManager.vertical > 0 || Input.GetButtonDown("Jump")) {
			shouldJump = true;
		}

		RotatePlayer();
	}

	void LateUpdate() {
		if (isDead || GameManager.isPaused) {
			return;
		}

		RotateArm();
	}

	void Die() {
		if (!isDead) {
			isDead = true;
			anim.SetBool("isDead", true);
			gr.alignUp = false;
			GameManager.ui.UpdateUI("GameOver", true);
		}
	}

	public void OnBulletHit() {
		health -= bulletDamage;
	}

	public void OnLaserHit() {
		health -= laserDamage;
	}

	public void OnUFOHit() {
		health -= ufoHitDamage;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Planet") {
			numJumps = maxJumps;
			anim.SetBool("isJumping", false);
		}
	}

	void FixedUpdate() {
		if (isDead || GameManager.isPaused) {
			return;
		}

		if (GameManager.horizontal < -0.1f || GameManager.horizontal > 0.1f) {
			anim.SetBool("isWalking", true);
			anim.SetFloat("walkSpeed", animWalkSpeedMultiplier * rb.velocity.magnitude);
		} else {
			anim.SetBool("isWalking", false);
		}
		rb.AddForce(transform.right * GameManager.horizontal * movementSpeed);
		if (shouldJump) {
			shouldJump = false;
			if (numJumps > 0) {
				numJumps--;
				rb.AddForce(transform.up * jumpForce);
				anim.SetBool("isJumping", true);
			}
		}
	}
}
