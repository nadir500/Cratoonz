using System.Collections;
using System.Collections.Generic;

public class GridDataModel
{
    private int _width;
    private int _height;
    private TileDataModel[,] _tilesArray;

    public int width
    {
        get { return _width; }
    }
    public int height
    {
        get { return _height; }
    }
    public TileDataModel[,] tilesArray
    {
        get { return _tilesArray; }
        set { _tilesArray = value; }
    }
    public GridDataModel(int width, int height)
    {
        _width = width;
        _height = height;
    }
}
