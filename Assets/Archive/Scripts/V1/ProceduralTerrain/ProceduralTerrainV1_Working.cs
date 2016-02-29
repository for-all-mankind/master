using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
[RequireComponent(typeof(FlatMeshGeneratorV4_Working))]
public class ProceduralTerrainV1_Working : MonoBehaviour {

	[Range(1, 250)]
	public int tilesX;
	[Range(1, 250)]
	public int tilesZ;

	int prevTilesX;
	int prevTilesZ;

	public int seed;

	[Range(0, 8)]
	public int octave;
	[Range(2f, 250f)]
	public float frequency;
	[Range(0f, 250f)]
	public float amplitude;

	Mesh mesh;

	void OnValidate() {
		RegenerateMesh ();
	}

	void Start() {
		prevTilesX = tilesX;
		prevTilesZ = tilesZ;

		RegenerateMesh ();
	}

	void RegenerateMesh() {
		if (prevTilesX != tilesX || prevTilesZ != tilesZ) {
			prevTilesX = tilesX;
			prevTilesZ = tilesZ;

			mesh = GetComponent<FlatMeshGeneratorV4_Working> ().GenerateMesh (mesh);
		}

		PerlinNoise noise = new PerlinNoise (seed);
		Vector3[] curVerts = mesh.vertices;

		for (int z = 0; z < (tilesZ + 1); z++) {
			for (int x = 0; x < (tilesX + 1); x++) {
				Vector3 curVert = curVerts [z * (tilesX + 1) + x];
				curVert.y = noise.FractalNoise (x, z, octave, frequency, amplitude);
				curVerts [z * (tilesX + 1) + x] = curVert;
			}
		}

		mesh.vertices = curVerts;

		mesh.Optimize ();
		mesh.RecalculateNormals ();
	}
}
