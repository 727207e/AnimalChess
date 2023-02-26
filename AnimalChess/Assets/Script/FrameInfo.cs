using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameInfo : MonoBehaviour
{
    public AnimalChessPieces OnThisFramePiecesInfo;

    public bool IsFrameGotPieces()
    {
        if (OnThisFramePiecesInfo != null)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
