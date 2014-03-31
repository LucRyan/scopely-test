using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
	
	public static World currentWorld;
	public int chunkWidth = 20, chunkHeight = 20, seed = 0;
	public float viewRange = 30;
	
	public float brickHeight = 1;
	
	public Chunk chunkFab;
	
	#region Unity Lifecycle
	// Use this for initialization
	void Awake () {
		Chunk.chunks = new List<Chunk>();
		Time.timeScale = 1;
		currentWorld = this;
		if (seed == 0)
			seed = Random.Range(0, int.MaxValue);
	}
	
	void Start()
	{
		StartCoroutine(CreateMesh());	
	}
	#endregion

	IEnumerator CreateMesh()
	{		
		for (float x = transform.position.x - viewRange; x < transform.position.x + viewRange * 5; x+= chunkWidth)
		{
			for (float z = transform.position.z - viewRange; z < transform.position.z + viewRange * 5 ; z+= chunkWidth)
			{
				
				Vector3 pos = new Vector3(x, 0, z);
				pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
				pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
				
				Chunk chunk = Chunk.FindChunk(pos);
				if (chunk != null) continue;
				
				chunk = (Chunk)Instantiate(chunkFab, pos, Quaternion.identity);	
			}
		}
		
		yield return null;
	}
}


