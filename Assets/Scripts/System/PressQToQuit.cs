using UnityEngine;
using System.Collections;

public class PressQToQuit : MonoBehaviour {
	
	bool isPaused = false;
	float savedTimeScale;
	
	void Update () {

		if (Input.GetKey (KeyCode.Q)) {
			Application.Quit();
		}
		
		if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePauseGame();
		}
		
	}
	
		
	public void TogglePauseGame()
	{
		if(!isPaused)
		{
			savedTimeScale = Time.timeScale;
			Time.timeScale = 0.0f;	
			AudioListener.pause = true;
			
			Screen.showCursor = true;
			isPaused = true;
		
		}
		else
		{
			Time.timeScale = savedTimeScale;
    		AudioListener.pause = false;
			Screen.showCursor = false;
			isPaused = false;
		}
	
	
	}	
}
