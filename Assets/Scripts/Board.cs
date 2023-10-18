using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
  private readonly int maxRow = 8;
  private readonly int maxCol = 8;
  private readonly float top = -3.57f;
  private readonly float left = -3.57f;
  private readonly float dx = 1.02f;
  private readonly float dy = 1.02f;

  [SerializeField] private Sprite[] numberSprites;
  [SerializeField] private GameObject tilePrefab;

  private GameObject[][] board;

  private void Awake()
  {
    board = new GameObject[maxRow][];
    for(int r=0; r<maxRow; r++)
    {
      board[r] = new GameObject[maxCol];

      for(int c=0; c<maxCol; c++)
      {
        int spriteIndex = GetRandomSpriteIndex();

        GameObject tileObj = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);

        tileObj.GetComponent<SpriteRenderer>().sprite = numberSprites[spriteIndex];
        tileObj.transform.position = new Vector3(left+dx*c, top+dy*r, 0);
        tileObj.GetComponent<Tile>().spriteIndex = spriteIndex;

        board[r][c] = tileObj;
      }
    }
  }

  private int GetRandomSpriteIndex()
  {
    return Random.Range(0, numberSprites.Length);
  }

  public void ResetTiles()
  {

    for (int r = 0; r < maxRow; r++)
    {
      for (int c = 0; c < maxCol; c++)
      {
        board[r][c].GetComponent<Tile>().SetSelected(false);
      }
    }
  }
}
