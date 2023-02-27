using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMoveFieldCheck : MonoBehaviour
{
    public int checkFrameNumber = -1;
    bool isTrigger = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.tag == "tile")
        {
            isTrigger = true;
            checkFrameNumber = other.gameObject.GetComponent<FrameInfo>().tableIndexNumber;
        }
    }

    private void Update()
    {
        if(!isTrigger)
        {
            checkFrameNumber = -1;
        }
    }
}
