using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarController : UIController {
	private float maxWidth;
	
	private RectTransform _rt;
	private RectTransform rt {
		get {
			if (_rt == null) {
				_rt = GetComponent<RectTransform>();
			}
			return _rt;
		}
	}

	
	void Start () {
		base.BaseStart();
		maxWidth = rt.rect.width;
	}
	
	public override void UpdateUI(float value, float value2) {
		rt.sizeDelta = new Vector2((value / value2) * maxWidth, rt.rect.height);
	}
}
