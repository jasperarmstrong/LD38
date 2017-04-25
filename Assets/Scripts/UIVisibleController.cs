using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVisibleController : UIController {
	[SerializeField] private GameObject uiObj;
	
	void Start () {
		base.BaseStart();
	}

	public override void UpdateUI(bool value) {
		uiObj.SetActive(value);
	}
}
