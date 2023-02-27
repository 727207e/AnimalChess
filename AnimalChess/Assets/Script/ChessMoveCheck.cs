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
        //�Է� ����
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

        //�Է� ������
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
        //�� ���̰�, ������ ���
        if (clickedObj.isMyPieces && clickedObj.isCapturedObject)
        {
            preClickedObject = clickedObj;
        }

        //���õ� ���� ���ٸ�
        else if (preClickedObject == null)
        {
            //�� ���̶��
            if (clickedObj.isMyPieces)
            {
                preClickedObject = clickedObj;
                preClickedObject.ShowPossibleMovePosition();
            }
        }

        //���õ� ���� �ִٸ�
        else
        {
            //������ ���ý� �ʱ�ȭ
            if (preClickedObject.gameObject == clickedObj.gameObject)
            {
                preClickedObject.DeactivePossibleMovePosition();
                preClickedObject = null;
            }
            //�ٸ��� ���ý� 
            else
            {
                //���δ� ����
                if(preClickedObject.isCapturedObject)
                {
                    return;
                }

                //������ ������ ���
                else if(clickedObj.isMyPieces == false)
                {
                    preClickedObject.CatchPieces(clickedObj);
                }
            }
        }
    }

    private void SelectedTile (int tableIndexNumber)
    {
        //���õ� ���� ���ٸ�
        if(preClickedObject == null)
        {
            return;
        }

        //���õ� ���� �ִٸ�
        else
        {
            if(tableIndexNumber == -1)
            {
                preClickedObject = null;
            }
            else
            {
                //�����ΰ��
                if(preClickedObject.isCapturedObject)
                {
                    //�� ���� ��� ����
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
