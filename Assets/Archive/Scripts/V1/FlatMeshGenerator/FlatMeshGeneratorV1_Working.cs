using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class FlatMeshGeneratorV1_Working : MonoBehaviour {

	void Start() {
		int numTilesX = 100;
		int numTilesZ = 100;

		Mesh mesh = GetComponent<MeshFilter> ().sharedMesh;
		if (!mesh) {
			mesh = new Mesh ();
			GetComponent<MeshFilter> ().sharedMesh = mesh;
		}

		Vector3[] verts = new Vector3[(numTilesX + 1) * (numTilesZ + 1)];
		Vector2[] uvs = new Vector2[(numTilesX + 1) * (numTilesZ + 1)];
		int[] tris = new int[(numTilesX * numTilesZ) * (2 * 3)];

		for (int z = 0; z < (numTilesZ + 1); z++) {
			for (int x = 0; x < (numTilesX + 1); x++) {
				verts [z * (numTilesX + 1) + x] = new Vector3 (x, 0, z);
				uvs [z * (numTilesX + 1) + x] = new Vector2 ((float)x / numTilesX, (float)z / numTilesZ);

				if ((x < numTilesX) && (z < numTilesZ)) {
					tris[6 * (z * numTilesX + x) + 0] = z * (numTilesX + 1) + x;
					tris[6 * (z * numTilesX + x) + 1] = (z + 1) * (numTilesX + 1) + x;
					tris[6 * (z * numTilesX + x) + 2] = (z + 1) * (numTilesX + 1) + x + 1;

					tris[6 * (z * numTilesX + x) + 3] = z * (numTilesX + 1) + x;
					tris[6 * (z * numTilesX + x) + 4] = (z + 1) * (numTilesX + 1) + x + 1;
					tris[6 * (z * numTilesX + x) + 5] = z * (numTilesX + 1) + x + 1;
				}
			}
		}

		mesh.Clear ();

		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.triangles = tris;

		mesh.Optimize ();
		mesh.RecalculateNormals ();
	}
}
