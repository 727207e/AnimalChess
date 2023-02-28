using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor;
using System.Threading.Tasks;

public abstract class AnimalChessPieces : MonoBehaviourPun
{
    public bool isCapturedObject = false;
    public bool isMyPieces;
    public int nowMyTableIndex;
    public List<int> CanMoveTableIndexNumber = new List<int>();
    public List<CanMoveFieldCheck> CanMoveTableCheckBox = new List<CanMoveFieldCheck>();

    public List<GameObject> ShowPossibleMovePos;
    public GameObject GameObjectSelectedCheck;

    public Material player_1_Mat;
    public Material player_2_Mat;


    public void InitChessPieces(int IndexNumber, string ObjectName, bool isMyPieces)
    {
        photonView.RPC("PhotonThisPiecesSetting", RpcTarget.All, IndexNumber, ObjectName, isMyPieces);
    }

    [PunRPC]
    public void PhotonThisPiecesSetting(int IndexNumber, string ObjectName, bool isMyPieces)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            isMyPieces = !isMyPieces;
        }

        transform.name = ObjectName;
        transform.SetParent(GameManager.instance.ChessTable.TableFrame[IndexNumber].transform);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        transform.Translate(0, 0.1f, 0);
        transform.GetComponent<AnimalChessPieces>().isMyPieces = isMyPieces;
        transform.GetComponent<AnimalChessPieces>().nowMyTableIndex = IndexNumber;

        SetMyPossibleMove();
        DeactivePossibleMovePosition();
        GameObjectSelectedCheck.SetActive(false);

        if (isMyPieces)
        {
            GetComponent<MeshRenderer>().material = player_1_Mat;
        }
        else
        {
            GetComponent<MeshRenderer>().material = player_2_Mat;
        }
    }

    public virtual void MovePieces(int tableIndexNumber)
    {
        //���� ������Ʈ�� �ƴϸ� �̵� ���� Ȯ���ϱ�
        if (!isCapturedObject)
        {
            //�̵� ���� ��ġ�ΰ�
            int findIndexInTable = CanMoveTableIndexNumber.FindIndex(x => x == tableIndexNumber);

            if (findIndexInTable == -1)
            {
                return;
            }
        }
        photonView.RPC("MovePiecesOnSync", RpcTarget.All, tableIndexNumber);
        GameManager.instance.MyTurnOver();

        //�� ���� �Ǹ� �ٽ� �����ϴ� �ɷ� �����Ұ�
        SetMyPossibleMove();
    }

    [PunRPC]
    public void MovePiecesOnSync(int tableIndexNumber)
    {
        //�ش� ĭ���� �̵�
        transform.SetParent(GameManager.instance.ChessTable.TableFrame[tableIndexNumber].transform);
        transform.localPosition = Vector3.zero;
        transform.Translate(0, 0.1f, 0);

        nowMyTableIndex = tableIndexNumber;
    }

    public virtual void ShowPossibleMovePosition()
    {
        SetMyPossibleMove();
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
            if (CanMoveTableIndexNumber[index] != -1)
            {
                ShowPossibleMovePos[index].SetActive(false);
            }
        }
    }

    protected void EndMove()
    {

    }

    public void CatchPieces(AnimalChessPieces enemyPiece)
    {
        Destroy(enemyPiece.gameObject);
        MovePieces(enemyPiece.nowMyTableIndex);
    }

    private void SpawnPieces()
    {

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
