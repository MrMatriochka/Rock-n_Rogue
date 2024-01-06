using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    PlayerController player;
    private void Start()
    {
        player = transform.parent.GetComponent<PlayerController>();
    }
    public void RollEnd()
    {
        transform.parent.position += transform.parent.forward*2.75f;
        player.canMove = true;
        player.canRotate = true;
    }
}
