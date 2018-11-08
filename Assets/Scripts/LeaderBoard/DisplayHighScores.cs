using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayHighScores : MonoBehaviour {
	public Text[] highscoreFields;
	WorldWideHighscores highscoresManager;

	public Text yourBestScoreUI;
	public Text yourWorldRankUI;

	private int yourBest;
	private int yourRank = 0;
	private string yourName;
	
	void Start() {
		yourBest = PlayerPrefs.GetInt ("USER_BEST_SCORE");
		yourName = PlayerPrefs.GetString ("USER_NAME");

		//Debug.Log ("BEST : " + yourBest.ToString());
		for (int i = 0; i < highscoreFields.Length; i ++) {
			highscoreFields[i].text = i+1 + ". Fetching...";
		}
		
		
		highscoresManager = GetComponent<WorldWideHighscores>();
		StartCoroutine("RefreshHighscores");
	}
	
	public void OnHighscoresDownloaded(Highscore[] highscoreList) {
		yourRank = 0;
		for (int i =0; i < highscoreFields.Length; i ++) {
			highscoreFields[i].text = i+1 + ". ";
			if (i < highscoreList.Length) {
				highscoreFields[i].text += highscoreList[i].username + " - " + highscoreList[i].score;
			}
		}

		for (int i =0; i < highscoreList.Length; i ++) {
			//Debug.Log(i + "--> "+highscoreList[i].score);
			yourRank++;
			if(yourBest == highscoreList[i].score)break;

		}

		yourBestScoreUI.text = "Your Best Score: "+ yourBest.ToString();
		yourWorldRankUI.text = "Your World Rank: " + yourRank.ToString ();

	}
	
	IEnumerator RefreshHighscores() {
		while (true) {
			highscoresManager.DownloadHighscores();
			yield return new WaitForSeconds(30);
		}
	}
}
