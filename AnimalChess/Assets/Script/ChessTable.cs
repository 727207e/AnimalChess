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
    [SerializeField] List<FrameInfo> TableFrame;

    //0 1 2     0
    //0 1 2     1
    //0 1 2     2
    //0 1 2     3
    public List<List<(FrameInfo, AnimalChessPieces)>> tableFrameNumber;

    public string jolObjectPref;
    public string kingObjectPref;
    public string DaeObjectPref;
    public string JigObjectPref;

    public void Start()
    {
        //2번 플레이어면 판을 뒤집음.
        if (PhotonNetwork.IsMasterClient)
        {
            transform.Rotate(0, 180, 0);
        }

        tableFrameNumber = new List<List<(FrameInfo, AnimalChessPieces)>>();
        tableFrameNumber.Add(new List<(FrameInfo, AnimalChessPieces)>()
        { (TableFrame[0],null), (TableFrame[1],null), (TableFrame[2],null) });

        tableFrameNumber.Add(new List<(FrameInfo, AnimalChessPieces)>()
        { (TableFrame[3],null), (TableFrame[4],null), (TableFrame[5],null) });

        tableFrameNumber.Add(new List<(FrameInfo, AnimalChessPieces)>()
        { (TableFrame[6],null), (TableFrame[7],null), (TableFrame[8],null) });

        tableFrameNumber.Add(new List<(FrameInfo, AnimalChessPieces)>()
        { (TableFrame[9],null), (TableFrame[10],null), (TableFrame[11],null) });

        //마스터는 생성
        if (PhotonNetwork.IsMasterClient)
        {
            //0번 1번 2번 4번 셋팅
            SpawnObject(DaeObjectPref, 3, 0, "player_1_Dae", true);
            SpawnObject(kingObjectPref, 3, 1, "player_1_King", true);
            SpawnObject(JigObjectPref, 3, 2, "player_1_Jig", true);
            SpawnObject(jolObjectPref, 2, 1, "player_1_jol", true);

            //7번 9번 10번 11번 셋팅
            SpawnObject(DaeObjectPref, 0, 2, "player_2_Dae", false);
            SpawnObject(kingObjectPref, 0, 1, "player_2_King", false);
            SpawnObject(JigObjectPref, 0, 0, "player_2_Jig", false);
            SpawnObject(jolObjectPref, 1, 1, "player_2_jol", false);
        }

        //내 진영 체크
        if (GameManager.instance.MyPlayNumber == 1)
        {
            CheckEnemyBaseGround(new int[] { 9, 10, 11 });
        }

        else if (GameManager.instance.MyPlayNumber == 2)
        {
            CheckEnemyBaseGround(new int[] { 0, 1, 2 });
        }

    }

    private GameObject SpawnObject(string prefabObjectString, int IndexNumberRow, int IndexNumberCol, string ObjectName, bool isMyPieces)
    {
        object[] data = new object[4];
        data[0] = IndexNumberRow;
        data[1] = IndexNumberCol;
        data[2] = ObjectName;
        data[3] = isMyPieces;

        GameObject playerObject = PhotonNetwork.Instantiate(prefabObjectString,
            tableFrameNumber[IndexNumberRow][IndexNumberCol].Item1.transform.position,
            Quaternion.Euler(0, 0, 0), 0, data);

        return playerObject;
    }

    private void CheckEnemyBaseGround(int[] enemyBaseRandIndex)
    {
        foreach (int index in enemyBaseRandIndex)
        {
            TableFrame[index].isEnemyBaseFrame = true;
        }
    }

    public void AddChessPiecesDataInTable(int rowIndex, int colIndex, AnimalChessPieces animalChessPieces)
    {
        //데이터가 있으면 Catch
        if (tableFrameNumber[rowIndex][colIndex].Item2 != null)
        {
            animalChessPieces.CatchPieces(tableFrameNumber[rowIndex][colIndex].Item2);
        }

        //데이터 추가
        (FrameInfo, AnimalChessPieces) Data = (tableFrameNumber[rowIndex][colIndex].Item1, animalChessPieces);
        tableFrameNumber[rowIndex][colIndex] = Data;
    }

    public void RemoveChessPiecesDataInTable(int rowIndex, int colIndex)
    {
        //데이터 초기화
        (FrameInfo, AnimalChessPieces) Data = (tableFrameNumber[rowIndex][colIndex].Item1, null);
        tableFrameNumber[rowIndex][colIndex] = Data;
    }
}
