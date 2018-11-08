using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	private Vector3 cameraNewPosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
			cameraNewPosition = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
			transform.position = cameraNewPosition;
		}
	}
}
