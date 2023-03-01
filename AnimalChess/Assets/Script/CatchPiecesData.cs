using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CatchPiecesData : MonoBehaviourPun
{
    [SerializeField] List<AnimalChessPieces> User_1_CatchPiecesList;
    [SerializeField] List<AnimalChessPieces> User_2_CatchPiecesList;

    Dictionary<int, List<AnimalChessPieces>> PiecesDic;

    [SerializeField] GameObject User_1_Pocket;
    [SerializeField] GameObject User_2_Pocket;

    public float pieceOffset = 2.5f;

    public void Start()
    {
        User_1_CatchPiecesList = new List<AnimalChessPieces>();
        User_2_CatchPiecesList = new List<AnimalChessPieces>();

        PiecesDic = new Dictionary<int, List<AnimalChessPieces>>()
            {{ 1, User_1_CatchPiecesList }, {2, User_2_CatchPiecesList }};

    }

    public void AddUserCatch(int ListNumber, AnimalChessPieces animalChessPieces)
    {
        int viewID = animalChessPieces.GetComponent<PhotonView>().ViewID;
        photonView.RPC("SyncAddUserCatch", RpcTarget.All, ListNumber, viewID);
    }

    [PunRPC]
    public void SyncAddUserCatch(int ListNumber, int chessPiecePhotonView)
    {
        AnimalChessPieces animalChessPieces = PhotonView.Find(chessPiecePhotonView).GetComponent<AnimalChessPieces>();

        PiecesDic[ListNumber].Add(animalChessPieces);
        MovePocketPosition(ListNumber, animalChessPieces);
    }

    public void UseUserCatchPiece(int ListNumber, AnimalChessPieces animalChessPieces)
    {
        PiecesDic[ListNumber].Remove(animalChessPieces);
    }

    private void MovePocketPosition(int ListNumber, AnimalChessPieces animalChessPieces)
    {
        animalChessPieces.isCapturedObject = true;
        animalChessPieces.isMyPieces = (ListNumber == GameManager.instance.MyPlayNumber);
        animalChessPieces.nowMyTableIndex = -1;
        animalChessPieces.CanMoveTableIndexNumber.Clear();
        animalChessPieces.CanMoveTableCheckBox.Clear();

        ShowCatchPiece();
    }

    private void ShowCatchPiece()
    {
        for (int index = 0; index < User_1_CatchPiecesList.Count; index++)
        {
            User_1_CatchPiecesList[index].transform.SetParent(User_1_Pocket.transform);
            User_1_CatchPiecesList[index].transform.localPosition = Vector3.zero;
            User_1_CatchPiecesList[index].transform.Translate(0, 0, pieceOffset * index);
        }

        for (int index = 0; index < User_2_CatchPiecesList.Count; index++)
        {
            User_2_CatchPiecesList[index].transform.SetParent(User_2_Pocket.transform);
            User_2_CatchPiecesList[index].transform.localPosition = Vector3.zero;
            User_2_CatchPiecesList[index].transform.Translate(0, 0, -1 * pieceOffset * index);
        }
    }
}
