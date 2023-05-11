using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityPrograms : MonoBehaviour
{
    public bool lockRotation = false;
    public bool detectCollisionPlayer = false;
    void LateUpdate()
    {
        if(lockRotation == true)
            transform.rotation = Quaternion.Euler(0, 0, 0); //locks the rotation of attached objects
        else
            return;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (detectCollisionPlayer == true)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                PlayerAI.isGrounded = true;
            }
        }
        else
            return;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (detectCollisionPlayer == true)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                PlayerAI.isGrounded = false;
            }
        }
        else
            return;
    }
}
