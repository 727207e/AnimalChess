using UnityEngine;
using Photon.Pun;

public class PhotonUserSync : MonoBehaviourPunCallbacks
{
    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.K))
        {
            photonView.RPC("MoveChessObject", RpcTarget.All);
        }
    }

    [PunRPC]
    public void MoveChessObject()
    {
        Debug.Log("hh");
    }
}
