using UnityEngine;
using TigerForge;

public class DemoGroupEmitter : MonoBehaviour
{

    void Start()
    {
        Debug.Log("Welcome in TigerForge - Easy Event Manager, version 2.3!\n");
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("<color=green>I'm the Emitter: I emitted <b>ON_ENEMY_SPAWNED</b> <i>event</i> with random data managed by <b>SetDataGroup</b> method.</color>\n");

            var randomNumber = Random.Range(1, 4);
            if (randomNumber == 1) EventManager.SetDataGroup("ON_ENEMY_SPAWNED", randomNumber, "Monster", 1200, 600, false);
            if (randomNumber == 2) EventManager.SetDataGroup("ON_ENEMY_SPAWNED", randomNumber, "Demon", 1400, 1000, true);
            if (randomNumber == 3) EventManager.SetDataGroup("ON_ENEMY_SPAWNED", randomNumber, "Dragon", 2000, 1500, true);
            if (randomNumber == 4) EventManager.SetDataGroup("ON_ENEMY_SPAWNED", randomNumber, "Troll", 2200, 2000, false);

            EventManager.EmitEvent("ON_ENEMY_SPAWNED");
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("<color=green>I'm the Emitter: I emitted <b>ON_ENEMY_KILLED</b> <i>event</i> with some data managed by <b>SetIndexedDataGroup</b> method.</color>\n");

            EventManager.SetIndexedDataGroup(
                "ON_ENEMY_KILLED",
                new EventManager.DataGroup { id = "points", data = 100 },
                new EventManager.DataGroup { id = "coins", data = 250 },
                new EventManager.DataGroup { id = "bonus", data = "Iron Shield" }
                );

            EventManager.EmitEvent("ON_ENEMY_KILLED");
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log("<color=green>I'm the Emitter: I emitted <b>ON_COIN_TAKEN</b> <i>event</i> with value 50 using the <b>EmitEventData</b> method.</color>\n");

            EventManager.EmitEventData("ON_COIN_TAKEN", 50);
        }

    }
}
