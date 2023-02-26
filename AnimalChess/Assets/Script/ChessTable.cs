using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessTable : MonoBehaviour
{
    //9  10 11
    //6  7  8
    //3  4  5
    //0  1  2
    public List<FrameInfo> TableFrame;

    public GameObject jolObjectPref;
    public GameObject kingObjectPref;
    public GameObject DaeObjectPref;
    public GameObject JigObjectPref;

    public void Start()
    {
        //0번 1번 2번 4번 셋팅
        SpawnObject(DaeObjectPref, 0, true, "player_1_Dae");
        SpawnObject(kingObjectPref, 1, true, "player_1_King");
        SpawnObject(JigObjectPref, 2, true, "player_1_Jig");
        SpawnObject(jolObjectPref, 4, true, "player_1_jol");

        //7번 9번 10번 11번 셋팅
        SpawnObject(DaeObjectPref, 11, false, "player_2_Dae");
        SpawnObject(kingObjectPref, 10, false, "player_2_King");
        SpawnObject(JigObjectPref, 9, false, "player_2_Jig");
        SpawnObject(jolObjectPref, 7, false, "player_2_jol");
    }

    private GameObject SpawnObject(GameObject prefabObject, 
        int IndexNumber, bool isRotate, string ObjectName)
    {
        float rotateValue = 0;
        if (!isRotate)
            rotateValue = 180.0f;

        GameObject playerObject = Instantiate(prefabObject,
            TableFrame[IndexNumber].transform.position,
            Quaternion.Euler(0, rotateValue, 0));
        playerObject.name = ObjectName;
        playerObject.transform.SetParent(TableFrame[IndexNumber].transform);
        playerObject.transform.Translate(0, 0.1f, 0);

        return playerObject;
    }
}
