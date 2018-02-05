using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainGenerator {
    protected int width;
    protected Vector3[] terrainCoords;

    public TerrainGenerator(int w) {
        width = w;
        terrainCoords = new Vector3[width];
    }

    public virtual void generate() {}
    public virtual Vector3[] terrainData { get { return terrainCoords; } }
}
