using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum Color
{
    Red, 
    Green, 
    Blue, 
    Yellow
}
public class TileController : MonoBehaviour
{
    public Vector2Int singleTilePos;
    public BoardC boardController; //ref

    private Vector2 initialTouchPosition;
    private Vector2 finalTouchPosition;
    private bool IsClicked;
    private float swipeAngle = 0;
    private TileController replacedTile;
    public Color type; 
    public bool isMatched; 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsClicked && Input.GetMouseButtonUp(0))
        {
            IsClicked = false;
            finalTouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            // Debug.Log("GetMouseButtonUp " + finalTouchPosition);
            CalculateMovementAngleSpace(initialTouchPosition, finalTouchPosition);
        }
    }
    public void SetupGem(Vector2Int pos, BoardC boardC)
    {
        singleTilePos = pos;
        boardController = boardC;
    }
    private void OnMouseDown()
    {
        initialTouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        IsClicked = true;
        // Debug.Log("OnMouseDown " + initialTouchPosition);
    }
    private void CalculateMovementAngleSpace(Vector3 firstPoint, Vector3 secondPoint)
    {
        float floatingPointDistance = Vector2.Distance(firstPoint, secondPoint);
        swipeAngle = Mathf.Atan2(secondPoint.y - firstPoint.y, secondPoint.x - firstPoint.x);
        swipeAngle = swipeAngle * Mathf.Rad2Deg;

        if (floatingPointDistance > 0.06f)
        {
            //do the swipe 
        }
        // Debug.Log("swipeAngle " + swipeAngle + " " + floatingPointDistance);
        SwipeTiles(swipeAngle);
    }
    private void SwipeTiles(float resultedAngle)
    {
        if (singleTilePos.x < boardController.width - 1 || singleTilePos.y < boardController.height - 1)
        {

            if (resultedAngle < 45.0f && resultedAngle > -45.0f)
            {
                replacedTile = boardController.tilesPool[singleTilePos.x + 1, singleTilePos.y].GetComponent<TileController>(); //get the tile
                replacedTile.singleTilePos.x--;
                this.singleTilePos.x++;
            }
            else
            {
                if (resultedAngle > 45.0f && resultedAngle <= 135.0f)
                {
                    replacedTile = boardController.tilesPool[singleTilePos.x, singleTilePos.y + 1].GetComponent<TileController>(); //get the tile
                    replacedTile.singleTilePos.y--;
                    this.singleTilePos.y++;
                }
                else
                {
                    if (resultedAngle < -45.0f && resultedAngle >= -135.0f && singleTilePos.y > 0)
                    {
                        replacedTile = boardController.tilesPool[singleTilePos.x, singleTilePos.y - 1].GetComponent<TileController>(); //get the tile
                        replacedTile.singleTilePos.y++;
                        this.singleTilePos.y--;
                    }
                    else
                    {
                        if ((resultedAngle > 135.0f || resultedAngle < -135.0f) && singleTilePos.x > 0)
                        {
                            replacedTile = boardController.tilesPool[singleTilePos.x - 1, singleTilePos.y].GetComponent<TileController>(); //get the tile
                            replacedTile.singleTilePos.x++;
                            this.singleTilePos.x--;
                        }
                    }
                }
            }
            if (replacedTile != null)
            {
                Debug.Log("replaced " + singleTilePos + replacedTile.singleTilePos);
                boardController.tilesPool[singleTilePos.x, singleTilePos.y] = this.gameObject;
                boardController.tilesPool[replacedTile.singleTilePos.x, replacedTile.singleTilePos.y] = replacedTile.gameObject;
                StartCoroutine(MoveTiles(this.transform, replacedTile.transform));
            }
        }
    }
    IEnumerator MoveTiles(Transform p1, Transform p2)
    {
        Vector3 pos1 = p1.localPosition;
        Tween movement = p1.DOLocalMove(p2.localPosition, 0.5f);
        movement = p2.DOLocalMove(pos1, 0.5f);
        yield return movement.WaitForCompletion();
        boardController.FindAllMatches();
    }


}
