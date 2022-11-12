using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TileType
{
    Red,
    Green,
    Blue,
    Yellow
}
public class TileDataModel
{
    private TileType _tileType;
    private SpriteRenderer _tileSpriteRenderer;

    public TileType tileType
    {
        get { return _tileType; }
    }
    public SpriteRenderer tileSprite
    {
        get { return _tileSpriteRenderer; }
        set { _tileSpriteRenderer = value; }
    }
    public TileDataModel(TileType tileType, SpriteRenderer tileSpriteRenderer)
    {
        _tileType = tileType;
        _tileSpriteRenderer = tileSpriteRenderer;
    }
}
