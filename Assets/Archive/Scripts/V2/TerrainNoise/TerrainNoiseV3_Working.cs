using UnityEngine;
using System.Collections;

public class TerrainNoiseV3_Working : MonoBehaviour {

	[HideInInspector]
	public int seed;
	[HideInInspector]
	public int octave;
	[HideInInspector]
	public float frequency;
	[HideInInspector]
	public float amplitude;

	PerlinNoise noise;

	public void GenerateTerrainNoise(GameObject terrainTile, float offsetX, float offsetZ) {
		noise = new PerlinNoise (seed);

		Mesh mesh = terrainTile.GetComponent<MeshFilter> ().sharedMesh;

		Vector3[] curVerts = mesh.vertices;

		for (int i = 0; i < curVerts.Length; i++) {
			Vector3 curVert = curVerts [i];

			curVert.y = noise.FractalNoise (curVert.x + offsetX, curVert.z + offsetZ, octave, frequency, amplitude);
			curVerts [i] = curVert;
		}

		mesh.vertices = curVerts;

		mesh.Optimize ();
		mesh.RecalculateNormals ();

		terrainTile.GetComponent<MeshFilter> ().sharedMesh = mesh;
	}

}
