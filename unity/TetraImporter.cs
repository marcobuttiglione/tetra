using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;

[ScriptedImporter(1, "tetra")]
public class TetrahedralMeshImporter : ScriptedImporter
{
    /// <summary>
    /// Called when the asset is imported. Reads the file content, parses it into a TetrahedralMeshAsset,
    /// and adds it to the asset import context.
    /// </summary>
    /// <param name="ctx">The asset import context provided by Unity.</param>
    public override void OnImportAsset(AssetImportContext ctx)
    {
        // Read the content of the file at the asset path.
        string text = File.ReadAllText(ctx.assetPath);
        // Split the file content into lines.
        string[] lines = text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);
        // Extract the mesh name from the file name.
        string meshName = Path.GetFileNameWithoutExtension(ctx.assetPath);

        // Parse the lines into a TetrahedralMeshAsset.
        var asset = ParseToTetrahedralMesh(lines, meshName);

        // Add the parsed asset to the import context and set it as the main object.
        ctx.AddObjectToAsset("MainAsset", asset);
        ctx.SetMainObject(asset);
    }

    /// <summary>
    /// Parses the lines of a tetrahedral mesh file into a TetrahedralMeshAsset.
    /// </summary>
    /// <param name="lines">The lines of the file to parse.</param>
    /// <param name="meshName">The name of the mesh.</param>
    /// <returns>A TetrahedralMeshAsset containing the parsed data.</returns>
    private TetrahedralMeshAsset ParseToTetrahedralMesh(string[] lines, string meshName)
    {
        // Check if the input lines are empty and log an error if so.
        if (lines.Length == 0)
        {
            Debug.LogError("TextAsset input is null.");
            return null;
        }

        // List to store vertex coordinates.
        var vertexList = new List<float>();
        // List to store tetrahedral vertex indices.
        var tetraIndices = new List<int>();
        // Set to store unique edges of the tetrahedra.
        var edgeSet = new HashSet<(int, int)>();

        // Iterate through each line in the file.
        foreach (var line in lines)
        {
            // Split the line into tokens.
            var tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) continue;

            // Parse vertex coordinates if the line starts with "v".
            if (tokens[0] == "v")
            {
                for (int i = 1; i <= 3; i++)
                    vertexList.Add(float.Parse(tokens[i], CultureInfo.InvariantCulture));
            }
            // Parse tetrahedral indices if the line starts with "t".
            else if (tokens[0] == "t")
            {
                var faceIndices = new List<int>();
                foreach (var token in tokens[1..])
                {
                    int index = int.Parse(token);
                    tetraIndices.Add(index);
                    faceIndices.Add(index);
                }

                // Add edges of the tetrahedron to the edge set.
                for (int i = 0; i < 4; i++)
                {
                    int a = faceIndices[i];
                    int b = faceIndices[(i + 1) % 4];
                    var edge = (Math.Min(a, b), Math.Max(a, b));
                    edgeSet.Add(edge);
                }
            }
        }

        // Convert the edge set to a list of edge indices.
        var edgeIndices = new List<int>();
        foreach (var (a, b) in edgeSet)
        {
            edgeIndices.Add(a);
            edgeIndices.Add(b);
        }

        // Create a new TetrahedralMeshAsset and populate its fields.
        var asset = ScriptableObject.CreateInstance<TetrahedralMeshAsset>();
        asset.meshName = meshName;
        asset.vertexCoordinates = vertexList.ToArray();
        asset.tetrahedraVertexIndices = tetraIndices.ToArray();
        asset.tetrahedraEdgeIndices = edgeIndices.ToArray();
        return asset;
    }
}