using UnityEngine;
using System.Collections;

namespace UnitySampleAssets.CrossPlatformInput.PlatformSpecific
{
public class PlayerFighterController : MonoBehaviour {


	public GameObject explosionParticles;
	private GameObject gameController;
	private PlayerController pc;
	// Use this for initialization
	void Start () {
		gameController = GameObject.Find ("GameController");
		pc = GameObject.Find ("player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Enemy") {
			if(pc.isPowerUpActive==false){
				GameObject go = Instantiate(explosionParticles, gameObject.transform.position, Quaternion.identity) as GameObject;
				Destroy(go, 5.0f);
				gameController.SendMessage("OnPlayerDestruction");
				Destroy(this.gameObject);
			}
		}
	}
}

}