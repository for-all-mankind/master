using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TerrainTilesV2_Working), typeof(MeshTilesV2_Working), typeof(TerrainNoiseV4_Working))]
public class TerrainManagerV1_Working : MonoBehaviour {

	[Header("Terrain Tiles")]
	public int terrainTileNumX = 1;
	public int terrainTileNumZ = 1;

	[Space(5f)]

	[Header("Mesh Tiles")]
	public int meshTileNumX = 1;
	public int meshTileNumZ = 1;

	public float meshTileSizeX = 1f;
	public float meshTileSizeZ = 1f;

	[Space(5f)]

	[Header("Terrain Noise")]
	public int seed = 0;
	public int octave = 8;
	public float frequency = 100f;
	public float amplitude = 50f;

	[Space(5f)]

	[Header("Texture")]
	public Material mat;

	TerrainTilesV2_Working terrainTiles;

	public GameObject[,] terrainTileArray;

	MeshTilesV2_Working meshTiles;
	TerrainNoiseV4_Working terrainNoise;

	void Start() {
		GenerateTerrain ();
	}

	public void GenerateTerrain() {
		if (!Application.isPlaying && terrainTileArray != null) {
			StopCoroutine (TerrainCoroutine ());

			foreach (GameObject terrainTile in terrainTileArray) {
				if (terrainTile != null) {
					DestroyImmediate (terrainTile);
				}
			}
		}

		terrainTiles = GetComponent<TerrainTilesV2_Working> ();

		InitializeMeshTiles ();
		InitializeTerrainNoise ();

		terrainTileArray = terrainTiles.GenerateTerrainTiles (gameObject, "Terrain", terrainTileNumX, terrainTileNumZ);

		StartCoroutine (TerrainCoroutine ());
	}

	void InitializeMeshTiles() {
		meshTiles = GetComponent<MeshTilesV2_Working> ();

		meshTiles.meshTileNumX = meshTileNumX;
		meshTiles.meshTileNumZ = meshTileNumZ;

		meshTiles.meshTileSizeX = meshTileSizeX;
		meshTiles.meshTileSizeZ = meshTileSizeZ;

		meshTiles.meshTileScaleNumX = terrainTileNumX;
		meshTiles.meshTileScaleNumZ = terrainTileNumZ;
	}

	void InitializeTerrainNoise() {
		terrainNoise = GetComponent<TerrainNoiseV4_Working> ();

		terrainNoise.seed = seed;
		terrainNoise.octave = octave;

		terrainNoise.frequency = frequency;
		terrainNoise.amplitude = amplitude;
	}

	IEnumerator TerrainCoroutine() {
		for (int x = 0; x < terrainTileNumX; x++) {
			for (int z = 0; z < terrainTileNumZ; z++) {
				GameObject terrainTile = terrainTileArray [x, z];

				meshTiles.GenerateMeshTiles (terrainTile, x, z);

				terrainTile.GetComponent<MeshRenderer> ().sharedMaterial = mat;
				terrainTile.transform.position = new Vector3 (meshTileNumX * meshTileSizeX * x, 0, meshTileNumZ * meshTileSizeZ * z);

				terrainNoise.GenerateTerrainNoise (terrainTile, meshTileNumX * meshTileSizeX * x, meshTileNumZ * meshTileSizeZ * z);

				if (Application.isPlaying) {
					yield return new WaitForSeconds (0.01f);
				}
			}
		}

		yield return null;
	}
}
