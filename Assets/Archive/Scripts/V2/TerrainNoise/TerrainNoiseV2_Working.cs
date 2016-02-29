using UnityEngine;

[RequireComponent(typeof(FlatMeshGeneratorV4_Working))]
public class TerrainNoiseV2_Working : MonoBehaviour {

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

	public delegate void callback();
	public callback callbackFunc;
	void OnValidate() {
		if (callbackFunc != null) {
			callbackFunc ();
		}
	}

	public Mesh RegenerateMesh(Mesh mesh, float offsetX, float offsetZ) {
		mesh = GetComponent<FlatMeshGeneratorV4_Working> ().GenerateMesh (mesh);

		if (noiseArray.Length > 0) {
			Vector3[] curVerts = mesh.vertices;

			for (int i = 0; i < curVerts.Length; i++) {
				Vector3 curVert = curVerts [i];

				float noiseSum = 0f;
				foreach (Noise noise in noiseArray) {
					if (noise != null) {
						noiseSum += noise.GetNoise (curVert.x + offsetX, curVert.z + offsetZ);
					}
				}

				curVert.y = noiseSum;
				curVerts [i] = curVert;
			}

			mesh.vertices = curVerts;
		}

		mesh.Optimize ();
		mesh.RecalculateNormals ();

		return mesh;
	}
}