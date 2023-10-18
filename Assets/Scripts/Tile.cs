using UnityEngine;

public class Tile : MonoBehaviour
{
  [SerializeField] private GameObject selectedObj;

  public int spriteIndex;
  public bool selected;

  public void SetSelected(bool selected)
  {
    this.selected = selected;
    selectedObj.SetActive(selected);
  }
}
