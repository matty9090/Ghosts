using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLoader : MonoBehaviour {
	void Start () {
        MidPointDisplacement t = new MidPointDisplacement(9);

        GetComponent<LineRenderer>().positionCount = t.terrainData.Length;
        GetComponent<LineRenderer>().SetPositions(t.terrainData);
	}
}
