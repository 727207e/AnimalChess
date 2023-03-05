using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaeChessPieces : AnimalChessPieces
{
    protected override void InitData()
    {
        canMovePoint.Add((-1, -1));
        canMovePoint.Add((-1, 1));
        canMovePoint.Add((1, 1));
        canMovePoint.Add((1, -1));
    }
}
