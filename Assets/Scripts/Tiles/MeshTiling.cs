using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileShape {
	
	//== Syntax sugar ==//
	static float sqrt3 = Mathf.Sqrt(3.0f);
	static int numVertsPerTriangle = 3;

	//== Tile shape ==//
	public enum Shape {
		hexagon
	}

	Shape shape;

	//== Shape properties ==//
	public int numEdges;
	public int numTriangles; // Number of vertices that make up all triangles of the tile
	public int numRealTriangles;

	// The distance between each tile, their scale
	float xScale;
	float zScale;

	//== Mesh info ==//
	List<Vector3> tileVertexList;
	List<int> tileTriangleList;

	public TileShape(Shape shape) {
		this.shape = shape;

		if (shape == Shape.hexagon) {
			numEdges = 6;
			numRealTriangles = numEdges - 2;

			numTriangles = numRealTriangles * numVertsPerTriangle;

			xScale = 1.5f;
			zScale = sqrt3;

			tileVertexList = new List<Vector3> (numEdges) {
				// Bottom
				new Vector3 (1f / 2f, 0, 0         ), // Left
				new Vector3 (3f / 2f, 0, 0         ), // Right

				// Middle
				new Vector3 (      0, 0, sqrt3 / 2f), // Left
				new Vector3 (      2, 0, sqrt3 / 2f), // Right

				// Top
				new Vector3 (1f / 2f, 0, sqrt3     ), // Left
				new Vector3 (3f / 2f, 0, sqrt3     ), // Right
			};

			tileTriangleList = new List<int> (numTriangles) {
				0, 2, 1, // Bottom left
				3, 1, 2, // Bottom right
				2, 4, 3, // Top left
				5, 3, 4  // Top right
			};
		}
	}

	public void AddTileMeshInfo(List<Vector3> vertexList, List<int> triangleList, int tileIndex, int column, int row) {
		for (int i = 0; i < numEdges; i++) {
			int vertexIndex = tileIndex * numEdges + i;
			
			if (shape == Shape.hexagon) {
				float zOffset = 0;

				if (column % 2 != 0) {
					// Offset every second column so hexagons align
					zOffset = sqrt3 / 2f;
				}

				Vector3 vectorOffset = new Vector3 (column * xScale, 0, zOffset + row * zScale);

				vertexList.Insert (vertexIndex, tileVertexList [i] + vectorOffset);
			}

		}

		for (int i = 0; i < numTriangles; i++) {
			int triangleIndex = tileIndex * numTriangles + i;
			int triangleOffset = tileIndex * numEdges;

			triangleList.Insert (triangleIndex, tileTriangleList [i] + triangleOffset);
		}
	}
}

public class MeshTiling : MonoBehaviour {

	TileShape tileShape;

	//== Mesh Tiles ==//
	// (Vertices grouped together into tile)
	int tileNumX; // Number of tile 'columns' in mesh (along X axis)
	int tileNumZ; // Number of tile 'rows' in mesh (along Z axis)

	public void Initialize(TileShape tileShape) {
		Initialize (tileShape, 1, 1);
	}

	public void Initialize(TileShape tileShape, int tileNumX, int tileNumZ) {
		this.tileShape = tileShape;
		this.tileNumX = tileNumX;
		this.tileNumZ = tileNumZ;

		// Total number of tiles contained in mesh
		int tileNum = tileNumX * tileNumZ; 

		int vertNum = tileNum * tileShape.numEdges; // Vertices
		int triNum = tileNum * tileShape.numTriangles; // Triangles

		// Mesh info
		List<Vector3> vertices = new List<Vector3> (vertNum);
		List<int> triangles = new List<int> (triNum);

		for (int i = 0; i < tileNumZ; i++) {
			for (int j = 0; j < tileNumX; j++) {
				int tileIndex = i * tileNumX + j;

				// Add vertex and triangle info
				tileShape.AddTileMeshInfo (vertices, triangles, tileIndex, i, j);
			}
		}

		Mesh tileMesh = new Mesh();
		tileMesh.name = "Tile Mesh";

		tileMesh.vertices = vertices.ToArray ();
		tileMesh.triangles = triangles.ToArray ();

		tileMesh.RecalculateNormals ();
		tileMesh.Optimize ();

		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer> ();
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter> ();

		meshFilter.sharedMesh = tileMesh;
	}

}
