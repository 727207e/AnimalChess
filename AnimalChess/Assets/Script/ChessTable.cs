using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessTable : MonoBehaviourPun
{
    //9  10 11
    //6  7  8
    //3  4  5
    //0  1  2
    public List<FrameInfo> TableFrame;

    public string jolObjectPref;
    public string kingObjectPref;
    public string DaeObjectPref;
    public string JigObjectPref;

    public void Start()
    {
        //2번 플레이어면 판을 뒤집음.
        if (!PhotonNetwork.IsMasterClient)
        {
            transform.Rotate(0, 180, 0);
        }
        
        //마스터는 생성
        if (PhotonNetwork.IsMasterClient)
        {
            //0번 1번 2번 4번 셋팅
            SpawnObject(DaeObjectPref, 0, "player_1_Dae", true);
            SpawnObject(kingObjectPref, 1, "player_1_King", true);
            SpawnObject(JigObjectPref, 2, "player_1_Jig", true);
            SpawnObject(jolObjectPref, 4, "player_1_jol", true);

            //7번 9번 10번 11번 셋팅
            SpawnObject(DaeObjectPref, 11, "player_2_Dae", false);
            SpawnObject(kingObjectPref, 10, "player_2_King", false);
            SpawnObject(JigObjectPref, 9, "player_2_Jig", false);
            SpawnObject(jolObjectPref, 7, "player_2_jol", false);
        }

        //내 진영 체크
        if (GameManager.instance.MyPlayNumber == 1)
        {
            CheckMyGround(new int[] { 0, 1, 2, 3, 4, 5 });
        }

        else if (GameManager.instance.MyPlayNumber == 2)
        {
            CheckMyGround(new int[] { 6, 7, 8, 9, 10, 11 });
        }

    }

    private GameObject SpawnObject(string prefabObjectString, int IndexNumber, string ObjectName, bool isMyPieces)
    {
        object[] data = new object[3];
        data[0] = IndexNumber;
        data[1] = ObjectName;
        data[2] = isMyPieces;

        GameObject playerObject = PhotonNetwork.Instantiate(prefabObjectString,
            TableFrame[IndexNumber].transform.position,
            Quaternion.Euler(0, 0, 0), 0, data);

        return playerObject;
    }

    private void CheckMyGround(int[] myRandIndexs)
    {
        foreach (int index in myRandIndexs)
        {
            TableFrame[index].isMyFrame = true;
        }
    }
}
