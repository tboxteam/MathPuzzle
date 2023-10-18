using UnityEngine;

public class GameManager : MonoBehaviour
{
  private Camera mainCamera;

  [SerializeField] private SelectedDisplay selectedDisplay;

  private void Awake()
  {
    mainCamera = Camera.main;
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
    if(hitObj && !hitObj.GetComponent<Tile>().selected)
    {      
      GameObject tileObj = Instantiate(hitObj);
      selectedDisplay.AddTile(tileObj);
      hitObj.GetComponent<Tile>().SetSelected(true);
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
}
