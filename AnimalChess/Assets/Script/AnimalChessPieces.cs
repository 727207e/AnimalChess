using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AnimalChessPieces : MonoBehaviour
{
    public bool isCapturedObject = false;
    public bool isMyPieces;
    public int nowMyTableIndex;
    public List<int> CanMoveTableIndexNumber = new List<int>();
    public List<CanMoveFieldCheck> CanMoveTableCheckBox = new List<CanMoveFieldCheck>();

    public List<GameObject> ShowPossibleMovePos;
    public GameObject GameObjectSelectedCheck;

    public void Start()
    {
        SetMyPossibleMove();
        DeactivePossibleMovePosition();
        GameObjectSelectedCheck.SetActive(false);
    }

    public virtual void MovePieces(int tableIndexNumber)
    {
        //포로 오브젝트가 아니면 이동 가능 확인하기
        if (!isCapturedObject)
        {
            //이동 가능 위치인가
            int findIndexInTable = CanMoveTableIndexNumber.FindIndex(x => x == tableIndexNumber);

            if (findIndexInTable == -1)
            {
                return;
            }
        }
        //해당 칸으로 이동
        transform.SetParent(ChessTable.instance.TableFrame[tableIndexNumber].transform);
        transform.localPosition = Vector3.zero;
        transform.Translate(0, 0.1f, 0);

        nowMyTableIndex = tableIndexNumber;

        //내 턴이 되면 다시 측정하는 걸로 수정할것
        SetMyPossibleMove();
    }

    public virtual void ShowPossibleMovePosition()
    {
        for(int index = 0; index < CanMoveTableIndexNumber.Count; index++)
        {
            if (CanMoveTableIndexNumber[index] != -1)
            {
                ShowPossibleMovePos[index].SetActive(true);
            }
        }
    }

    public virtual void DeactivePossibleMovePosition()
    {
        for (int index = 0; index < CanMoveTableIndexNumber.Count; index++)
        {
            if (CanMoveTableIndexNumber[index] != -1)
            {
                ShowPossibleMovePos[index].SetActive(false);
            }
        }
    }

    protected void EndMove()
    {

    }

    public void CatchPieces(int tableIndexNumber)
    {

    }

    private void SpawnPieces()
    {

    }

    protected void SetMyPossibleMove()
    {
        CanMoveTableIndexNumber.Clear();
        foreach (var checkBox in CanMoveTableCheckBox)
        {
            CanMoveTableIndexNumber.Add(checkBox.checkFrameNumber);
        }
    }
}
