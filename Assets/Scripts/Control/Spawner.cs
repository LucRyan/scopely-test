using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
		
	public const int MAX_ENEMY = 30;
	public const float SPAWN_COOLDOWN = 2.0f;
	public const float SPAWN_RADIUS = 128.0f;
	public const float POSITION_CHANGE_COOLDOWN = 15f;
	public const float POSITION_CHANGE_RADIUS = 625;
	
	public const float CHECK_NUM_COOLDOWN = 2.0f;
	public const int DIFFICULTY_PROGRESSION_OFFSET = 15;//15;
	
	public GameObject creeper;
	public GameObject zombie;
	public AudioClip warningSFX;
	
	private GameObject _player;
	private float _cooldown = 0.0f;
	private float _checkNumCooldown = 0.0f;
	private float _positionChangeCooldown = 5f;
	private int _dpFactor = 1; // DIFFICULTY_PROGRESSION_Factor
	
	private WarningGUIMgr _warningGUIMgr;



	#region Unity Lifecycle
	void Start () {
		_cooldown = Random.value * 3.0f;
		_player = GameObject.Find("Player");
		_warningGUIMgr = (WarningGUIMgr)GameObject.FindObjectOfType(typeof(WarningGUIMgr));
	}

	void Update () {
		_positionChangeCooldown -=Time.deltaTime;
		_cooldown -= Time.deltaTime;
		_checkNumCooldown -= Time.deltaTime;
		
		if (_cooldown < 0.0f){
			_cooldown = SPAWN_COOLDOWN;
			if (EnemyCount() < MAX_ENEMY){
				DifficultyProgression();
			}
		}
		
		 ChangePosition();
	}
	#endregion

	#region Helpers
	void DifficultyProgression()
	{
		//Normal Spawn
		SpawnEnemy(creeper);
		if(_checkNumCooldown < 0.0f)
		{
			_checkNumCooldown = CHECK_NUM_COOLDOWN;
			string[] words = ScoreMgr.CreeperText.Split(' ');
			int numOfCreeperKilled = System.Convert.ToInt32(words[1]);	
//			Debug.Log(numOfCreeperKilled);
			if( numOfCreeperKilled > 10 + DIFFICULTY_PROGRESSION_OFFSET * _dpFactor)
			{
				Warning();
				//Spawn Boss
				for(int i = 0; i < _dpFactor; i++)
				{
					SpawnEnemy(zombie);
				}
				_dpFactor++;
			}
		}
	}
	
	void Warning()
	{
		_warningGUIMgr.Warning();
		AudioSource.PlayClipAtPoint(warningSFX, _player.transform.position);
	}
	
	void ChangePosition()
	{
		if(_positionChangeCooldown < 0.0f)
		{
			Debug.Log ("ChangePosition Start");
			float radius, rot, newX, newZ;
			do{
				radius = Mathf.Sqrt(Random.value * POSITION_CHANGE_RADIUS) + 10;
				rot = Random.value * Mathf.PI * 2.0f;
				newX = Mathf.Sin(rot) * radius + _player.transform.position.x;
				newZ = Mathf.Cos(rot) * radius + _player.transform.position.z;
			}
			while((newX < -40 || newX > 240) && (newZ < -40 || newZ > 240));

			
			this.transform.position = new Vector3(newX, 100f, newZ);
			_positionChangeCooldown = POSITION_CHANGE_COOLDOWN;
		}
		
	}
	
	private void SpawnEnemy(GameObject enemy){
		GameObject e = GameObject.Instantiate(enemy) as GameObject;
		float radius = Mathf.Sqrt(Random.value * SPAWN_RADIUS);
		float rot = Random.value * Mathf.PI * 2.0f;
		Vector3 spawnPos = new Vector3(Mathf.Sin(rot) * radius, Mathf.Cos(rot) * radius, 0.0f) + this.transform.position;
		e.transform.position = spawnPos;
	}
	public int EnemyCount(){
		Enemy[] enemies = (Enemy[])GameObject.FindObjectsOfType(typeof(Enemy));
		return enemies.Length;
	}
	#endregion
}
