using System.Collections.Generic;
using UnityEngine;

public class MeshMerger : MonoBehaviour
{
    [SerializeField] List<MeshFilter> meshes;
    [SerializeField] MeshFilter targetMesh;

    [ContextMenu("Merge Meshes")]
    void MergeMeshes()
    {
        var merge = new CombineInstance[meshes.Count];

        for (int i = 0; i < meshes.Count; i++)
        {
            merge[i].mesh = meshes[i].sharedMesh;
            merge[i].transform = meshes[i].transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(merge);
        targetMesh.mesh = mesh;
    }
}
