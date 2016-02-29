using UnityEngine;
using System.Collections;

public class TerrainTilesV1_Working : MonoBehaviour {

	public GameObject[,] GenerateTerrainTiles(GameObject parent, string tileNamePrefix, int numTilesX, int numTilesZ) {
		GameObject[,] terrainTiles = new GameObject[numTilesX, numTilesZ];

		for (int x = 0; x < numTilesX; x++) {
			for (int z = 0; z < numTilesZ; z++) {
				GameObject terrainTile = new GameObject ();
				terrainTile.name = tileNamePrefix + " (" + x + ", " + z + ")";
				terrainTile.transform.parent = parent.transform;

				terrainTiles [x, z] = terrainTile;
			}
		}

		return terrainTiles;
	}

}
