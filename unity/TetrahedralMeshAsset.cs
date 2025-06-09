using UnityEngine;

/// <summary>
/// Represents a tetrahedral mesh asset that can be created and used in Unity.
/// </summary>
[CreateAssetMenu(fileName = "NewTetrahedralMesh", menuName = "Mesh/TetrahedralMesh")]
public class TetrahedralMeshAsset : ScriptableObject
{
    /// <summary>
    /// The name of the mesh.
    /// </summary>
    public string meshName;

    /// <summary>
    /// An array of vertex coordinates for the mesh.
    /// Each vertex is represented by three consecutive float values (x, y, z).
    /// </summary>
    public float[] vertexCoordinates;

    /// <summary>
    /// An array of indices representing the vertices of tetrahedra in the mesh.
    /// Each tetrahedron is defined by four consecutive indices.
    /// </summary>
    public int[] tetrahedraVertexIndices;

    /// <summary>
    /// An array of indices representing the edges of tetrahedra in the mesh.
    /// Each edge is defined by two consecutive indices.
    /// </summary>
    public int[] tetrahedraEdgeIndices;
}