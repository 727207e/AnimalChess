using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigChessPieces : AnimalChessPieces
{
    protected override void InitData()
    {
        canMovePoint.Add((-1, 0));
        canMovePoint.Add((0, -1));
        canMovePoint.Add((0, 1));
        canMovePoint.Add((1, 0));
    }
}
