using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "UFO") {
			other.gameObject.GetComponent<UFOController>().Deactivate();
		}
	}
}
