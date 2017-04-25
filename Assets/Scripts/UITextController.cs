using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextController : UIController {
	private Text text;
	private string initialValue;
	
	[SerializeField] private string format;
	
	void Start () {
		base.BaseStart();
		text = GetComponent<Text>();
		initialValue = text.text;
	}
	
	public override void UpdateUI(object value) {
		if (value is float) {
			value = string.Format("{0:n}", value);
		}
		if (format != "") {
			text.text = string.Format(format, value.ToString());
		} else {
			text.text = value.ToString();
		}
	}
}
