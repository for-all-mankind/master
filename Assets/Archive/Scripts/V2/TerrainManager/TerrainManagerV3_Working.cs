using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TerrainTilesV2_Working), typeof(MeshTilesV2_Working), typeof(TerrainNoiseV4_Working))]
public class TerrainManagerV3_Working : MonoBehaviour {

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
	public Texture heightMap;

	TerrainTilesV2_Working terrainTiles;

	public GameObject[,] terrainTileArray;

	MeshTilesV2_Working meshTiles;
	TerrainNoiseV4_Working terrainNoise;

	public void Start() {
		StartCoroutine (GenerateTerrain ());
	}
		
	IEnumerator GenerateTerrain() {
		StopAllCoroutines ();

		if (transform.childCount > 0) {
			if (Application.isPlaying) {
				for (int i = 0; transform.childCount > 0; i++) {
					Destroy (transform.GetChild (0).gameObject);
				}
			} else {
				for (int i = 0; transform.childCount > 0; i++) {
					DestroyImmediate (transform.GetChild (0).gameObject);
				}
			}
		}

		terrainTiles = GetComponent<TerrainTilesV2_Working> ();

		InitializeMeshTiles ();
		InitializeTerrainNoise ();

		terrainTileArray = terrainTiles.GenerateTerrainTiles (gameObject, "Terrain", terrainTileNumX, terrainTileNumZ);

		yield return StartCoroutine (TerrainCoroutine ());
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

				//terrainNoise.GenerateTerrainNoise (terrainTile, meshTileNumX * meshTileSizeX * x, meshTileNumZ * meshTileSizeZ * z);

				ApplyHeightmap ();

				yield return null;
			}
		}

		yield return null;
	}

	void ApplyHeightmap() {

	}

	/*
	void AddMountain(GameObject terrainTile, float offsetX, float offsetZ) {
		PerlinNoise noise = new PerlinNoise (seed);

		Mesh mesh = terrainTile.GetComponent<MeshFilter> ().sharedMesh;

		Vector3[] curVerts = mesh.vertices;

		for (int i = 0; i < curVerts.Length; i++) {
			Vector3 curVert = curVerts [i];

			curVert.y += Mathf.Max (0, noise.FractalNoise (curVert.x + offsetX, curVert.z + offsetZ, 8, 100f, 40f));
			curVerts [i] = curVert;
		}

		mesh.vertices = curVerts;

		//mesh.Optimize ();
		//mesh.RecalculateNormals ();

		terrainTile.GetComponent<MeshFilter> ().sharedMesh = mesh;

		int heightMapOffsetX = 10;
		int heightMapOffsetZ = 10;
		int heightMapSizeX = 10;
		int heightMapSizeZ = 10;

		PerlinNoise noise = new PerlinNoise (123);

		GameObject terrainTile = terrainTileArray [0, 0];
		int terrainTileIndexX = 0;
		int terrainTileIndexZ = 0;

		Vector3[] curVerts = terrainTile.GetComponent<MeshFilter> ().sharedMesh.vertices;

		int x = heightMapOffsetX, z = heightMapOffsetZ;
		for (int zIndex = heightMapOffsetZ; zIndex < heightMapOffsetZ + heightMapSizeZ; zIndex++, z++) {
			if (z > meshTileNumZ) {
				z = 0;
				zIndex--;
				terrainTileIndexZ++;
				terrainTile = terrainTileArray [terrainTileIndexX, terrainTileIndexZ];
				curVerts = terrainTile.GetComponent<MeshFilter> ().sharedMesh.vertices;
			}

			for (int xIndex = heightMapOffsetX; xIndex < heightMapOffsetX + heightMapSizeX; xIndex++, x++) {
				if (x > meshTileNumX) {
					x = 0;
					xIndex--;
					terrainTileIndexX++;
					terrainTile = terrainTileArray [terrainTileIndexX, terrainTileIndexZ];
					curVerts = terrainTile.GetComponent<MeshFilter> ().sharedMesh.vertices;
				}

				int tileIndex = z * (meshTileNumX + 1) + x;

				Vector3 curVert = curVerts [tileIndex];

				float ampX = heightMapSizeX / 2f - Mathf.Abs(xIndex - heightMapOffsetX - heightMapSizeX / 2f);
				float ampZ = heightMapSizeZ / 2f - Mathf.Abs(zIndex - heightMapOffsetZ - heightMapSizeZ / 2f);

				float amp = ampX * ampZ / (heightMapSizeX * heightMapSizeZ / ((heightMapSizeX + heightMapSizeZ) * 25f)) + Random.Range(-heightMapSizeX * heightMapSizeZ / 1000f, heightMapSizeX * heightMapSizeZ / 1000f);

				curVert.y = curVert.y + Mathf.Abs (noise.FractalNoise (xIndex, zIndex, 4, heightMapSizeX * heightMapSizeZ / 10f, amp)) + noise.FractalNoise (xIndex, zIndex, 8, 10f, 50f);

				curVerts [tileIndex] = curVert;

				terrainTile.GetComponent<MeshFilter> ().sharedMesh.vertices = curVerts;
			}

			x = heightMapOffsetX;
			terrainTileIndexX = 0;
			terrainTile = terrainTileArray [terrainTileIndexX, terrainTileIndexZ];
			curVerts = terrainTile.GetComponent<MeshFilter> ().sharedMesh.vertices;

			yield return null;
		}

		terrainTile.GetComponent<MeshFilter> ().sharedMesh.vertices = curVerts;
		

		yield return null;
	}
	*/
}
