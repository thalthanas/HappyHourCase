using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private RectTransform selectionBox;
    [SerializeField]
    private LayerMask unitLayers;
    [SerializeField]
    private LayerMask floorLayers;
    [SerializeField]
    private float dragDelay = 0.1f;


    private float mouseDownTime;
    private Vector2 startMousePosition;

    [SerializeField]
    private bool canMove;

    float unitSelectedTime;

    private void Update()
    {
       
        HandleSelectionInputs();
        HandleMovementInputs();
    }

    IEnumerator MovementInputBlocker()
    {
        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }

    private void HandleMovementInputs()
    {
        if(Input.GetKeyUp(KeyCode.Mouse0)&& SelectionManagerScript.Instance.SelectedUnits.Count>0 && canMove == true)
        {
            if(Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit,floorLayers))
            {
                foreach(SelectableUnitScript unit in SelectionManagerScript.Instance.SelectedUnits)
                {
                    
                    unit.MoveTo(hit.point);
                }
            }
        }
    }

    private void HandleSelectionInputs()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            selectionBox.sizeDelta = Vector2.zero;
            selectionBox.gameObject.SetActive(true);
            startMousePosition = Input.mousePosition;
            mouseDownTime = Time.time;

           

        }
        else if(Input.GetKey(KeyCode.Mouse0) && mouseDownTime+dragDelay<Time.time)
        {
            ResizeSelectionBox();
            canMove = false;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            selectionBox.sizeDelta = Vector2.zero;
            selectionBox.gameObject.SetActive(false);

            StartCoroutine(MovementInputBlocker());

            if(Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, unitLayers)
                &&hit.collider.TryGetComponent<SelectableUnitScript>(out SelectableUnitScript unit))
            {

                /*if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (SelectionManagerScript.Instance.IsSelected(unit))
                    {
                        SelectionManagerScript.Instance.DeSelect(unit);
                    }
                    else
                    {
                        SelectionManagerScript.Instance.Select(unit);
                    }
                }
                else
                {
                    SelectionManagerScript.Instance.DeSelectAll();
                    SelectionManagerScript.Instance.Select(unit);
                }*/
                SelectionManagerScript.Instance.DeSelectAll();
                SelectionManagerScript.Instance.Select(unit);

            }
            else if(mouseDownTime + dragDelay >Time.time)
            {
               // SelectionManagerScript.Instance.DeSelectAll();
            }
            mouseDownTime = 0;
        }
    }

    private void ResizeSelectionBox()
    {
        float width = Input.mousePosition.x - startMousePosition.x;
        float height = Input.mousePosition.y - startMousePosition.y;

        selectionBox.anchoredPosition = startMousePosition+ new Vector2(width/2, height/2);
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Bounds bounds = new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);

        for(int i = 0; i<SelectionManagerScript.Instance.AvailableUnits.Count;i++)
        {
            if (UnitIsInSelectionBox(camera.WorldToScreenPoint(SelectionManagerScript.Instance.AvailableUnits[i].transform.position),bounds))
            {
                SelectionManagerScript.Instance.Select(SelectionManagerScript.Instance.AvailableUnits[i]);
            }
            else 
            {
                SelectionManagerScript.Instance.DeSelect(SelectionManagerScript.Instance.AvailableUnits[i]);
            }
        }
    }

    private bool UnitIsInSelectionBox(Vector2 position, Bounds bounds)
    {
        return position.x > bounds.min.x && position.x < bounds.max.x 
            && position.y > bounds.min.y && position.y < bounds.max.y;
    }
}
