using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonClicks : MonoBehaviour {


	public Canvas leaderBoardUserNameCanvas;
	public Text userName;



	public void loadScene(string sceneName){
		Application.LoadLevel (sceneName);
	}

	public void loadLeaderBoard(){
		if (PlayerPrefs.GetInt ("IS_USER_NAME_SET") == 0) {
			leaderBoardUserNameCanvas.enabled = true;
		} 
		else {
			Application.LoadLevel("LeaderBoard");
		}
	}

	public void setUserName(){
		if (string.IsNullOrEmpty (userName.text.ToString ()) == false) {
			PlayerPrefs.SetInt ("IS_USER_NAME_SET", 1);
			PlayerPrefs.SetString ("USER_NAME", userName.text);
			loadLeaderBoard ();
		} else
			cancelSettingUserName ();
	}

	public void cancelSettingUserName(){
		leaderBoardUserNameCanvas.enabled = false;
	}

	public void QuitGame(){
		Application.Quit ();
	}



}
