using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingChessPieces : AnimalChessPieces
{
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

    public override bool MovePieces(int tableIndexNumber)
    {
        return base.MovePieces(tableIndexNumber);
    }

    protected override void EndMove()
    {
        //만약 내 위치가 적 기지 라면 체크
        if (GameManager.instance.ChessTable.TableFrame[nowMyTableIndex].isEnemyBaseFrame)
        {
            //살아있으면 다음 내차례때 승리함.
            GameManager.instance.actionIsMyTurn += GameManager.instance.actionIsWin;
        }
    }
}
