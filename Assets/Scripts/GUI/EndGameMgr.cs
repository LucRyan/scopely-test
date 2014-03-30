using UnityEngine;
using System.Collections;

public class EndGameMgr : MonoBehaviour
{
	public UILabel creeperLbl;
	public UILabel zombieLbl;
	public UILabel gameOverLbl;
	
	#region Unity Lifecycle
	// Use this for initialization
	void Awake()
	{
		
	}
	// Update is called once per frame
	void Update ()
	{

	}
	#endregion
	
	#region GameOver Helper
	public void GameOver()	
	{
		Time.timeScale = 0f;
		creeperLbl.text = ScoreMgr.CreeperText;
		zombieLbl.text = ScoreMgr.ZombieText;
	}
	#endregion
	
	#region Internal Helper

	#endregion
}


