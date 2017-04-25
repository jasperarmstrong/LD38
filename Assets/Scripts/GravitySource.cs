using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour {
	public float gravityScale = 9.8f;
	public float gravityRadius = 15f;
	public float explosionForce = 5000f;
	
	private bool shouldExplode = false;
	private bool hasExploded = false;

	void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, gravityRadius);
    }

	void FixedUpdate() {
		if (hasExploded) {
			return;
		}

		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, gravityRadius);

		if (shouldExplode) {
			hasExploded = true;
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<PolygonCollider2D>().enabled = false;
			GameManager.pc.health -= GameManager.pc.health;
			Camera.main.GetComponent<SmoothFollow>().enabled = false;
			for (int i = 0; i < cols.Length; i++){
				if (cols[i].gameObject.tag == "UFO") {
					continue;
				}

				GravityReceiver gr = cols[i].GetComponent<GravityReceiver>();

				if (gr != null) {
					gr.alignUp = false;
					gr.counteractGravity = false;
					Vector3 force = (cols[i].transform.position - transform.position).normalized * explosionForce;
					Rigidbody2D rb = cols[i].GetComponent<Rigidbody2D>();
					rb.drag = 0;
					rb.AddForce(force * rb.mass);
				}
			}
		} else {
			for (int i = 0; i < cols.Length; i++){
				GravityReceiver gr = cols[i].GetComponent<GravityReceiver>();
				if (gr != null) {
					Vector3 force = (transform.position - cols[i].transform.position).normalized * gravityScale;
					cols[i].GetComponent<Rigidbody2D>().AddForce(force);
					if (gr.alignUp) {
						cols[i].transform.up = cols[i].transform.position - transform.position;
					}
					gr.force += force;
				}
			}
		}
	}

	public void Explode() {
		shouldExplode = true;
	}
}
