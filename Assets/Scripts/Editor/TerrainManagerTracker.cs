using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TerrainManagerV3_Working))]
public class TerrainManagerTracker : Editor {
	/*
	public override void OnInspectorGUI () {
		base.OnInspectorGUI();

		TerrainManager targ = (TerrainManager)target;

		if (GUI.changed) {
			targ.Start ();
		}
	}
	*/
}