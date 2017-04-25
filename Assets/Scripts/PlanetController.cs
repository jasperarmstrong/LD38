using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {
	[SerializeField] private float edgeRadius;

	[SerializeField] private int bulletDamage = 5;
	[SerializeField] private int laserDamage = 15;

	[SerializeField] private float explosionScale = 0.5f;

	public bool isDead = false;
	public int maxHealth = 1000;
	private int _health;
	public int health {
		get {
			return _health;
		}
		set {
			_health = value;
			if (_health > maxHealth) {
				_health = maxHealth;
			} else if (_health < 1) {
				_health = 0;
				Die();
			}
			GameManager.ui.UpdateUI("PlanetHealth", (float)_health, (float)maxHealth);
		}
	}

	void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, edgeRadius);
    }

	public Vector3 GetPointOnEdge(float angle) {
		angle *= Mathf.Deg2Rad;
		return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * edgeRadius;
	}

	void Die() {
		isDead = true;
		GetComponent<GravitySource>().Explode();
		GameManager.em.SpawnExplosion(transform.position, explosionScale);
		GameManager.ui.UpdateUI("GameOver", true);
	}

	void Start() {
		GameManager.SetPlanet(this);

		health = maxHealth;
	}

	public void OnBulletHit() {
		health -= bulletDamage;
	}

	public void OnLaserHit() {
		health -= laserDamage;
	}
}
