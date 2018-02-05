using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainGenerator {
    protected int width;

    public TerrainGenerator(int w) { width = w;}
    public virtual void generate() {}
}
