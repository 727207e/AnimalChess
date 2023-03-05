using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor;
using System.Threading.Tasks;

public abstract class AnimalChessPieces : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    protected List<(int, int)> canMovePoint = new List<(int, int)>();
    List<GameObject> canMovePointObjectList = new List<GameObject>();
    private bool isCapturedObject = false;
    public virtual bool IsCapturedObject
    {
        get
        {
            return isCapturedObject; 
        }
        set 
        {
            isCapturedObject = value; 
        }
    }

    public bool isMyPieces;
    public int[] nowMyTableIndex;
    public List<int> CanMoveTableIndexNumber = new List<int>();
    public List<CanMoveFieldCheck> CanMoveTableCheckBox = new List<CanMoveFieldCheck>();

    public List<GameObject> ShowPossibleMovePos;
    public GameObject GameObjectSelectedCheck;

    class SpawnObjectDataType
    {
        public int _indexNumberRow;
        public int _indexNumberCol;
        public string _objectName;
        public bool _isMypieces;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        nowMyTableIndex = new int[2];
        SpawnObjectDataType objectData = new SpawnObjectDataType();

        object[] data = this.gameObject.GetPhotonView().InstantiationData;
        if(data != null)
        {
            objectData._indexNumberRow = (int)data[0];
            objectData._indexNumberCol = (int)data[1];
            objectData._objectName = (string)data[2];
            objectData._isMypieces = (bool)data[3];
        }


        isMyPieces = objectData._isMypieces;
        if (!PhotonNetwork.IsMasterClient)
        {
            isMyPieces = !isMyPieces;
        }

        float rotateValue = 0;
        if (!isMyPieces)
            rotateValue = 180.0f;

        transform.localRotation = Quaternion.Euler(0, rotateValue, 0);

        transform.name = objectData._objectName;
        transform.SetParent(GameManager.instance.ChessTable.
            tableFrameNumber[objectData._indexNumberRow][objectData._indexNumberCol].Item1.transform);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        transform.Translate(0, 0.1f, 0);
        nowMyTableIndex[0] = objectData._indexNumberRow;
        nowMyTableIndex[1] = objectData._indexNumberCol;
        GameManager.instance.ChessTable.AddChessPiecesDataInTable(nowMyTableIndex[0], nowMyTableIndex[1], this);

        SetMyPossibleMove();
        GameManager.instance.actionIsMyTurn += SetMyPossibleMove;

        DeactivePossibleMovePosition();
        GameObjectSelectedCheck.SetActive(false);

        if (isMyPieces)
        {
            GetComponent<MeshRenderer>().material = GameManager.instance.player_1_Mat;
        }
        else
        {
            GetComponent<MeshRenderer>().material = GameManager.instance.player_2_Mat;
        }

        GameManager.instance.actionIsEnemyTurn += DeactivePossibleMovePosition;

        InitData();
    }

    protected virtual void InitData()
    {

    }

    protected virtual void ShowUpCanMovePoint()
    {
        List<List<(FrameInfo, AnimalChessPieces)>> tableClone = GameManager.instance.ChessTable.tableFrameNumber;

        foreach (var point in canMovePoint)
        {
            int GoalRow = nowMyTableIndex[0] + point.Item1;
            int GoalCol = nowMyTableIndex[1] + point.Item2;

            //row 연산
            if (GoalRow >= 0 && GoalRow < tableClone.Count)
            {
                //col 연산
                if(GoalCol >= 0 && GoalCol < tableClone[0].Count)
                {
                    //내 말이 거기 있으면 패스
                    if(tableClone[GoalRow][GoalCol].Item2 != null)
                    {
                        if(tableClone[GoalRow][GoalCol].Item2.isMyPieces)
                        {
                            continue;
                        }
                    }

                    GameObject gg = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    gg.transform.SetParent(tableClone[GoalRow][GoalCol].Item1.transform);
                    gg.transform.localScale = Vector3.one * 5f;
                    gg.transform.localPosition = Vector3.zero;
                    gg.AddComponent<CanMoveFieldCheck>().GoalPoint = (GoalRow, GoalCol);
                    gg.tag = "tile";
                    gg.layer = LayerMask.NameToLayer("Clickable");

                    canMovePointObjectList.Add(gg);
                }
            }
        }
    }


    public virtual bool MovePieces((int,int) tableIndexNumber)
    {
        photonView.RPC("MovePiecesOnSync", RpcTarget.All, tableIndexNumber.Item1, tableIndexNumber.Item2);
        GameManager.instance.MyTurnOver();

        return true;
    }

    [PunRPC]
    public void MovePiecesOnSync(int index1, int index2)
    {
        GameManager.instance.ChessTable.RemoveChessPiecesDataInTable(nowMyTableIndex[0], nowMyTableIndex[1]);

        //해당 칸으로 이동
        transform.SetParent(GameManager.instance.ChessTable.tableFrameNumber[index1][index2].Item1.transform);
        transform.localPosition = Vector3.zero;
        transform.Translate(0, 0.1f, 0);

        nowMyTableIndex[0] = index1;
        nowMyTableIndex[1] = index2;

        GameManager.instance.ChessTable.AddChessPiecesDataInTable(nowMyTableIndex[0], nowMyTableIndex[1], this);

        EndMove();
    }

    public virtual void ShowPossibleMovePosition()
    {
        ShowUpCanMovePoint();
    }

    public virtual void DeactivePossibleMovePosition()
    {
        foreach(var obj in canMovePointObjectList)
        {
            Destroy(obj);
        }
    }

    protected virtual void EndMove()
    {

    }

    public void CatchPieces(AnimalChessPieces enemyPiece)
    {
        GameManager.instance.CatchPiecesData.AddUserCatch(GameManager.instance.MyPlayNumber, enemyPiece);
    }

    public void SpawnPieces(int indexRow, int indexCol)
    {
        if(GameManager.instance.ChessTable.tableFrameNumber[indexRow][indexCol].Item2 != null)
        {
            return;
        }

        photonView.RPC("RemovePocketListIndex", RpcTarget.All);
        photonView.RPC("MovePiecesOnSync", RpcTarget.All, indexRow, indexCol);
        GameManager.instance.MyTurnOver();
    }

    [PunRPC]
    public void RemovePocketListIndex()
    {
        IsCapturedObject = false;
        GameManager.instance.CatchPiecesData.FindAndRemovePiece(this);
    }

    protected void SetMyPossibleMove()
    {
        //CanMoveTableIndexNumber.Clear();
        //foreach (var checkBox in CanMoveTableCheckBox)
        //{
        //    checkBox.CheckCanMovePosition();
        //    CanMoveTableIndexNumber.Add(checkBox.checkFrameNumber);
        //}
    }
}
