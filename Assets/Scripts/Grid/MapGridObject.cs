using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Present on all Grid positions
public class MapGridObject
{
    private Grid<MapGridObject> grid;
    private int x;
    private int y;

    public MapGridObject(Grid<MapGridObject> _grid, int _x, int _y)
    {
        grid = _grid;
        x = _x;
        y = _y;
    }
}
