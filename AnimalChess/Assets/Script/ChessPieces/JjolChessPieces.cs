using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JjolChessPieces : AnimalChessPieces
{
    public string JjolChessPieceUpPrefab = "ChessPieces_JJol_Up";

    protected override void InitData()
    {
        canMovePoint.Add((-1, 0));
    }

    protected override void EndMove()
    {
        //if (GameManager.instance.ChessTable.TableFrame[nowMyTableIndex].isEnemyBaseFrame)
        //{
        //    object[] data = new object[3];
        //    data[0] = nowMyTableIndex;
        //    data[1] = "Jjol_Up";
        //    data[2] = !isMyPieces;

        //    GameObject playerObject = PhotonNetwork.Instantiate(JjolChessPieceUpPrefab,
        //        GameManager.instance.ChessTable.TableFrame[nowMyTableIndex].transform.position,
        //        Quaternion.Euler(0, 0, 0), 0, data);

        //    photonView.RPC("ActionRemove", RpcTarget.All);
        //}
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
