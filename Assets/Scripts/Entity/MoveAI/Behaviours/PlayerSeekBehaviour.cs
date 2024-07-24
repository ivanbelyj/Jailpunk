using System.Collections;
using System.Collections.Generic;
using Codice.Client.Common;
using UnityEngine;
using Zor.SimpleBlackboard.Components;

public class PlayerSeekBehaviour : NavBehaviour
{
    private Player player;

    public override void Update() {
        if (player == null) {
            player = FindObjectOfType<Player>();
        } else {
            CurrentDest = player.transform.position;
            base.Update();
        }
    }
}
