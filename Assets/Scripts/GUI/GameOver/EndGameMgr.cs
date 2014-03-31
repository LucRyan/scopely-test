using UnityEngine;
using System.Collections;

public class EndGameMgr : MonoBehaviour
{
	public UILabel creeperLbl;
	public UILabel zombieLbl;
	public UILabel gameOverLbl;

	
	private GameObject[] _panels;
	#region Unity Lifecycle
	// Use this for initialization
	void Awake()
	{
		NGUITools.SetActive(gameOverLbl.gameObject, false);
		_panels = GameObject.FindGameObjectsWithTag("Panel");
	
	}
	#endregion
	
	#region GameOver Helper
	public void GameOver()	
	{
		Time.timeScale = 0f;
		NGUITools.SetActive(gameOverLbl.gameObject, true);
		
		Screen.showCursor = true;
		Screen.lockCursor = false;
		
		//Update Score
		creeperLbl.text = ScoreMgr.CreeperText;
		zombieLbl.text = ScoreMgr.ZombieText;
		
		//Disable all GUI
		DisableAllGuiButSelf();
	}
	#endregion
	
	#region Internal Helper
	void DisableAllGuiButSelf()
	{
		foreach(GameObject go in _panels)
		{
			go.SetActive(false);
		}
		this.gameObject.SetActive(true);
	}
	
	void EnableAllGui()
	{
		foreach(GameObject go in _panels)
		{
			go.SetActive(true);
		}
	}
	#endregion
}


