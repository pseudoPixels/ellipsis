using UnityEngine;
using System.Collections;

public class EnemyFighterController : MonoBehaviour {

	public GameObject explosionParticles;
	private GameObject gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.Find ("GameController");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Enemy") {
			GameObject go = Instantiate(explosionParticles, gameObject.transform.position, Quaternion.identity) as GameObject;
			gameController.SendMessage("OnFighterDestruction");
			Destroy(go, 5.0f);
			Destroy(this.gameObject);
		}
	}
}
