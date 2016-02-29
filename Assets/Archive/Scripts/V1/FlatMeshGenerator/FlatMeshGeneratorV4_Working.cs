using UnityEngine;

public class FlatMeshGeneratorV4_Working : MonoBehaviour {

	[Range(1, 250)]
	public int numTilesX;
	[Range(1, 250)]
	public int numTilesZ;

	[HideInInspector]
	public bool forceChange = false;

	int prevTilesX;
	int prevTilesZ;

	public delegate void callback();
	public callback callbackFunc;
	void OnValidate() {
		if (callbackFunc != null) {
			callbackFunc ();
		}
	}

	public Mesh GenerateMesh() {
		return GenerateMesh (null);
	}
	
	public Mesh GenerateMesh(Mesh mesh) {
		if (mesh == null) {
			mesh = new Mesh ();
			mesh.name = "Procedural Mesh";
		} else if (!forceChange && prevTilesX == numTilesX && prevTilesZ == numTilesZ) {
			return mesh;
		}

		if (forceChange) {
			prevTilesX = numTilesX;
			prevTilesZ = numTilesZ;
		}

		int numTiles = numTilesX * numTilesZ;

		int trisPerTile = 2;
		int vertsPerTriangle = 3;
		int triVertsPerTile = trisPerTile * vertsPerTriangle;

		int numVertsX = numTilesX + 1;
		int numVertsZ = numTilesZ + 1;
		int numVerts = numVertsX * numVertsZ;

		Vector3[] verts = new Vector3[numVerts];
		Vector2[] uvs = new Vector2[numVerts];
		int[] tris = new int[numTiles * triVertsPerTile];

		for (int z = 0; z < numVertsZ; z++) {
			for (int x = 0; x < numVertsX; x++) {
				int tileIndex = z * numTilesX + x;
				int triIndex = triVertsPerTile * tileIndex;

				int vertIndex = z * numVertsX + x;

				int bottom = z * numVertsX;
				int top = (z + 1) * numVertsX;

				int left = x;
				int right = x + 1;

				verts [vertIndex] = new Vector3 (x, 0, z);
				uvs [vertIndex] = new Vector2 ((float)x / numTilesX, (float)z / numTilesZ);

				if ((x < numTilesX) && (z < numTilesZ)) {
					tris [triIndex + 0] = bottom + left;
					tris [triIndex + 1] =    top + left;
					tris [triIndex + 2] =    top + right;

					tris [triIndex + 3] = bottom + left;
					tris [triIndex + 4] =    top + right;
					tris [triIndex + 5] = bottom + right;
				}
			}
		}

		mesh.Clear ();

		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.triangles = tris;

		mesh.Optimize ();
		mesh.RecalculateNormals ();

		return mesh;
	}
}