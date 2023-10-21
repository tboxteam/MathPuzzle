using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  private Camera mainCamera;

  [SerializeField] private DisplayContoller answerDisplay;
  [SerializeField] private DisplayContoller selectedDisplay;
  [SerializeField] private BoardController boardController;

  private readonly int minNumTilesToCompute = 3;

  private readonly int answer = 12;

  private void Awake()
  {
    mainCamera = Camera.main;
    selectedDisplay.OnTileChange += TileChange;
  }

  private void Start()
  {
    answerDisplay.AddTile(boardController.InstantiateSprite(answer / 10));
    answerDisplay.AddTile(boardController.InstantiateSprite(answer % 10));
  }

  private void Update()
  {
    Vector3 downPosition = MouseOrTouchDownPosition();
    if (downPosition != default)
      MouseDownHandler(downPosition);
  }

  private void MouseDownHandler(Vector3 downPosition)
  {
    GameObject hitObj = GetHitObject(downPosition);
    if (hitObj && !hitObj.GetComponent<Tile>().selected)
    {
      GameObject tileObj = Instantiate(hitObj);

      hitObj.GetComponent<Tile>().SetSelected(true);

      selectedDisplay.AddTile(tileObj);
    }
  }

  private GameObject GetHitObject(Vector3 downPosition)
  {
    Ray ray = mainCamera.ScreenPointToRay(downPosition);
    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

    if (hit.collider == null) return null;

    return hit.collider.gameObject;
  }

  private Vector3 MouseOrTouchDownPosition()
  {
    if (Input.GetMouseButtonDown(0)) return Input.mousePosition;

    if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began)) return Input.GetTouch(0).position;

    return default;
  }

  private void TileChange(List<int> tileValues)
  {
    if (tileValues.Count >= minNumTilesToCompute)
    {
      var result = Compute(tileValues);
      if (result.success)
      {
        Debug.Log($" (value, answer) ({result.value}, {answer})");
        if (result.value == answer)
        {
          Debug.Log("equals");
        }
      }

    }

    if (selectedDisplay.GetNumTiles() == selectedDisplay.maxTiles)
    {
      selectedDisplay.ClearTiles();
      boardController.ResetTiles();
    }
  }

  private (bool success, int value) Compute(List<int> tileValues)
  {
    if (tileValues[0] >= boardController.numberEnd) return (false, 0);

    List<int> queue = new();

    int result = 0;
    bool wasSign = false; 
    foreach (int v in tileValues)
    {
      if(v<=boardController.numberEnd) // number case
      {
        if(wasSign)
        {
          result = v;
        } else
        {
          result = 10 * result + v;
        }
        wasSign = false;
      } else // sign case
      {
        if(wasSign)
        {
          return (false, 0);
        } else
        {
          queue.Add(result);
          queue.Add(v);
        }
        wasSign = true;
      }
    }
    if(wasSign) return (false, 0);
    queue.Add(result);

    result = queue[0];
    for(int i=1; i<queue.Count; i+=2)
    {
      switch (queue[i])
      {
        case BoardController.PLUS:
          result += queue[i + 1];
          break;
        case BoardController.MINUS:
          result -= queue[i + 1];
          break;
        case BoardController.MULIPLY:
          result *= queue[i + 1];
          break;
        case BoardController.DIVIDE:
          result /= queue[i + 1];
          break;
      }

    }

    return (true, result);
  }

}
