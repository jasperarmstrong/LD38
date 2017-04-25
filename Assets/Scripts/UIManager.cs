using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
	private Dictionary<string, UIController> uiControllers;

	void Start() {
		uiControllers = new Dictionary<string, UIController>();
	}

	public void Register(string name, UIController controller) {
		if (uiControllers != null) {
			uiControllers[name] = controller;
		}
	}

	public void UpdateUI(string name, object value) {
		if (uiControllers != null) {
			uiControllers[name].UpdateUI(value);
		}
	}

	public void UpdateUI(string name, bool value) {
		if (uiControllers != null) {
			uiControllers[name].UpdateUI(value);
		}
	}

	public void UpdateUI(string name, float value, float value2) {
		if (uiControllers != null) {
			uiControllers[name].UpdateUI(value, value2);
		}
	}
}
