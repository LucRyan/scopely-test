using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public const int MAX_ZUNNIES = 30;
	public const float SPAWN_COOLDOWN = 1.0f;
	public const float SPAWN_RADIUS = 128.0f;

	private float _cooldown = 0.0f;

	public GameObject enemy;

	#region Unity Lifecycle
	void Start () {
		_cooldown = Random.value * 3.0f;
	}

	void Update () {
		_cooldown -= Time.deltaTime;
		if (_cooldown < 0.0f){
			_cooldown = SPAWN_COOLDOWN;
			if (EnemyCount() < MAX_ZUNNIES){
				SpawnEnemy();
			}
		}
	}
	#endregion

	#region Helpers
	private void SpawnEnemy(){
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
