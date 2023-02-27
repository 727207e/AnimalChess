using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CanMoveFieldCheck : MonoBehaviour
{
    public int checkFrameNumber = -1;
    bool isTrigger = false;

    public void CheckCanMovePosition()
    {
        //해당 체크 위치에 있는가 확인
        Collider[] hitColliders 
            = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 40, Quaternion.identity);

        //클릭 가능한 대상위에 아군이 있으면 못감.
        foreach(Collider collider in hitColliders)
        {
            if(collider.TryGetComponent<AnimalChessPieces>(out var chessPiece))
            {
                if(chessPiece.isMyPieces)
                {
                    checkFrameNumber = -1;
                    return;
                }
            }
        }

        //클릭 가능한 대상이 있는지 검색
        Collider clickableCollider = Array.Find(hitColliders, x => x.tag == "tile");
        if(clickableCollider != null)
        {
            checkFrameNumber = clickableCollider.GetComponent<FrameInfo>().tableIndexNumber;
        }
    }


    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log(other.tag);
    //    if (other.gameObject.transform.tag == "tile")
    //    {
    //        isTrigger = true;
    //        checkFrameNumber = other.gameObject.GetComponent<FrameInfo>().tableIndexNumber;
    //    }
    //}

    private void Update()
    {
        if(!isTrigger)
        {
            checkFrameNumber = -1;
        }
    }
}
