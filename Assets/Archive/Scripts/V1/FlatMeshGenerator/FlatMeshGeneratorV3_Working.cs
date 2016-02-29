using UnityEngine;

public class FlatMeshGeneratorV3_Working : MonoBehaviour {

	public Mesh GenerateMesh(int numTilesX, int numTilesZ) {
		Mesh mesh = new Mesh ();
		mesh.name = "Procedural Mesh";

		return GenerateMesh (numTilesX, numTilesZ, mesh);
	}
	
	public Mesh GenerateMesh(int numTilesX, int numTilesZ, Mesh mesh) {
		if (mesh == null) {
			return GenerateMesh (numTilesX, numTilesZ);
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