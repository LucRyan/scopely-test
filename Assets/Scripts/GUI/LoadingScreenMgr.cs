using UnityEngine;
using System.Collections;

public class LoadingScreenMgr : MonoBehaviour {
	
	
	public GameObject sword;
	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		//sword.transform.localPosition(new Vector3(
		sword.transform.Rotate(new Vector3(0f, 2f, 0f));
	}
}
