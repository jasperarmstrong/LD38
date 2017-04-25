using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageController : MonoBehaviour {
	private bool playerInRange = false;
	private GameObject ui;
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			playerInRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			playerInRange = false;
		}
	}

	void Start() {
		ui = transform.FindChild("GarageUI").gameObject;
	}

	void Update() {
		if (GameManager.pc.isDead || GameManager.planet.isDead || GameManager.isPaused) {
			if (ui.activeInHierarchy) {
				ui.SetActive(false);
			}
			return;
		}

		if (playerInRange) {
			if (!ui.activeInHierarchy) {
				ui.SetActive(true);
			}
		} else {
			if (ui.activeInHierarchy) {
				ui.SetActive(false);
				GameManager.canShoot = true;
			}
		}
	}
}
