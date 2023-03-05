using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingChessPieces : AnimalChessPieces
{
    protected override void InitData()
    {
        canMovePoint.Add((-1, 0));
        canMovePoint.Add((0, -1));
        canMovePoint.Add((0, 1));
        canMovePoint.Add((1, 0));
        canMovePoint.Add((-1, -1));
        canMovePoint.Add((-1, 1));
        canMovePoint.Add((1, 1));
        canMovePoint.Add((1, -1));
    }

    public override bool IsCapturedObject 
    {
        get => base.IsCapturedObject;
        set
        {
            //��� ŷ ������� �¸�
            if(value == true && photonView.IsMine)
            {
                GameManager.instance.actionIsWin?.Invoke();
            }
        }
    }

    protected override void EndMove()
    {
        if(!isMyPieces)
        {
            return;
        }

        //���� �� ��ġ�� �� ���� ��� üũ
        if (GameManager.instance.ChessTable.tableFrameNumber[nowMyTableIndex[0]][nowMyTableIndex[1]].Item1.isEnemyBaseFrame)
        {
            Debug.Log("King Stay win");
            //��������� ���� �����ʶ� �¸���.
            GameManager.instance.actionIsMyTurn += GameManager.instance.actionIsWin;
        }
    }
}
