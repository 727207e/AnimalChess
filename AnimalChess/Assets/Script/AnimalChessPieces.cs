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

    public Material player_1_Mat;
    public Material player_2_Mat;

    public void Start()
    {
        SetMyPossibleMove();
        DeactivePossibleMovePosition();
        GameObjectSelectedCheck.SetActive(false);

        if(isMyPieces)
        {
            GetComponent<MeshRenderer>().material = player_1_Mat;
        }
        else
        {
            GetComponent<MeshRenderer>().material = player_2_Mat;
        }
    }

    public virtual void MovePieces(int tableIndexNumber)
    {
        //���� ������Ʈ�� �ƴϸ� �̵� ���� Ȯ���ϱ�
        if (!isCapturedObject)
        {
            //�̵� ���� ��ġ�ΰ�
            int findIndexInTable = CanMoveTableIndexNumber.FindIndex(x => x == tableIndexNumber);

            if (findIndexInTable == -1)
            {
                return;
            }
        }
        //�ش� ĭ���� �̵�
        transform.SetParent(ChessTable.instance.TableFrame[tableIndexNumber].transform);
        transform.localPosition = Vector3.zero;
        transform.Translate(0, 0.1f, 0);

        nowMyTableIndex = tableIndexNumber;

        //�� ���� �Ǹ� �ٽ� �����ϴ� �ɷ� �����Ұ�
        SetMyPossibleMove();
    }

    public virtual void ShowPossibleMovePosition()
    {
        SetMyPossibleMove();
        for (int index = 0; index < CanMoveTableIndexNumber.Count; index++)
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

    public void CatchPieces(AnimalChessPieces enemyPiece)
    {
        Destroy(enemyPiece.gameObject);
        MovePieces(enemyPiece.nowMyTableIndex);
    }

    private void SpawnPieces()
    {

    }

    protected void SetMyPossibleMove()
    {
        CanMoveTableIndexNumber.Clear();
        foreach (var checkBox in CanMoveTableCheckBox)
        {
            checkBox.CheckCanMovePosition();
            CanMoveTableIndexNumber.Add(checkBox.checkFrameNumber);
        }
    }
}
