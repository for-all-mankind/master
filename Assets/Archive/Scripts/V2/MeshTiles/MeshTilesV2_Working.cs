using UnityEngine;
using System.Collections;

public class MeshTilesV2_Working : MonoBehaviour {

	static int trisPerTile = 2;
	static int vertsPerTriangle = 3;
	static int triVertsPerTile = trisPerTile * vertsPerTriangle;

	[HideInInspector]
	public int meshTileNumX;
	[HideInInspector]
	public int meshTileNumZ;

	[HideInInspector]
	public float meshTileSizeX;
	[HideInInspector]
	public float meshTileSizeZ;

	[HideInInspector]
	public int meshTileScaleNumX;
	[HideInInspector]
	public int meshTileScaleNumZ;

	public void GenerateMeshTiles(GameObject terrainTile, int meshTileOffsetNumX, int meshTileOffsetNumZ) {
		int numTiles = meshTileNumX * meshTileNumZ;

		int numVertsX = meshTileNumX + 1;
		int numVertsZ = meshTileNumZ + 1;
		int numVerts = numVertsX * numVertsZ;

		Vector3[] verts = new Vector3[numVerts];
		Vector2[] uvs = new Vector2[numVerts];
		int[] tris = new int[numTiles * triVertsPerTile];

		for (int x = 0; x < numVertsX; x++) {
			for (int z = 0; z < numVertsZ; z++) {
				int tileIndex = z * meshTileNumX + x;
				int triIndex = triVertsPerTile * tileIndex;

				int vertIndex = z * numVertsX + x;

				int bottom = z * numVertsX;
				int top = (z + 1) * numVertsX;

				int left = x;
				int right = x + 1;

				verts [vertIndex] = new Vector3 (x * meshTileSizeX, 0, z * meshTileSizeZ);

				float uvPosX = (float)meshTileOffsetNumX / meshTileScaleNumX + ((float)x / meshTileNumX) / meshTileScaleNumX;
				float uvPosZ = (float)meshTileOffsetNumZ / meshTileScaleNumZ + ((float)z / meshTileNumZ) / meshTileScaleNumZ;

				uvs [vertIndex] = new Vector2 (uvPosX, uvPosZ);

				if ((x < meshTileNumX) && (z < meshTileNumZ)) {
					tris [triIndex + 0] = bottom + left;
					tris [triIndex + 1] = top + left;
					tris [triIndex + 2] = top + right;

					tris [triIndex + 3] = bottom + left;
					tris [triIndex + 4] = top + right;
					tris [triIndex + 5] = bottom + right;
				}
			}
		}
		
		if (terrainTile.GetComponent<MeshFilter> () == null) {
			terrainTile.AddComponent<MeshFilter> ();
		}

		if (terrainTile.GetComponent<MeshRenderer> () == null) {
			terrainTile.AddComponent<MeshRenderer> ();
		}

		Mesh mesh = terrainTile.GetComponent<MeshFilter> ().sharedMesh;

		if (mesh == null) {
			mesh = new Mesh ();
		}

		if (mesh.name == "") {
			mesh.name = "Procedural Mesh";
		}

		mesh.Clear ();

		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.triangles = tris;

		mesh.Optimize ();
		mesh.RecalculateNormals ();

		terrainTile.GetComponent<MeshFilter> ().sharedMesh = mesh;
	}

}
