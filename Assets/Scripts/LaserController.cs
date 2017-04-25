using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {
	private ObjectPool lop;

	[SerializeField] private float speed = 15;
	private LayerMask groundLayer;

	void Start () {
		groundLayer = (1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Player"));
	}

	public void Reset() {

	}

	public void Initialize(ObjectPool _lop) {
		lop = _lop;
	}
	
	void FixedUpdate() {
		Vector3 moveVector = -transform.up * speed * Time.fixedDeltaTime;
		RaycastHit2D hit = Physics2D.Raycast(
			transform.position,
			-transform.up,
			moveVector.magnitude,
			groundLayer
		);

		if (hit) {
			lop.DeactivateObject(gameObject);
			switch (hit.collider.gameObject.tag) {
				case "Player":
					hit.collider.gameObject.GetComponent<PlayerController>().OnLaserHit();
					break;
				case "Planet":
					hit.collider.gameObject.GetComponent<PlanetController>().OnLaserHit();
					break;
				default:
					break;
			}
		} else {
			transform.position += moveVector;
		}
	}
}
