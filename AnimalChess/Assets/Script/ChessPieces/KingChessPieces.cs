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
            //상대 킹 잡은경우 승리
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

        //만약 내 위치가 적 기지 라면 체크
        if (GameManager.instance.ChessTable.tableFrameNumber[nowMyTableIndex[0]][nowMyTableIndex[1]].Item1.isEnemyBaseFrame)
        {
            Debug.Log("King Stay win");
            //살아있으면 다음 내차례때 승리함.
            GameManager.instance.actionIsMyTurn += GameManager.instance.actionIsWin;
        }
    }
}
