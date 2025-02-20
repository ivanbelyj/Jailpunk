using UnityEngine;
using TigerForge;

public class EnemyB : MonoBehaviour
{
    void Start()
    {
        EventManager.StartListening("PRESSED_G", gameObject, WhoIAmCallBack);
    }

    void WhoIAmCallBack()
    {
        Debug.Log("<color=orange>I'm EnemyB!</color>\n");
    }
}
