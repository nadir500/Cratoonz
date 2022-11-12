using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardC : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject gridPrefab;
    public GameObject[] tilesReferences;
    public GameObject[,] tilesPool;
    public List<TileController> currentMatches = new List<TileController>();

    void Start()
    {
        Init();
    }

    public void Init()
    {
        int tileIndex = 0;
        Vector2Int tilePos = Vector2Int.zero;
        tilesPool = new GameObject[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 pos = new Vector2(i, j);
                tilePos.x = i;
                tilePos.y = j;

                GameObject tileGameObject = Instantiate(gridPrefab, pos, Quaternion.identity);
                tileGameObject.transform.parent = this.transform;
                tileGameObject.name = "Tile Container- " + i + " , " + j;
                tileIndex = Random.Range(0, tilesReferences.Length - 1);
                TilesSpawn(tilesReferences[tileIndex], tilePos);

            }
        }
    }
    private void TilesSpawn(GameObject singleTile, Vector2Int tilePos) //dealing with grids 
    {
        Vector3 vec3pos = new Vector3(tilePos.x, tilePos.y, 0.0f);
        TileController tileObject = Instantiate(singleTile, vec3pos, Quaternion.identity).GetComponent<TileController>();
        tileObject.transform.parent = this.transform;
        tileObject.name = "Actual Tile - " + tilePos.x + " , " + tilePos.y;
        tilesPool[tilePos.x, tilePos.y] = tileObject.gameObject;
        tileObject.SetupGem(tilePos, this);
    }

    public void FindAllMatches()
    {
        currentMatches.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                TileController currentTile = tilesPool[x, y].GetComponent<TileController>();
                if (currentTile != null)
                {
                    if (x > 0 && x < width - 1)
                    {
                        TileController leftTile = tilesPool[x - 1, y].GetComponent<TileController>();
                        TileController rightTile = tilesPool[x + 1, y].GetComponent<TileController>();
                        if (leftTile != null && rightTile != null)
                        {
                            if (leftTile.type == currentTile.type && rightTile.type == currentTile.type)
                            {
                                currentTile.isMatched = true;
                                leftTile.isMatched = true;
                                rightTile.isMatched = true;

                                currentMatches.Add(currentTile);
                                currentMatches.Add(leftTile);
                                currentMatches.Add(rightTile);
                            }
                        }
                    }

                    if (y > 0 && y < height - 1)
                    {
                        TileController aboveTile = tilesPool[x, y + 1].GetComponent<TileController>();
                        TileController belowTile = tilesPool[x, y - 1].GetComponent<TileController>();
                        if (aboveTile != null && belowTile != null)
                        {
                            if (aboveTile.type == currentTile.type && belowTile.type == currentTile.type)
                            {
                                currentTile.isMatched = true;
                                aboveTile.isMatched = true;
                                belowTile.isMatched = true;

                                currentMatches.Add(currentTile);
                                currentMatches.Add(aboveTile);
                                currentMatches.Add(belowTile);
                            }
                        }
                    }
                }
            }
        }
        // if (currentMatches.Count > 0)
        // {
        //     for (int i = 0; i < currentMatches.Count; i++)
        //     {
        //         if (currentMatches[i] != null)
        //         {
        //             DestroyTiles(currentMatches[i].singleTilePos);
        //         }
        //     }
        // }
    }

    private void DestroyTiles(Vector2Int tilePos)
    {
        if (tilesPool[tilePos.x, tilePos.y] != null)
        {
            if (tilesPool[tilePos.x, tilePos.y].GetComponent<TileController>().isMatched)
            {
                Destroy(tilesPool[tilePos.x, tilePos.y]);
                tilesPool[tilePos.x, tilePos.y] = null;
            }
        }
    }
}
