using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class GameController : MonoBehaviour {

	public string yourInterstitialAdID = "ca-app-pub-9495227778204546/6517622115";
	public Transform[] spawnPositions;
	public GameObject[] fighters;

	public Text scoreUI;
	private float elapsedTime = 0;
	private int playerScore = 0;
	public Canvas canvas;

	private bool isGameOver = false;
	private float gameOverElapsedTime = 0;

	public AudioClip explosionSFX;


	InterstitialAd interstitial;

	public GameObject shp;

	private float powerUpElapsedTime = 0;

	//public GameObject leaderBoardController;

	// Use this for initialization
	void Start () {
		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(yourInterstitialAdID);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}
	
	// Update is called once per frame
	void Update () {
		powerUpElapsedTime += Time.deltaTime;
		if (powerUpElapsedTime >= 8.0f) {
			Debug.Log("powerup created...");
			powerUpElapsedTime = 0;
			Vector3 screenPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Random.Range (0, Screen.width), Random.Range (0, Screen.height), 0));
			Instantiate (shp, screenPosition, Quaternion.identity);
		}


		elapsedTime += Time.deltaTime;
		if (elapsedTime >= 1.0f) {
			if(isGameOver==false)playerScore += 1;
			scoreUI.text = " " + playerScore.ToString();
			elapsedTime = 0;

			if(PlayerPrefs.GetInt("USER_BEST_SCORE")< playerScore){
				//new WorldWideHighscores().AddNewHighscore("Mostaeen", playerScore);
				PlayerPrefs.SetInt("USER_BEST_SCORE", playerScore);
			}
		}

		if (isGameOver == true) {
			gameOverElapsedTime += Time.deltaTime;
			if(gameOverElapsedTime >= 2.50f){
				if (interstitial.IsLoaded()) {
					interstitial.Show();
				}
				//leaderBoardController.SendMessage("AddNewHighscore");
				Application.LoadLevel ("Gameplay");
			}
		}
	}

	public void OnFighterDestruction(){
		audio.PlayOneShot (explosionSFX);
		Instantiate (fighters[Random.Range(0, 5)], spawnPositions[Random.Range(0, 4)].position, Quaternion.identity);
	}
	public void OnPlayerDestruction(){
		audio.PlayOneShot (explosionSFX);
		canvas.GetComponent<Animator> ().SetBool ("isGameOver", true);
		isGameOver = true;
	}
}
