using UnityEngine;

[RequireComponent(typeof(FlatMeshGeneratorV4_Working))]
public class ProceduralTerrainV4_Working : MonoBehaviour {

	[Range(1, 250)]
	public int tilesX;
	[Range(1, 250)]
	public int tilesZ;

	int prevTilesX;
	int prevTilesZ;

	[System.Serializable]
	public class Noise {
		
		[Range(0, 1000)]
		public int seed;
		int prevSeed;

		[Range(0, 8)]
		public int octave;
		[Range(2f, 250f)]
		public float frequency;
		[Range(0f, 250f)]
		public float amplitude;

		PerlinNoise perlin;

		public float GetNoise(float x, float z) {
			if (perlin == null || prevSeed != seed) {
				prevSeed = seed;
				perlin = new PerlinNoise (seed);
			}

			return perlin.FractalNoise (x, z, octave, frequency, amplitude);
		}
	}

	public Noise[] noiseArray;

	Mesh mesh;

	void OnValidate() {
		// Regenerate on every change
		RegenerateMesh ();
	}

	/*
	void Start() {
		RegenerateMesh ();
	}
	*/

	void RegenerateMesh() {
		if (mesh == null || GetComponent<MeshFilter> ().sharedMesh != mesh || prevTilesX != tilesX || prevTilesZ != tilesZ) {
			prevTilesX = tilesX;
			prevTilesZ = tilesZ;

			mesh = GetComponent<FlatMeshGeneratorV4_Working> ().GenerateMesh (mesh);
			GetComponent<MeshFilter> ().sharedMesh = mesh;
		}

		if (noiseArray.Length > 0) {
			Vector3[] curVerts = mesh.vertices;

			for (int z = 0; z < (tilesZ + 1); z++) {
				for (int x = 0; x < (tilesX + 1); x++) {
					Vector3 curVert = curVerts [z * (tilesX + 1) + x];

					float noiseSum = 0f;
					foreach (Noise noise in noiseArray) {
						if (noise != null) {
							noiseSum += noise.GetNoise (x, z);
						}
					}

					curVert.y = noiseSum;
					curVerts [z * (tilesX + 1) + x] = curVert;
				}
			}

			mesh.vertices = curVerts;
		}

		mesh.Optimize ();
		mesh.RecalculateNormals ();
	}
}