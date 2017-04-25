using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowController : UIController {
	[SerializeField] private GameObject[] windows;
	private GameObject button;
	private Image image;
	
	void Start () {
		base.BaseStart();
		button = transform.FindChild("Back").gameObject;
		image = GetComponent<Image>();
	}
	
	public override void UpdateUI(object value) {
		if (value == null) {
			foreach (GameObject obj in windows) {
				obj.SetActive(false);
			}
			button.SetActive(false);
			image.enabled = false;
		} else {
			button.SetActive(true);
			image.enabled = true;
			foreach (GameObject obj in windows) {
				if (obj.name == value.ToString()) {
					obj.SetActive(true);
				}
			}
		}
	}
}

