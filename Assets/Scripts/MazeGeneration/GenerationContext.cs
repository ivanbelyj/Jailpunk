using System.Collections;
using System.Collections.Generic;
using System;

public class GenerationContext
{
    public MazeData MazeData { get; set; }
    public Random Random { get; set; }

    public List<BSPLeaf> BSPLeaves { get; set; }
}
