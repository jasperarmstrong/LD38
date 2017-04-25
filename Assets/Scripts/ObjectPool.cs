using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
	[SerializeField] private int maxObjects = 50;
	[SerializeField] private bool disableGameObjectsWhenIdle = true;
	[SerializeField] private GameObject prefab;
	[SerializeField] private Transform parentTransform;

	private List<GameObject> active;
	private List<GameObject> idle;

	void Start () {
		if (!parentTransform) {
			parentTransform = GameObject.Find("ObjectPool").transform;
		}
		GameObject go = new GameObject();
		go.name = prefab.name + " Pool";
		go.transform.position = Vector3.zero;
		go.transform.parent = parentTransform;
		parentTransform = go.transform;

		active = new List<GameObject>();
		idle = new List<GameObject>();
	}

	public bool hasRoom() {
		return idle.Count > 0 || active.Count + idle.Count < maxObjects;
	}

	public GameObject InstantiatePooled(Vector3 position, Quaternion rotation, bool init = true) {
		GameObject go = null;
		if (active.Count + idle.Count >= maxObjects) {
			if (idle.Count > 0) {
				//"popping" the first element in the idle objects list
				go = idle[0];
				idle.RemoveAt(0);
				active.Add(go);

				if (!go.activeInHierarchy) {
					go.SetActive(true);
				}
				go.SendMessage("Reset");
				go.transform.position = position;
				go.transform.rotation = rotation;
				return go;
			}
		}
		go = (GameObject)Instantiate(prefab, position, rotation, parentTransform);
		if (init) {
			go.SendMessage("Initialize", this);
		}
		go.name = prefab.name;
		active.Add(go);
		return go;
	}

	IEnumerator DeactivateGOWhenAudioDone(GameObject obj, AudioSource audioSource) {
		while (audioSource.isPlaying) {
			yield return new WaitForSeconds(0.1f);
		}
		obj.SetActive(false);
	} 

	public void DeactivateObject(GameObject obj, AudioSource audioSource = null) {
		active.Remove(obj);
		idle.Add(obj);
		if (disableGameObjectsWhenIdle && obj.activeInHierarchy) {
			if (audioSource != null) {
				StartCoroutine(DeactivateGOWhenAudioDone(obj, audioSource));
			} else {
				obj.SetActive(false);
			}
		}
		// Debug.Log(string.Format("active: {0} / idle: {1}", objects[obj.name + "-active"].Count, objects[obj.name + "-idle"].Count));
	}
}
