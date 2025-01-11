using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Grid<TGridObject>
{
    #region Events
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    #endregion
    private int width, height;
    private float cellSize;
    private TGridObject[,] gridArray;
    private Vector3 origin;
    private TextMesh[,] debugTextArray;

    #region Constructor
    public Grid(int _w, int _h, float _c, Vector3 _origin, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        width = _w;
        height = _h;
        cellSize = _c;
        origin = _origin;

        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMesh[width,height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x,y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = true;

        if(showDebug)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                        debugTextArray[x,y]= UtilsClass.CreateWorldText(gridArray[x,y].ToString(), null, 
                            GetWorldPosition(x,y) + new Vector3(cellSize, cellSize) * .5f, 10, Color.white, 
                            TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x,y+1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x+1,y), Color.white, 100f);
                    }
                }
            Debug.DrawLine(GetWorldPosition(0,height), GetWorldPosition(width,height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width,0), GetWorldPosition(width,height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
        }
    }
    #endregion
    #region GridFunctions

    public int GetHeight() {return height;}
    public int GetWidth() {return width;}
    public float GetCellSize() {return cellSize;}

    private Vector3 GetWorldPosition(int _x, int _y)
    {
        return new Vector3(_x , _y) * cellSize + origin;
    }

    private void GetXY(Vector3 _worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((_worldPosition - origin).x / cellSize);
        y = Mathf.FloorToInt((_worldPosition - origin).y / cellSize);
    }

    public void SetValue(int _x, int _y, TGridObject _value)
    {
        if(_x >= 0 && _y >= 0 && _x < width && _y < height)
        {
            gridArray[_x, _y] = _value;
            debugTextArray[_x, _y].text = gridArray[_x, _y].ToString();
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = _x, y = _y });
        }
    }

    public void TriggerGridObjectChanged(int _x, int _y)
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = _x, y = _y });
    }

    public void SetValue(Vector3 _worldPosition, TGridObject _value)
    {
        int x,y;
        GetXY(_worldPosition, out x, out y);
        SetValue(x, y ,_value);
    }

    public TGridObject GetValue(int _x, int _y)
    {
        if(_x >= 0 && _y >= 0 && _x < width && _y < height)
        {
            return gridArray[_x, _y];
        }
        else return default(TGridObject);
    }

    public TGridObject GetValue(Vector3 _worldPosition)
    {
        int x,y;
        GetXY(_worldPosition, out x, out y);
        return GetValue(x, y);
    }
    #endregion
}
