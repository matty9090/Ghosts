using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainGenerator {
    protected int width, scene_w;
    protected float ratio;
    protected float y_offset;

    protected Vector3[] terrainCoords;
    protected Vector2[] terrainCoords2D;

    public TerrainGenerator(int w, int sw, float off) {
        width       = w;
        scene_w     = sw;
        y_offset    = off;
        ratio       = (float)sw / (float)w;

        terrainCoords   = new Vector3[width];
        terrainCoords2D = new Vector2[width];
    }

    public virtual void generate() {}

    public virtual Vector3[] terrainData { get { return terrainCoords; } }
    public virtual Vector2[] terrainData2D { get { return terrainCoords2D; } }
}
