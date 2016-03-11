using UnityEngine;
using System.Collections;

public class TileGenerator : MonoBehaviour {

	void Start() {
		for (int i = 0; i < 1; i++) {
			for (int j = 0; j < 1; j++) {
				GameObject tile = new GameObject ();

				tile.name = "Tile (" + i + ", " + j + ")";
				tile.transform.parent = transform;

				TileShape tileShape = new TileShape (TileShape.Shape.hexagon);

				MeshTiling meshTiling = tile.AddComponent<MeshTiling> ();
				meshTiling.Initialize (tileShape, 100, 100);
			}
		}
	}

}
