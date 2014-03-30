using UnityEngine;
using System.Collections;

public class ScoreMgr : MonoBehaviour {
	public enum EnemyType{Creeper, Zombie}; 
	public static string ZombieText
	{
		get{return _zombieLbl.text;}
	}
	public static string CreeperText
	{
		get{return _creeperLbl.text;}
	}
	
	private GameObject _scoreBoard;
	private static int _creeperCount;
	private static int _zombieCount;
	private static UILabel _creeperLbl;
	private static UILabel _zombieLbl;
		

	#region Unity Lifecycle
	// Use this for initialization
	void Start () {
		initialScore();
	}
	
	// Update is called once per frame
	void Update () {
		ToggleScoreBoard();
	}
	#endregion
	
	
	#region Score Helper
	private void initialScore()
	{
		_scoreBoard = this.transform.FindChild("BG").gameObject;
		NGUITools.SetActiveSelf(_scoreBoard, false);
		_creeperLbl = _scoreBoard.transform.FindChild("Creeper").transform.FindChild("Amount").gameObject.GetComponent<UILabel>();
		_zombieLbl = _scoreBoard.transform.FindChild("Zombie").transform.FindChild("Amount").gameObject.GetComponent<UILabel>();	
	}
	public static void UpdateKills(EnemyType eType, int count){
		if(eType == EnemyType.Creeper)
		{
			_creeperCount += count;
			if(_creeperCount < 10)
			{
				_creeperLbl.text = "X 0" + _creeperCount.ToString();
			}
			else
			{
				_creeperLbl.text = "X " + _creeperCount.ToString();
			}
			
		}
		else if(eType == EnemyType.Zombie)
		{
			_zombieCount += count;
			if(_zombieCount < 10)
			{
				_zombieLbl.text = "X 0" + _zombieCount.ToString();
			}
			else
			{
				_zombieLbl.text = "X " + _zombieCount.ToString();
			}
			
		}
	}
	
	private void ToggleScoreBoard()	
	{
		if (Input.GetKeyDown (KeyCode.Tab) ){
			NGUITools.SetActiveSelf(_scoreBoard, true);
		}
		if (Input.GetKeyUp (KeyCode.Tab) ){
			NGUITools.SetActiveSelf(_scoreBoard, false);
		}
	}
	#endregion
	
}
