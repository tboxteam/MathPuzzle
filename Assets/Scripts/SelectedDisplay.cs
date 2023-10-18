using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDisplay : MonoBehaviour
{
  private readonly int maxTiles = 5;
  List<GameObject> tiles = new();

  private readonly float tileSize = 1;
  private readonly float tileGap = 0.2f;

  public void AddTile(GameObject tileObj)
  {
    if (tiles.Count == maxTiles) ClearTiles();

    tiles.Add(tileObj);

    RearrangeTiles();
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
    float left = -(tileSize + tileGap) * (tiles.Count-1)/2;
    float y = transform.position.y;

    for(int i=0; i<tiles.Count; i++)
    {
      tiles[i].transform.position = new Vector3(left + (tileSize + tileGap) * i, y, 0);
    }
  }
}

