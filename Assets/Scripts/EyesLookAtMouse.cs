using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesLookAtMouse : MonoBehaviour {
	[SerializeField] private Transform eyeL;
	[SerializeField] private Transform eyeR;
	
	private PlayerController pc;
	
	void Start() {
		pc = GetComponent<PlayerController>();
	}

	void Update () {
		if (pc.isDead || GameManager.isPaused) {
			return;
		}
		eyeL.up = GameManager.mousePos - eyeL.position;
		eyeR.up = GameManager.mousePos - eyeR.position;
	}
}
