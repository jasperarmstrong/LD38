using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour {
	public bool isDead = false;
	[SerializeField] private float movementSpeed = 10;
	[SerializeField] private float laserSpawnDistance = 1.5f;

	[SerializeField] private float explosionScale = 0.2f;
	
	[SerializeField] private int minNumScrap = 0;
	[SerializeField] private int maxNumScrap = 5;

	[Tooltip("One shot will go off every framesPerShot fixedUpdates")]
	[SerializeField] private int framesPerShot = 10;
	[SerializeField] private float timeBeforeFirstShot = 5;
	private bool canShoot = false;
	private int currentShotFrame;

	private Rigidbody2D rb;
	private SpriteRenderer laserEmitter;
	private ObjectPool op, lop, sop;

	private AudioSource audioSource, audioSourceGeneric;
	[SerializeField] private AudioClip soundShot;
	[SerializeField] private AudioClip soundLaserStart;

	private IEnumerator WaitBeforeShooting() {
		float time = timeBeforeFirstShot;
		while (time > 0) {
			yield return new WaitForSeconds(0.5f);
			laserEmitter.color = new Color(
				laserEmitter.color.r,
				laserEmitter.color.g,
				laserEmitter.color.b,
				(timeBeforeFirstShot - time) / timeBeforeFirstShot
			);
			time -= 0.5f;
		}
		PlaySound(soundLaserStart);
		canShoot = true;
	}

	public void Initialize(ObjectPool _op, ObjectPool _lop, ObjectPool _sop) {
		rb = GetComponent<Rigidbody2D>();
		laserEmitter = transform.FindChild("LaserEmitter").GetComponent<SpriteRenderer>();
		op = _op;
		lop = _lop;
		sop = _sop;
		audioSource = GetComponent<AudioSource>();
		audioSourceGeneric = transform.FindChild("GenericSound").GetComponent<AudioSource>();
		audioSourceGeneric.time = 0;
		audioSourceGeneric.Play();
		StartCoroutine(WaitBeforeShooting());
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position - (transform.up * laserSpawnDistance), 0.05f);
	}

	public void Deactivate() {
		audioSourceGeneric.Pause();
		op.DeactivateObject(gameObject);
	}

	void Die() {
		if (!isDead) {
			isDead = true;

			for (int i = 0; i < Random.Range(minNumScrap, maxNumScrap); i++) {
				sop.InstantiatePooled(
					new Vector3(
						transform.position.x + Random.Range(0f, 1f),
						transform.position.y + Random.Range(0f, 1f),
						transform.position.z
					),
					Quaternion.identity
				);
			}

			Deactivate();

			GameManager.em.SpawnExplosion(transform.position, explosionScale);

			if (!GameManager.pc.isDead && !GameManager.planet.isDead) {
				GameManager.pc.kills++;
			}
		}
	}

	public void Reset() {
		isDead = false;
		canShoot = false;
		laserEmitter.color = new Color(
			laserEmitter.color.r,
			laserEmitter.color.g,
			laserEmitter.color.b,
			0
		);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (!isDead && other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerController>().OnUFOHit();
		}
	}

	public void OnBulletHit() {
		Die();
	}

	void PlaySound(AudioClip clip) {
		audioSource.clip = clip;
		audioSource.time = 0;
		audioSource.Play();
	}

	void ShootIfShould() {
		if (!canShoot) {
			return;
		}

		if (currentShotFrame >= framesPerShot) {
			if (lop.hasRoom()) {
				PlaySound(soundShot);
				lop.InstantiatePooled(
					transform.position - (transform.up * laserSpawnDistance),
					transform.rotation
				);
				currentShotFrame = 0;
			}
		} else {
			currentShotFrame++;
		}
	}

	void FixedUpdate() {
		if (GameManager.isPaused && audioSourceGeneric.isPlaying) {
			audioSourceGeneric.Pause();
		} else if (!GameManager.isPaused && !audioSourceGeneric.isPlaying) {
			audioSourceGeneric.Play();
		}

		if (isDead) {
			if (rb.velocity.magnitude > 0) {
				rb.velocity = Vector3.zero;
			}
			return;
		}

		float movementDir = Random.Range(-1f, 1f);
		rb.AddForce(transform.right * movementDir * movementSpeed);

		ShootIfShould();
	}
}
