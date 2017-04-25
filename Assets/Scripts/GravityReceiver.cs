using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityReceiver : MonoBehaviour {
    public Vector3 force;

    public bool alignUp = false;

    public bool counteractGravity = false;
    public float targetHeight = -1;

    private Rigidbody2D rb;
    private LayerMask groundLayer;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = (1 << LayerMask.NameToLayer("Ground"));
    }

    void FixedUpdate() {
        if (counteractGravity) {
            if (targetHeight > 0) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, targetHeight, groundLayer);
                if (hit) {
                    rb.AddForce(-force * 1.5f);
                } else {
                    rb.AddForce(-force * 0.5f);
                }
            } else {
                rb.AddForce(-force);
            }
        }
        force = Vector3.zero;
    }
}
