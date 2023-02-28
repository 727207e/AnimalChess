using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CanMoveFieldCheck : MonoBehaviour
{
    public int checkFrameNumber = -1;

    public void CheckCanMovePosition()
    {
        //�ش� üũ ��ġ�� �ִ°� Ȯ��
        Collider[] hitColliders 
            = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 40, Quaternion.identity);

        //Ŭ�� ������ ������� �Ʊ��� ������ ����.
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

        //Ŭ�� ������ ����� �ִ��� �˻�
        Collider clickableCollider = Array.Find(hitColliders, x => x.tag == "tile");
        if(clickableCollider != null)
        {
            checkFrameNumber = clickableCollider.GetComponent<FrameInfo>().tableIndexNumber;
        }
    }
}
