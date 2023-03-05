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
            if (preClicked_Hide != null)
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

    public void Start()
    {
        GameManager.instance.actionIsEnemyTurn += (() => preClickedObject = null);
    }

    public void Update()
    {
        if (!GameManager.instance.IsMyTurn || !GameManager.instance.isGameStart)
        {
            return;
        }

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
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif

        //�Է� ������
        int layerMask = 1 << LayerMask.NameToLayer("Clickable");
        if (Physics.Raycast(ray, out var _hit, 100000, layerMask))
        {
            if (_hit.collider.tag == "pieces")
            {
                SelectedPieces(_hit.collider.gameObject.GetComponent<AnimalChessPieces>());
            }
            else if (_hit.collider.tag == "tile")
            {
                SelectedTile(_hit.collider.gameObject.GetComponent<CanMoveFieldCheck>().GoalPoint);
            }
            else if (_hit.collider.tag == "FrameInfo")
            {
                SelectedFrame(_hit.collider.gameObject.GetComponent<FrameInfo>());
            }
        }
    }

    private void SelectedPieces(AnimalChessPieces clickedObj)
    {
        //�� ���̰�, ������ ���
        if (clickedObj.isMyPieces && clickedObj.IsCapturedObject)
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
        }
    }

    private void SelectedTile((int, int) tableIndexNumber)
    {
        //���õ� ���� ���ٸ�
        if (preClickedObject == null)
        {
            return;
        }

        //���õ� ���� �ִٸ�
        else
        {
            //�����ΰ��
            if (preClickedObject.IsCapturedObject)
            {
                return;
            }
            else
            {
                preClickedObject.MovePieces(tableIndexNumber);
            }
        }
    }

    private void SelectedFrame(FrameInfo frame)
    {
        //���õ� ���� ���ٸ�
        if (preClickedObject == null)
        {
            return;
        }

        //���õ� ���� �ִٸ�
        else
        {
            //�����ΰ��
            if (preClickedObject.IsCapturedObject)
            {
                (int,int) indexs = SearchFrameIndex(frame);

                if(indexs.Item1 == -1 && indexs.Item2 == -1)
                {
                    Debug.Log("��ã��");
                    return;
                }

                //�� base�� �ƴ� ���
                if (!GameManager.instance.ChessTable.tableFrameNumber[indexs.Item1][indexs.Item2].Item1.isEnemyBaseFrame)
                {
                    preClickedObject.SpawnPieces(indexs.Item1, indexs.Item2);
                }
            }
        }
    }

    private (int,int) SearchFrameIndex(FrameInfo frame)
    {
        List<List<(FrameInfo, AnimalChessPieces)>> tableClone = GameManager.instance.ChessTable.tableFrameNumber;

        for (int i = 0; i < tableClone.Count; i++)
        {
            for (int j = 0; j < tableClone[0].Count; j++)
            {
                if (frame == tableClone[i][j].Item1)
                {
                    return (i, j);
                }
            }
        }

        return (-1, -1);
    }
}
