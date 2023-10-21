using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
  private readonly int maxRow = 8;
  private readonly int maxCol = 8;
  private readonly float top = -3.57f;
  private readonly float left = -3.57f;
  private readonly float dx = 1.02f;
  private readonly float dy = 1.02f;

  [SerializeField] private Sprite[] numberSprites;
  [SerializeField] private GameObject tilePrefab;

  private GameObject[][] boardObj;

  private readonly int minSigns = 8;
  private readonly int minNumbers = 20;
  private List<int> signs;
  private List<int> numbers;

  public readonly int numberBegin = 0;
  public readonly int numberEnd = 10;
  public readonly int signBegin = 10;
  public readonly int signEnd = 14;

  public const int PLUS = 10;
  public const int MINUS = 11;
  public const int MULIPLY = 12;
  public const int DIVIDE = 13;

  private void Start()
  {
    GenerateRandomTokens();
    PlaceRandomTokensToBoard();
  }

  private void PlaceRandomTokensToBoard()
  {
    int[] tokens = new int[maxRow * maxRow];
    int numSigns = signs.Count;
    int i = 0;
    foreach (int t in signs) tokens[i++] = t;
    foreach (int t in numbers) tokens[i++] = t;

    RandomizeArray(tokens);

    Transform parent = gameObject.transform;
    boardObj = new GameObject[maxRow][];
    for (int r = 0; r < maxRow; r++)
    {
      boardObj[r] = new GameObject[maxCol];

      for (int c = 0; c < maxCol; c++)
      {
        int spriteIndex = tokens[r * maxCol + c];

        GameObject tileObj = Instantiate(tilePrefab, parent);

        tileObj.GetComponent<SpriteRenderer>().sprite = numberSprites[spriteIndex];
        tileObj.transform.localPosition = new Vector3(left + dx * c, top + dy * r, 0);
        tileObj.GetComponent<Tile>().spriteIndex = spriteIndex;

        boardObj[r][c] = tileObj;
      }
    }
  }

  private void GenerateRandomTokens()
  {
    signs = new List<int>();
    for (int i = 0; i < minSigns; i++) signs.Add(Random.Range(signBegin, signEnd));

    numbers = new List<int>();
    for (int i = 0; i < minNumbers; i++) numbers.Add(Random.Range(numberBegin, numberEnd));

    for (int i = minSigns + minNumbers; i < maxCol * maxRow; i++)
    {
      int r = Random.Range(0, signEnd);
      if (r < numberEnd) numbers.Add(r);
      else signs.Add(r);
    }
  }

  public void ResetTiles()
  {

    for (int r = 0; r < maxRow; r++)
    {
      for (int c = 0; c < maxCol; c++)
      {
        boardObj[r][c].GetComponent<Tile>().SetSelected(false);
      }
    }
  }

  private void RandomizeArray(int[] arr)
  {
    int n = arr.Length;
    for (int i = 0; i < n; i++)
    {
      int j = Random.Range(0, n);
      (arr[i], arr[j]) = (arr[j], arr[i]);
    }
  }

  public GameObject InstantiateSprite(int t)
  {
    GameObject tileObj = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);

    tileObj.GetComponent<SpriteRenderer>().sprite = numberSprites[t];    
    tileObj.GetComponent<Tile>().spriteIndex = t;

    return tileObj;
  }
}
