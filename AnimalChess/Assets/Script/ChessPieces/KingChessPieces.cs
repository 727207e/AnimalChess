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

    //public override bool MovePieces(int tableIndexNumber)
    //{
    //    return base.MovePieces(tableIndexNumber);
    //}

    protected override void EndMove()
    {
        //���� �� ��ġ�� �� ���� ��� üũ
        //if (GameManager.instance.ChessTable.TableFrame[nowMyTableIndex].isEnemyBaseFrame)
        //{
        //    //��������� ���� �����ʶ� �¸���.
        //    GameManager.instance.actionIsMyTurn += GameManager.instance.actionIsWin;
        //}
    }
}
