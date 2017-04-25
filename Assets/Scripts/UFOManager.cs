using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOManager : MonoBehaviour {
	private ObjectPool op, lop, sop;

	[SerializeField] private float minHeight = 8;
	[SerializeField] private float maxHeight = 12;

	[Tooltip("Every FixedUpdate, there will be a 1 in spawnProbability chance of a ufo spawning if there is room in the object pool")]
	[SerializeField] private int spawnProbability = 60;

	void Start () {
		op = GetComponent<ObjectPool>();
		lop = transform.FindChild("LaserPool").GetComponent<ObjectPool>();
		sop = transform.FindChild("ScrapPool").GetComponent<ObjectPool>();
	}
	
	void FixedUpdate () {
		if (GameManager.pc.isDead || GameManager.planet.isDead) {
			return;
		}
		
		if (Random.Range(0, spawnProbability) == 0 && op.hasRoom()) {
			float height = Random.Range(minHeight, maxHeight);
			Vector3 planetEdgePoint = GameManager.planet.GetPointOnEdge(Random.Range(0f, 360f));
			GameObject obj = op.InstantiatePooled(
				GameManager.planet.transform.position + planetEdgePoint + (planetEdgePoint.normalized * height),
				Quaternion.identity,
				false
			);
			obj.GetComponent<UFOController>().Initialize(op, lop, sop);
			obj.GetComponent<GravityReceiver>().targetHeight = height;
		}
	}
}
