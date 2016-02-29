using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlatMeshGeneratorV4_Working), typeof(TerrainNoiseV2_Working))]
public class TerrainTilingGeneratorV1_Working : MonoBehaviour {

	public Material mat;
	public int numTilesX;
	public int numTilesZ;

	List<GameObject> terrainTiles;
	//Mesh mesh;

	void OnChangeMessage() {
		for (int i = 0; i < numTilesX; i++) {
			for (int j = 0; j < numTilesZ; j++) {
				GameObject terrainTile = terrainTiles [i * numTilesZ + j];

				if (i == numTilesX - 1 && j == numTilesZ - 1) {
					GetComponent<FlatMeshGeneratorV4_Working> ().forceChange = true;
				}

				Mesh terrainTileMesh = terrainTile.GetComponent<MeshFilter> ().sharedMesh;
				terrainTileMesh = GetComponent<TerrainNoiseV2_Working> ().RegenerateMesh (terrainTileMesh, i * 250f, j * 250f);
				terrainTile.GetComponent<MeshFilter> ().sharedMesh = terrainTileMesh;

				GetComponent<FlatMeshGeneratorV4_Working> ().forceChange = false;
			}
		}
	}

	void OnValidate() {
		if (GetComponent<FlatMeshGeneratorV4_Working> ().callbackFunc == null ||
			GetComponent<TerrainNoiseV2_Working> ().callbackFunc == null) {

			GetComponent<FlatMeshGeneratorV4_Working> ().callbackFunc = OnChangeMessage;
			GetComponent<TerrainNoiseV2_Working> ().callbackFunc = OnChangeMessage;
		}

		if (terrainTiles == null) {
			terrainTiles = new List<GameObject> (numTilesX * numTilesZ);

			for (int i = 0; i < numTilesX; i++) {
				for (int j = 0; j < numTilesZ; j++) {
					GameObject terrainTile = new GameObject ();

					terrainTiles.Add (terrainTile);

					terrainTile.name = "Terrain (" + i + ", " + j + ")";
					terrainTile.transform.parent = transform;

					terrainTile.AddComponent<MeshFilter> ();
					terrainTile.AddComponent<MeshRenderer> ();

					Mesh terrainTileMesh = terrainTile.GetComponent<MeshFilter> ().sharedMesh;
					terrainTileMesh = GetComponent<TerrainNoiseV2_Working> ().RegenerateMesh (terrainTileMesh, i * 250f, j * 250f);
					terrainTile.GetComponent<MeshFilter> ().sharedMesh = terrainTileMesh;

					terrainTile.GetComponent<MeshRenderer> ().sharedMaterial = mat;

					terrainTile.transform.position = new Vector3 (i * 250f, 0, j * 250f);
				}
			}
		}
	}
}
