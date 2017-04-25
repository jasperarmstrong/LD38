using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	protected void BaseStart() {
		GameManager.ui.Register(gameObject.name, this);
	}
	
	public virtual void UpdateUI(object value) {
		Debug.Log("Need to implement UpdateUI(object value) on class deriving from UIController.");
	}

	public virtual void UpdateUI(float value, float value2) {
		Debug.Log("Need to implement UpdateUI(float value, float value2) on class deriving from UIController.");
	}

	public virtual void UpdateUI(bool value) {
		Debug.Log("Need to implement UpdateUI(bool value) on class deriving from UIController.");
	}
}
