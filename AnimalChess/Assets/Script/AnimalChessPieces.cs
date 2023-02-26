using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AnimalChessPieces : MonoBehaviour
{


    public virtual void MovePieces(Vector3 targetPosition)
    {
        transform.localPosition = targetPosition;
    }

    public virtual void ShowPossibleMovePosition()
    {

    }

    protected void EndMove()
    {

    }

    private void CatchPieces()
    {

    }

    private void SpawnPieces()
    {

    }
}
