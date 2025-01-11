using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Data related to the complex generation and transferred between
/// generation stages
/// </summary>
public class GenerationContext
{
    public ComplexData ComplexData { get; set; }
    public GenerationData GenerationData => ComplexData.GenerationData;
    public string RootGameObjectName { get; set; } = "ComplexGrid";

    public GenerationSettings Settings { get; set; }
    public GenerationRequest Request { get; set; }
}
