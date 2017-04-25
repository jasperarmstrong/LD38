using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
	private bool shouldAlignVel = true;

	public float timeSinceExitedGun = 0;

	private Rigidbody2D rb;
	private ObjectPool op;

	void OnCollisionEnter2D(Collision2D other) {
		shouldAlignVel = false;

		if ((timeSinceExitedGun) < 0.3f && other.gameObject.tag == "Player") {
			return;
		}

		switch (other.gameObject.tag) {
			case "Player":
				other.gameObject.GetComponent<PlayerController>().OnBulletHit();
				break;
			case "UFO":
				other.gameObject.GetComponent<UFOController>().OnBulletHit();
				break;
			case "Planet":
				other.gameObject.GetComponent<PlanetController>().OnBulletHit();
				break;
			default:
				break;
		}

		op.DeactivateObject(gameObject);

	}

	public void Reset() {
		timeSinceExitedGun = 0;
		shouldAlignVel = true;
	}

	public void Initialize(ObjectPool _op) {
		op = _op;
	}

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	void Update() {
		if (shouldAlignVel) {
			transform.up = rb.velocity;
		}
		timeSinceExitedGun += Time.deltaTime;
	}
}
