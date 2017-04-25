using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLayerController : MonoBehaviour {
	[SerializeField] private List<GameObject> pieces;
	private List<SpriteRenderer> srs;

	void Start() {
		srs = new List<SpriteRenderer>();
		for (int i = 0; i < pieces.Count; i++) {
			srs.Add(pieces[i].GetComponent<SpriteRenderer>());
		}
	}
	
	public void SetLayer(string layer) {
		for (int i = 0; i < srs.Count; i++) {
			if (srs[i].sortingLayerName == layer) {
				return;
			}
			srs[i].sortingLayerName = layer;
		}
	}
}
