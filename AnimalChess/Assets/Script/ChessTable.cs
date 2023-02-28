using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessTable : MonoBehaviourPun
{
    public static ChessTable instance;

    //9  10 11
    //6  7  8
    //3  4  5
    //0  1  2
    public List<FrameInfo> TableFrame;

    public string jolObjectPref;
    public string kingObjectPref;
    public string DaeObjectPref;
    public string JigObjectPref;

    public bool isFirstUser = true;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        //0번 1번 2번 4번 셋팅
        SpawnObject(DaeObjectPref, 0, true, "player_1_Dae", true);
        SpawnObject(kingObjectPref, 1, true, "player_1_King", true);
        SpawnObject(JigObjectPref, 2, true, "player_1_Jig", true);
        SpawnObject(jolObjectPref, 4, true, "player_1_jol", true);

        //7번 9번 10번 11번 셋팅
        SpawnObject(DaeObjectPref, 11, false, "player_2_Dae", false);
        SpawnObject(kingObjectPref, 10, false, "player_2_King", false);
        SpawnObject(JigObjectPref, 9, false, "player_2_Jig", false);
        SpawnObject(jolObjectPref, 7, false, "player_2_jol", false);


        //내 진영 체크
        if (isFirstUser)
        {
            CheckMyGround(new int[] { 0, 1, 2, 3, 4, 5 });
        }

        else if (!isFirstUser)
        {
            CheckMyGround(new int[] { 6, 7, 8, 9, 10, 11 });
        }
    }

    private GameObject SpawnObject(string prefabObjectString,
        int IndexNumber, bool isRotate, string ObjectName, bool isMyPieces)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return null;
        }

        float rotateValue = 0;
        if (!isRotate)
            rotateValue = 180.0f;

        GameObject playerObject = PhotonNetwork.Instantiate(prefabObjectString,
            TableFrame[IndexNumber].transform.position,
            Quaternion.Euler(0, rotateValue, 0));
        playerObject.GetComponent<AnimalChessPieces>().InitChessPieces(IndexNumber, ObjectName, isMyPieces);

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
