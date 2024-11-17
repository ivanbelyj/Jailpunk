using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zor.SimpleBlackboard.Components;

public class PlayerSeekBehaviour : NavBehaviour
{
    private Player player;

    protected override void Update() {
        if (isServer) {
            if (player == null) {
                player = FindObjectOfType<Player>();
            } else {
                CurrentDest = player.transform.position;
                base.Update();
            }
        }
    }
}
