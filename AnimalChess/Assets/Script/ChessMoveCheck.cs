using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMoveCheck : MonoBehaviour
{
    AnimalChessPieces preClickedObject;
    Ray ray;

    public void Update()
    {
        UserInputCheck();
    }

    private void UserInputCheck()
    {
        //입력 감지
#if (UNITY_ANDROID || UNITY_IOS)
        if (Input.touchCount <= 0)
        {
            return;
        }
        ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#elif UNITY_EDITOR
        if(!Input.GetMouseButtonDown(0))
        {
            return;
        }
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif

        //입력 나누기
        if (Physics.Raycast(ray, out var _hit, 100000))
        {
            if(_hit.collider.tag == "pieces")
            {
                SelectedPieces(_hit.collider.gameObject);
            }
            else if(_hit.collider.tag == "tile")
            {
                SelectedTile();
            }
            else
            {
                SelectedTile();
            }
        }
    }

    private void SelectedPieces(GameObject clickedObj)
    {
        //값이 있으면
        if (preClickedObject == null)
        {
            preClickedObject = clickedObj.GetComponent<AnimalChessPieces>();
            preClickedObject.ShowPossibleMovePosition();
        }
    }

    private void SelectedTile ()
    {

    }

    private void MoveTile()
    {

    }

    private void PutDataInTile()
    {

    }

    private void SpawnNewPieces()
    {

    }
}
