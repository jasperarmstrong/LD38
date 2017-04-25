using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour {
	private float lerpAmount = 0.8f;
	
	private GameObject _target;
	private GameObject target {
		get {
			if (_target == null) {
				_target = GameObject.FindGameObjectWithTag("Player");
			}
			return _target;
		}
	}


	private PlayerController _pc;
	private PlayerController pc {
		get {
			if (_pc == null && target != null) {
				_pc = target.GetComponent<PlayerController>();
			}
			return _pc;
		}
	}
	
	void FixedUpdate () {
		if (target == null) {
			return;
		}
		Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, targetPosition, lerpAmount);
		if (!pc.isDead) {
			transform.up = Vector3.Lerp(transform.up, target.transform.up, lerpAmount);
		}
	}
}
