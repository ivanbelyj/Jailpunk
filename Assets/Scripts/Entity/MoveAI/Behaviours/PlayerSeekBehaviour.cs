using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Zor.SimpleBlackboard.Components;

public class PlayerSeekBehaviour : NavBehaviour
{
    private Player player;
    private int lastUpdateNetworkTimeSec = -1;

    protected override void Update() {
        if (isServer) {
            
            if (player == null) {
                player = FindAnyObjectByType<Player>();
            }
            else if ((int)NetworkTime.time != lastUpdateNetworkTimeSec) {
                lastUpdateNetworkTimeSec = (int)NetworkTime.time;

                CurrentDest = player.transform.position;
                base.Update();
            }
        }
    }
}
