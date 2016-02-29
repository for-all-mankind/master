using UnityEngine;
using System.Collections;

public class TerrainManager : MonoBehaviour {

	public class TerrainTileStructure {
		public int tileNumX = 1;
		public int tileNumZ = 1;

		public int tileSizeX {
			get {
				Debug.Log ("Accessing terrain tile size x");
				return tileSizeX;
			}

			set {
				tileSizeX = 0;
			}
		}
	}

	void Start() {
		TerrainTileStructure terrain = new TerrainTileStructure ();
		Debug.Log (terrain.tileSizeX);
	}

}
