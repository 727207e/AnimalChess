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

    public bool isGameStart = false;

    public GameObject WinText;
    public GameObject LoseText;

    private bool isMyTurn = false;
    public bool IsMyTurn
    {
        get
        {
            return isMyTurn;
        }
        set
        {
            if(isGameStart) //게임중일때만 턴이 넘어옴
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

        isGameStart = true;
        actionIsWin += GameWinShowUp;
        actionIsLose += GameLosdShowUp;

        WinText.SetActive(false);
        LoseText.SetActive(false);
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

    public void GameWinShowUp()
    {
        GameWinSync();
        photonView.RPC("GameLoseSync", RpcTarget.Others);   //타인은 패배
        isGameStart = false;
    }

    [PunRPC]
    public void GameWinSync()
    {
        WinText.SetActive(true);
    }

    public void GameLosdShowUp()
    {
        GameLoseSync();
        photonView.RPC("GameWinSync", RpcTarget.Others); //타인은 승리
        isGameStart = false;
    }

    [PunRPC]
    public void GameLoseSync()
    {
        LoseText.SetActive(true);
    }
}
