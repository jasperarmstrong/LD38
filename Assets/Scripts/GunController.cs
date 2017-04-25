using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
	public int maxBullets;

	[SerializeField] private int _numBulletsInMagazine;
	public int numBulletsInMagazine {
		get {
			return _numBulletsInMagazine;
		}
		set {
			_numBulletsInMagazine = value;
			UpdateUI();
		}
	}

	[SerializeField] private int _numBullets;
	public int numBullets {
		get {
			return _numBullets;
		}
		set {
			_numBullets = value;
			UpdateUI();
		}
	}

	private bool isReloading = false;
	
	private Transform bulletSpawnTransform;

	private PlayerController pc;
	private ObjectPool op;

	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private float bulletSpeed;

	private AudioSource audioSource;
	[SerializeField] private AudioClip soundShot;
	[SerializeField] private AudioClip soundReload;
	[SerializeField] private AudioClip soundClick;
	[SerializeField] private AudioClip soundCock;

	void UpdateUI() {
		GameManager.ui.UpdateUI("AmmoAmount", numBulletsInMagazine.ToString() + " / " + numBullets.ToString());				
	}

	public void Initialize (PlayerController _pc) {
		pc = _pc;
		op = GetComponent<ObjectPool>();
		bulletSpawnTransform = transform.FindChild("BulletExit");

		audioSource = GetComponent<AudioSource>();

		UpdateUI();
	}

	void PlaySound(AudioClip clip) {
		audioSource.clip = clip;
		audioSource.Play();
	}

	private IEnumerator Reload(int numToReload) {
		while (numToReload > 0) {
			PlaySound(soundReload);
			if (numBullets < 1) {
				break;
			}
			numBulletsInMagazine++;
			numBullets--;
			numToReload--;
			yield return new WaitForSeconds(soundReload.length);
		}
		PlaySound(soundCock);
		isReloading = false;
	}

	void Reload() {
		if (numBulletsInMagazine == maxBullets || numBullets < 1) {
			isReloading = false;
			return;
		}
		isReloading = true;

		StartCoroutine(Reload(maxBullets - numBulletsInMagazine));
	}

	void Shoot() {
		if (numBulletsInMagazine > 0) {
			GameObject obj = op.InstantiatePooled(
				bulletSpawnTransform.position,
				Quaternion.identity
			);
			obj.transform.up = GameManager.mousePos - bulletSpawnTransform.position;
			obj.GetComponent<Rigidbody2D>().velocity = obj.transform.up * bulletSpeed;
			numBulletsInMagazine--;
			PlaySound(soundShot);
		} else {
			PlaySound(soundClick);
		}
	}
	
	void Update () {
		if (pc.isDead || GameManager.isPaused) {
			return;
		}

		if (Input.GetMouseButtonDown(0) && !isReloading && GameManager.canShoot) {
			Shoot();
		}

		if (Input.GetButtonDown("Reload Gun") && !isReloading) {
			Reload();
		}
	}
}
