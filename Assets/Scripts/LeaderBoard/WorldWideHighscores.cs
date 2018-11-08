using UnityEngine;
using System.Collections;

public class WorldWideHighscores : MonoBehaviour {
	const string privateCode = "RGj3f1u6hkGQ_8kaVo57vgXdNmUx6MS0ydqdOPkKBkmg";
	const string publicCode = "579b4c068af6030548bc205b";
	const string webURL = "http://dreamlo.com/lb/";

	DisplayHighScores highscoreDisplay;
	public Highscore[] highscoresList;
	static WorldWideHighscores instance;
	
	void Awake() {
		highscoreDisplay = GetComponent<DisplayHighScores> ();
		//instance = this;

		/*AddNewHighscore("Ashik", 50);
		AddNewHighscore();*/

		AddNewHighscore(PlayerPrefs.GetString("USER_NAME"), PlayerPrefs.GetInt("USER_BEST_SCORE"));
	}
	
	public void AddNewHighscore(string username, int score) {
		StartCoroutine(UploadNewHighscore(username,score));
	}
	
	IEnumerator UploadNewHighscore(string username, int score) {
		WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
		yield return www;
		
		if (string.IsNullOrEmpty(www.error)) {
			print ("Upload Successful");
			DownloadHighscores();
		}
		else {
			print ("Error uploading: " + www.error);
		}
	}
	
	public void DownloadHighscores() {
		StartCoroutine("DownloadHighscoresFromDatabase");
	}
	
	IEnumerator DownloadHighscoresFromDatabase() {
		WWW www = new WWW(webURL + publicCode + "/pipe/");
		yield return www;
		
		if (string.IsNullOrEmpty (www.error)) {
			FormatHighscores (www.text);
			highscoreDisplay.OnHighscoresDownloaded(highscoresList);
		}
		else {
			print ("Error Downloading: " + www.error);
		}
	}
	
	void FormatHighscores(string textStream) {
		string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		highscoresList = new Highscore[entries.Length];
		
		for (int i = 0; i <entries.Length; i ++) {
			string[] entryInfo = entries[i].Split(new char[] {'|'});
			string username = entryInfo[0];
			int score = int.Parse(entryInfo[1]);
			highscoresList[i] = new Highscore(username,score);
//			print (highscoresList[i].username + ": " + highscoresList[i].score);
		}
	}
	
}

public struct Highscore {
	public string username;
	public int score;
	
	public Highscore(string _username, int _score) {
		username = _username;
		score = _score;
	}


}
