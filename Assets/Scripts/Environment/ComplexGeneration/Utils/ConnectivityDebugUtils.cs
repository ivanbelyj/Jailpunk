using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class ConnectivityDebugUtils
{
    // Todo: restructure ?
    public static void PrintGraphToConsole(Graph<int> graph)
    {
        if (graph.Nodes.Count == 0)
        {
            Debug.Log("Graph is empty.");
            return;
        }

        var sb = new StringBuilder("Graph structure:\n");
        foreach (var node in graph.Nodes)
        {
            sb.Append($"Node {node.Value}: ");
            foreach (var connectedNode in node.ConnectedNodes)
            {
                sb.Append($"{connectedNode.Value}, ");
            }
            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }

    public static void PrintBoundariesToConsole(List<AreasBoundary> boundaries)
    {
        if (boundaries.Count == 0)
        {
            Debug.Log("No sector boundaries found.");
            return;
        }

        var sb = new StringBuilder("Sector boundaries:\n");
        foreach (var boundary in boundaries)
        {
            sb.AppendLine($"Boundary between sectors {boundary.AreaA} and {boundary.AreaB}:");
            foreach (var tile in boundary.BoundaryTilesA)
            {
                sb.Append($"({tile.x}, {tile.y}) ");
            }
            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }

    public static void AddBoundariesDebugMarks(List<AreasBoundary> boundaries)
    {
        var colors = new[]
        {
            Color.red, Color.green, Color.blue, Color.yellow, 
            Color.magenta, Color.cyan, Color.white
        };

        int colorIndex = 0;

        foreach (var boundary in boundaries)
        {
            var color = colors[colorIndex % colors.Length];
            colorIndex++;

            foreach (var tile in boundary.BoundaryTilesA)
            {
                ComplexGenerator.AddDebugMarkToScheme(tile, color);
            }
            foreach (var tile in boundary.BoundaryTilesB)
            {
                ComplexGenerator.AddDebugMarkToScheme(tile, color);
            }
        }
    }
}
