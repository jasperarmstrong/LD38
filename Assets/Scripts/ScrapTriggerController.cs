﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapTriggerController : MonoBehaviour {
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerController>().scrap++;
			transform.parent.GetComponent<ScrapController>().Deactivate();
		}
	}
}
