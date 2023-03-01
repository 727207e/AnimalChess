using Photon.Pun;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    public static GameManager instance;

    public ChessTable ChessTable;
    public CatchPiecesData CatchPiecesData;

    public int MyPlayNumber;        //1번이 마스터, 2번은 게스트

    public Material player_1_Mat;
    public Material player_2_Mat;

    private bool isMyTurn = false;
    public bool IsMyTurn
    {
        get
        {
            return isMyTurn;
        }
        set
        {
            isMyTurn = value;
            if (isMyTurn)
            {
                actionIsMyTurn?.Invoke();
            }
            else
            {
                actionIsEnemyTurn?.Invoke();
            }
        }
    }

    public Action actionIsMyTurn;
    public Action actionIsEnemyTurn;
    public Action actionIsWin;
    public Action actionIsLose;

    public void Awake()
    {
        instance = this;

        if(PhotonNetwork.IsMasterClient)
        {
            MyPlayNumber = 1;
            isMyTurn = true;
        }
        else
        {
            MyPlayNumber = 2;
        }
    }

    public void MyTurnOver()
    {
        photonView.RPC("PhotonTurnOver", RpcTarget.All);
    }

    [PunRPC]
    public void PhotonTurnOver()
    {
        IsMyTurn = !IsMyTurn;
    }
}
