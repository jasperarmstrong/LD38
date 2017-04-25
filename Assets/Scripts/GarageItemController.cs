using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RewardType {
	AMMO, HEALTH, PLANETHEALTH
}

public class GarageItemController : MonoBehaviour {
	public int price;
	public int amount;
	public RewardType rewardType;

	void Start () {
		foreach (Text text in GetComponentsInChildren<Text>()) {
			switch (text.gameObject.name) {
				case "Price":
					text.text = price.ToString();
					break;
				case "Amount":
					if (rewardType == RewardType.AMMO) {
						text.text = amount.ToString();
					} else {
						text.text = "+" + amount.ToString() + "%";
					}
					break;
				default:
					break;
			}
		}
	}
	
	public void OnClick() {
		if (GameManager.pc.isDead || GameManager.planet.isDead || GameManager.isPaused) {
			return;
		}

		if (GameManager.pc.scrap >= price) {
			GameManager.pc.scrap -= price;
			switch (rewardType) {
				case RewardType.AMMO:
					GameManager.pc.gun.GetComponent<GunController>().numBullets += amount;
					break;
				case RewardType.HEALTH:
					GameManager.pc.health += (int)(((float)amount / 100f) * GameManager.pc.maxHealth);
					break;
				case RewardType.PLANETHEALTH:
					GameManager.planet.health += (int)(((float)amount / 100f) * GameManager.pc.maxHealth);
					break;
				default: 
					break;
			}
		}
	}
}
