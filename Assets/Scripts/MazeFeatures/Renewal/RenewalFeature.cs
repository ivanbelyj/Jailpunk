using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class RenewalFeature : MazeFeature
{
    [SerializeField]
    private RenewalConfig config;
    
    public event Action Renewal;

    [SerializeField]
    private bool autoRenewal;
    public bool AutoRenewal {
        get => autoRenewal;
    }

    [SerializeField]
    /// <summary>
    /// Minimal time between renewals
    /// </summary>
    private float necessaryIntervalToRenewal = 800;
    public float NecessaryInterval => necessaryIntervalToRenewal;

    private double lastRenewalTime;

    /// <summary>
    /// All controlled objects in the maze that can be refreshed
    /// </summary>
    public IEnumerable<IRenewable> Refreshables => maze.Activatables
        .OfType<IRenewable>();

    private void Update() {
        if (AutoRenewal &&
            NetworkTime.time > lastRenewalTime + necessaryIntervalToRenewal) {
            if (ShouldStartRenewal())
                StartRenewal();
        }
    }
    
    public void StartRenewal() {
        Debug.Log("Start renewal");
        if (config.HasFlag(RenewalConfig.RefreshObjects))
            RefreshObjects();
        if (config.HasFlag(RenewalConfig.PassOut))
            PassOut();
        Renewal?.Invoke();

        lastRenewalTime = NetworkTime.time;
    }

    private bool ShouldStartRenewal() {
        bool conditionsByConfiguration = true;
        // Todo
        return conditionsByConfiguration;
    }

    private void RefreshObjects() {
        foreach (IRenewable obj in Refreshables) {
            obj.Renew();
        }
    }

    /// <summary>
    /// Make prisoners lose consciousness
    /// </summary>
    private void PassOut() {
        // Todo:
    }
}
