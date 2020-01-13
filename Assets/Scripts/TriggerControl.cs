using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    public PlayerController Player;

    void OnTriggerEnter(Collider other)
    {
        Player.isOnGround = true;
    }
    void OnTriggerExit(Collider other)
    {
        Player.isOnGround = false;
    }
}
