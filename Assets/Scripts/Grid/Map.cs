using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Map
{
    private Grid<MapGridObject> grid;
    public Map()
    {
        grid = new Grid<MapGridObject>(10, 10, 1f, Vector3.zero, 
            (Grid<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y));
    }
}
