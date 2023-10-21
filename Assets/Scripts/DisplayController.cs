using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayContoller : MonoBehaviour
{
  public readonly int maxTiles = 5;
  readonly List<GameObject> tiles = new();

  private readonly float tileSize = 1;
  private readonly float tileGap = 0.2f;

  [SerializeField] private GameObject background;
  [SerializeField] private bool adjustBackground;
  
  public delegate void OnTileChangeDelegate(List<int> tileValues);
  public event OnTileChangeDelegate OnTileChange;

  public void AddTile(GameObject tileObj)
  {
    if (GetNumTiles() == maxTiles) return;

    tiles.Add(tileObj);

    RearrangeTiles();

    OnTileChange?.Invoke(GetTileValues());
  }

  public List<int> GetTileValues()
  {
    List<int> tileValues = new();
    foreach (GameObject tile in tiles)
    {
      tileValues.Add(tile.GetComponent<Tile>().spriteIndex);
    }

    return tileValues;
  }

  public int GetNumTiles()
  {
    return tiles.Count;
  }

  public void ClearTiles()
  {
    foreach (GameObject tile in  tiles)
    {
      Destroy(tile);
    }
    tiles.Clear();
  }

  private void RearrangeTiles()
  {
    if(adjustBackground)
    {
      float width = (tiles.Count == 0) ? tileSize + 2 * tileGap : (tileSize + tileGap) * tiles.Count + tileGap;
      background.transform.localScale = new Vector3(width, 1.4f, 1);
    }

    float left = -(tileSize + tileGap) * (tiles.Count-1)/2;
    float y = transform.position.y;

    for(int i=0; i<tiles.Count; i++)
    {
      tiles[i].transform.position = new Vector3(left + (tileSize + tileGap) * i, y, 0);
    }
  }
}

