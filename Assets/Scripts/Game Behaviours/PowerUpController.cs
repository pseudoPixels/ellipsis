using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {

	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("player");
		Destroy (gameObject, 3.50f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			player.SendMessage("onPowerUpCollection");
			Destroy(gameObject);
		}
	}
}
