using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMoveCheck : MonoBehaviour
{
    AnimalChessPieces preClicked_Hide;
    AnimalChessPieces preClickedObject
    {
        get
        {
            return preClicked_Hide;
        }
        set
        {
            if(preClicked_Hide != null)
            {
                preClicked_Hide.GameObjectSelectedCheck.SetActive(false);
            }
            preClicked_Hide = value;

            if (preClicked_Hide != null)
            {
                preClicked_Hide.GameObjectSelectedCheck.SetActive(true);
            }
        }
    }

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
        int layerMask = 1 << LayerMask.NameToLayer("Clickable");
        if (Physics.Raycast(ray, out var _hit, 100000, layerMask))
        {
            if(_hit.collider.tag == "pieces")
            {
                SelectedPieces(_hit.collider.gameObject.GetComponent<AnimalChessPieces>());
            }
            else if(_hit.collider.tag == "tile")
            {
                SelectedTile(_hit.collider.gameObject.GetComponent<FrameInfo>().tableIndexNumber);
            }
            else
            {
                SelectedTile(-1);
            }
        }
    }

    private void SelectedPieces(AnimalChessPieces clickedObj)
    {
        //내 말이고, 포로인 경우
        if (clickedObj.isMyPieces && clickedObj.isCapturedObject)
        {
            preClickedObject = clickedObj;
        }

        //선택된 말이 없다면
        else if (preClickedObject == null)
        {
            //내 말이라면
            if (clickedObj.isMyPieces)
            {
                preClickedObject = clickedObj;
                preClickedObject.ShowPossibleMovePosition();
            }
        }

        //선택된 말이 있다면
        else
        {
            //같은거 선택시 초기화
            if (preClickedObject.gameObject == clickedObj.gameObject)
            {
                preClickedObject.DeactivePossibleMovePosition();
                preClickedObject = null;
            }
            //다른말 선택시 
            else
            {
                //포로는 제외
                if(preClickedObject.isCapturedObject)
                {
                    return;
                }

                //적군을 선택한 경우
                else if(clickedObj.isMyPieces == false)
                {
                    preClickedObject.CatchPieces(clickedObj);
                }
            }
        }
    }

    private void SelectedTile (int tableIndexNumber)
    {
        //선택된 말이 없다면
        if(preClickedObject == null)
        {
            return;
        }

        //선택된 말이 있다면
        else
        {
            if(tableIndexNumber == -1)
            {
                preClickedObject = null;
            }
            else
            {
                //포로인경우
                if(preClickedObject.isCapturedObject)
                {
                    //내 땅인 경우 생성
                    if (ChessTable.instance.TableFrame[tableIndexNumber].isMyFrame)
                    {
                        SpawnNewPieces(tableIndexNumber);
                    }
                }
                else
                {
                    preClickedObject.MovePieces(tableIndexNumber);
                }
            }
        }
    }

    private void SpawnNewPieces(int tableIndexNumber)
    {
        preClickedObject.MovePieces(tableIndexNumber);
    }
}
