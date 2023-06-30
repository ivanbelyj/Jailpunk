using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Area : MonoBehaviour
{
    public event Action<GameObject> OnAreaEntered;
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Trigger enter");
        OnAreaEntered?.Invoke(other.gameObject);    
    }
}
