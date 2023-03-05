using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JjolChessPieces : AnimalChessPieces
{
    public string JjolChessPieceUpPrefab = "ChessPieces_JJol_Up";

    public GameObject imageJjol;
    public GameObject imageJjolUp;

    protected override void InitData()
    {
        JjolDataReset();

        base.InitData();
    }

    protected override void EndMove()
    {
        if (GameManager.instance.ChessTable.tableFrameNumber[nowMyTableIndex[0]][nowMyTableIndex[1]].Item1.isEnemyBaseFrame
            &&isMyPieces)
        {
            photonView.RPC("JjolPieceUpgrade", RpcTarget.All);
        }
    }

    [PunRPC]
    public void JjolPieceUpgrade()
    {
        imageJjol.SetActive(false);
        imageJjolUp.SetActive(true);

        canMovePoint.Clear();

        canMovePoint.Add((-1, 0));
        canMovePoint.Add((0, -1));
        canMovePoint.Add((0, 1));
        canMovePoint.Add((1, 0));
        canMovePoint.Add((-1, -1));
        canMovePoint.Add((-1, 1));
    }

    public override void PieceCatch()
    {
        photonView.RPC("JjolDataReset", RpcTarget.All);
    }

    [PunRPC]
    private void JjolDataReset()
    {
        imageJjol.SetActive(true);
        imageJjolUp.SetActive(false);

        canMovePoint.Clear();

        canMovePoint.Add((-1, 0));
    }

    [PunRPC]
    public void ActionRemove()
    {
        GameManager.instance.actionIsEnemyTurn -= DeactivePossibleMovePosition;

        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
