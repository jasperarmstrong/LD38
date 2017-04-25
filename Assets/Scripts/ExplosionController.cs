using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {
	private ObjectPool op;
	private ParticleSystem ps;
	private AudioSource audioSource;

	void Reset () {
		transform.localScale = Vector3.one;
	}
	
	public void Initialize (ObjectPool _op, float scale) {
		op = _op;
		ps = GetComponent<ParticleSystem>();
		audioSource = GetComponent<AudioSource>();
		transform.localScale = Vector3.one * scale;
	}

	public IEnumerator Explode() {
		ps.Play();
		audioSource.Play();
		while (ps != null && audioSource != null && (ps.isPlaying || audioSource.isPlaying)) {
			yield return null;
		}
		if (audioSource != null) {
			audioSource.time = 0;
		}
		op.DeactivateObject(gameObject);
	}
}
