using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/// <summary>
/// A feature that the complex may have in some generations,
/// or that may be disabled in other
/// </summary>
public abstract class ComplexFeature : NetworkBehaviour
{
    [SerializeField]
    private bool isEnabled;
    public bool IsEnabled => isEnabled;

    protected Complex complex;

    public void Initialize(Complex complex) {
        this.complex = complex;
    }
}
