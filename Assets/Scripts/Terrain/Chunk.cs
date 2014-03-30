using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;

[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]
[RequireComponent (typeof(MeshFilter))]
public class Chunk : MonoBehaviour {
	
	public static List<Chunk> chunks = new List<Chunk>();
	public static int width {
		get { return World.currentWorld.chunkWidth; }
	}
	public static int height {
		get { return World.currentWorld.chunkHeight; }
	}
	public static float brickHeight {
		get { return World.currentWorld.brickHeight; }
	}
	
	public byte[,,] map;
	public Mesh visualMesh;
	protected MeshRenderer meshRenderer;
	protected MeshCollider meshCollider;
	protected MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		
		chunks.Add(this);
		
		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();
		meshFilter = GetComponent<MeshFilter>();
		
		
	
		CalculateMapFromScratch();
		StartCoroutine(CreateVisualMesh());
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static byte GetTheoreticalByte(Vector3 pos) {
		Random.seed = World.currentWorld.seed;
		
		Vector3 grain0Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
		Vector3 grain1Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
		Vector3 grain2Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
		
		return GetTheoreticalByte(pos, grain0Offset, grain1Offset, grain2Offset);
		
	}
	
	public static byte GetTheoreticalByte(Vector3 pos, Vector3 offset0, Vector3 offset1, Vector3 offset2)
	{
		float heightBase = 10;
		float maxHeight = height - 10;
		float heightSwing = maxHeight - heightBase;
		
		byte brick = 1;
					
		float clusterValue = CalculateNoiseValue(pos, offset1,  0.02f);
		float blobValue = CalculateNoiseValue(pos, offset1,  0.05f);
		float mountainValue = CalculateNoiseValue(pos, offset0,  0.009f);
		if ( (mountainValue == 0) && (blobValue < 0.2f) )
			brick = 3;
		else if (clusterValue > 0.8f)
			brick = 4;
		else if (clusterValue > 0.6f)
			brick = 2;
				
		mountainValue = Mathf.Sqrt(mountainValue);
		
		mountainValue *= heightSwing;
		mountainValue += heightBase;
					
		mountainValue += (blobValue * 10) - 5f;
					
					
					
		if (mountainValue >= pos.y)
			return brick;
		return 0;
	}
	
	public virtual void CalculateMapFromScratch() {
		map = new byte[width, height, width];
		
		Random.seed = World.currentWorld.seed;
		Vector3 grain0Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
		Vector3 grain1Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
		Vector3 grain2Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
		
		
		
		for (int x = 0; x < World.currentWorld.chunkWidth; x++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int z = 0; z < width; z++)
				{
					map[x, y, z] = GetTheoreticalByte(new Vector3(x, y, z) + transform.position, grain0Offset, grain1Offset, grain2Offset);
				
				}
			}
		}
		
	}
	
	public static float CalculateNoiseValue(Vector3 pos, Vector3 offset, float scale)
	{
		
		float noiseX = Mathf.Abs((pos.x + offset.x) * scale);
		float noiseY = Mathf.Abs((pos.y + offset.y) * scale);
		float noiseZ = Mathf.Abs((pos.z + offset.z) * scale);
		
		return Mathf.Max(0, Noise.Generate(noiseX, noiseY, noiseZ));
		
	}
	
	
	public virtual IEnumerator CreateVisualMesh() {
		visualMesh = new Mesh();
		
		List<Vector3> verts = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> tris = new List<int>();
		
		
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int z = 0; z < width; z++)
				{
					if (map[x,y,z] == 0) continue;
					
					byte brick = map[x,y,z];
					// Left wall
					if (IsTransparent(x - 1, y, z))
						BuildFace (brick, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
					// Right wall
					if (IsTransparent(x + 1, y , z))
						BuildFace (brick, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris);
					
					// Bottom wall
					if (IsTransparent(x, y - 1 , z))
						BuildFace (brick, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);
					// Top wall
					if (IsTransparent(x, y + 1, z))
						BuildFace (brick, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris);
					
					// Back
					if (IsTransparent(x, y, z - 1))
						BuildFace (brick, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris);
					// Front
					if (IsTransparent(x, y, z + 1))
						BuildFace (brick, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
					
					
				}
			}
		}
					
		visualMesh.vertices = verts.ToArray();
		visualMesh.uv = uvs.ToArray();
		visualMesh.triangles = tris.ToArray();
		visualMesh.RecalculateBounds();
		visualMesh.RecalculateNormals();
		
		meshFilter.mesh = visualMesh;
		
		
		meshCollider.sharedMesh = null;
		meshCollider.sharedMesh = visualMesh;
		
		yield return 0;
		
	}
	public virtual void BuildFace(byte brick, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
	{
		int index = verts.Count;
		
		
		float uvRow = ((corner.y + up.y) % 7);
		if (uvRow > 4) uvRow = 7 - uvRow;
		uvRow /= 4f;
		Vector2 uvCorner = new Vector2(0.00f, uvRow);
		
		
		corner.y *= brickHeight;
		up.y *= brickHeight;
		right.y *= brickHeight;
		
		
		verts.Add (corner);
		verts.Add (corner + up);
		verts.Add (corner + up + right);
		verts.Add (corner + right);
		
		Vector2 uvWidth = new Vector2(0.25f, 0.25f);
		
		uvCorner.x += (float)(brick - 1) / 4;
		
		uvs.Add(uvCorner);
		uvs.Add(new Vector2(uvCorner.x, uvCorner.y + uvWidth.y));
		uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));
		uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y));
		
		if (reversed)
		{
			tris.Add(index + 0);
			tris.Add(index + 1);
			tris.Add(index + 2);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 0);
		}
		else
		{
			tris.Add(index + 1);
			tris.Add(index + 0);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 2);
			tris.Add(index + 0);
		}
		
	}
	public virtual bool IsTransparent (int x, int y, int z)
	{
		if ( y < 0) return false;
		byte brick = GetByte(x,y,z);
		switch (brick)
		{
		case 0: 
			return true;
		default:
			return false;
		}
	}
	public virtual byte GetByte (int x, int y , int z)
	{
		
		if ((y < 0) || (y >= height))
			return 0;
		
		if ( (x < 0) || (z < 0)  || (x >= width) || (z >= width))
		{
			
			Vector3 worldPos = new Vector3(x, y, z) + transform.position;
			Chunk chunk = Chunk.FindChunk(worldPos);
			if (chunk == this) return 0;
			if (chunk == null) 
			{
				return GetTheoreticalByte(worldPos);
			}
			return chunk.GetByte (worldPos);
		}
		return map[x,y,z];
	}
	public virtual byte GetByte(Vector3 worldPos) {
		worldPos -= transform.position;
		int x = Mathf.FloorToInt(worldPos.x);
		int y = Mathf.FloorToInt(worldPos.y);
		int z = Mathf.FloorToInt(worldPos.z);
		return GetByte (x, y, z);
	}
	
	public static Chunk FindChunk(Vector3 pos) {
		
		for (int a = 0; a < chunks.Count; a++)
		{
			Vector3 cpos = chunks[a].transform.position;
			
			if ( ( pos.x < cpos.x) || (pos.z < cpos.z) || (pos.x >= cpos.x + width) || (pos.z >= cpos.z + width) ) continue;
			return chunks[a];
			
		}
		return null;
		
	}
	
	// Not used: too slow.
	public static void Tangentify(Mesh mesh)
	{
		int triangleCount = mesh.triangles.Length / 3;
		int vertexCount = mesh.vertices.Length;
		 
		Vector3[] tan1 = new Vector3[vertexCount];
		Vector3[] tan2 = new Vector3[vertexCount];
		 
		Vector4[] tangents = new Vector4[vertexCount];
		 
		for(long a = 0; a < triangleCount; a+=3)
		{
		long i1 = mesh.triangles[a+0];
		long i2 = mesh.triangles[a+1];
		long i3 = mesh.triangles[a+2];
		 
		Vector3 v1 = mesh.vertices[i1];
		Vector3 v2 = mesh.vertices[i2];
		Vector3 v3 = mesh.vertices[i3];
		 
		Vector2 w1 = mesh.uv[i1];
		Vector2 w2 = mesh.uv[i2];
		Vector2 w3 = mesh.uv[i3];
		 
		float x1 = v2.x - v1.x;
		float x2 = v3.x - v1.x;
		float y1 = v2.y - v1.y;
		float y2 = v3.y - v1.y;
		float z1 = v2.z - v1.z;
		float z2 = v3.z - v1.z;
		 
		float s1 = w2.x - w1.x;
		float s2 = w3.x - w1.x;
		float t1 = w2.y - w1.y;
		float t2 = w3.y - w1.y;
		 
		float r = 1.0f / (s1 * t2 - s2 * t1);
		 
		Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
		Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
		 
		tan1[i1] += sdir;
		tan1[i2] += sdir;
		tan1[i3] += sdir;
		 
		tan2[i1] += tdir;
		tan2[i2] += tdir;
		tan2[i3] += tdir;
		}
		 
		 
		for (long a = 0; a < vertexCount; ++a)
		{
		Vector3 n = mesh.normals[a];
		Vector3 t = tan1[a];
		 
		Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
		tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
		 
		tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
		}
		 
		mesh.tangents = tangents;
	}
	
}


