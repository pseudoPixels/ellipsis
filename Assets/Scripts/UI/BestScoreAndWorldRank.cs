using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BestScoreAndWorldRank : MonoBehaviour {

	public Text yourBestUI;
	public Text yourWorldRankUI;

	const string privateCode = "RGj3f1u6hkGQ_8kaVo57vgXdNmUx6MS0ydqdOPkKBkmg";
	const string publicCode = "579b4c068af6030548bc205b";
	const string webURL = "http://dreamlo.com/lb/";
	

	public Highscore[] highscoresList;
	private int yourBest;


	// Use this for initialization
	void Start () {
		yourBest = PlayerPrefs.GetInt ("USER_BEST_SCORE");
		yourBestUI.text =  "YOUR BEST: " + yourBest;

		StartCoroutine("DownloadHighscoresFromDatabase");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator DownloadHighscoresFromDatabase() {
		WWW www = new WWW(webURL + publicCode + "/pipe/");
		yield return www;
		
		if (string.IsNullOrEmpty (www.error)) {
			FormatHighscores (www.text);
			int rank = 0;
			for (int i =0; i < highscoresList.Length; i ++) {
				//Debug.Log(i + "--> "+highscoreList[i].score);
				rank++;
				if(yourBest == highscoresList[i].score)break;
				
			}
			yourWorldRankUI.text = "WORLD RANK: "+ rank; 

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
