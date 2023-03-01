using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor;
using System.Threading.Tasks;

public abstract class AnimalChessPieces : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public bool isCapturedObject = false;
    public bool isMyPieces;
    public int nowMyTableIndex;
    public List<int> CanMoveTableIndexNumber = new List<int>();
    public List<CanMoveFieldCheck> CanMoveTableCheckBox = new List<CanMoveFieldCheck>();

    public List<GameObject> ShowPossibleMovePos;
    public GameObject GameObjectSelectedCheck;

    class SpawnObjectDataType
    {
        public int _indexNumber;
        public string _objectName;
        public bool _isMypieces;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        SpawnObjectDataType objectData = new SpawnObjectDataType();

        object[] data = this.gameObject.GetPhotonView().InstantiationData;
        if(data != null)
        {
            objectData._indexNumber = (int)data[0];
            objectData._objectName = (string)data[1];
            objectData._isMypieces = (bool)data[2];
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
        transform.SetParent(GameManager.instance.ChessTable.TableFrame[objectData._indexNumber].transform);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        transform.Translate(0, 0.1f, 0);
        transform.GetComponent<AnimalChessPieces>().isMyPieces = isMyPieces;
        transform.GetComponent<AnimalChessPieces>().nowMyTableIndex = objectData._indexNumber;

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
    }

    public virtual bool MovePieces(int tableIndexNumber)
    {
        //포로 오브젝트가 아니면 이동 가능 확인하기
        if (!isCapturedObject)
        {
            //이동 가능 위치인가
            int findIndexInTable = CanMoveTableIndexNumber.FindIndex(x => x == tableIndexNumber);

            if (findIndexInTable == -1)
            {
                return false;
            }
        }
        photonView.RPC("MovePiecesOnSync", RpcTarget.All, tableIndexNumber);
        GameManager.instance.MyTurnOver();

        return true;
    }

    [PunRPC]
    public void MovePiecesOnSync(int tableIndexNumber)
    {
        //해당 칸으로 이동
        transform.SetParent(GameManager.instance.ChessTable.TableFrame[tableIndexNumber].transform);
        transform.localPosition = Vector3.zero;
        transform.Translate(0, 0.1f, 0);

        nowMyTableIndex = tableIndexNumber;
    }

    public virtual void ShowPossibleMovePosition()
    {
        for (int index = 0; index < CanMoveTableIndexNumber.Count; index++)
        {
            if (CanMoveTableIndexNumber[index] != -1)
            {
                ShowPossibleMovePos[index].SetActive(true);
            }
        }
    }

    public virtual void DeactivePossibleMovePosition()
    {
        for (int index = 0; index < CanMoveTableIndexNumber.Count; index++)
        {
            ShowPossibleMovePos[index].SetActive(false);
        }
    }

    protected void EndMove()
    {

    }

    public void CatchPieces(AnimalChessPieces enemyPiece)
    {
        if(MovePieces(enemyPiece.nowMyTableIndex))
        {
            GameManager.instance.CatchPiecesData.AddUserCatch(GameManager.instance.MyPlayNumber, enemyPiece);
        }
    }

    public void SpawnPieces(int tableIndexNumber)
    {
        photonView.RPC("RemovePocketListIndex", RpcTarget.All);
        photonView.RPC("MovePiecesOnSync", RpcTarget.All, tableIndexNumber);
        GameManager.instance.MyTurnOver();
    }

    [PunRPC]
    public void RemovePocketListIndex()
    {
        GameManager.instance.CatchPiecesData.FindAndRemovePiece(this);
    }

    protected void SetMyPossibleMove()
    {
        CanMoveTableIndexNumber.Clear();
        foreach (var checkBox in CanMoveTableCheckBox)
        {
            checkBox.CheckCanMovePosition();
            CanMoveTableIndexNumber.Add(checkBox.checkFrameNumber);
        }
    }
}
