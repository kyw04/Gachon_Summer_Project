using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SkinnedMeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class MeshColliderUpdater : MonoBehaviour
{
    // Update is called once per frame

    private SkinnedMeshRenderer renderer;
    private MeshCollider col;
    
    private void Awake()
    {
        renderer = GetComponent<SkinnedMeshRenderer>();
        col = GetComponent<MeshCollider>();
    }

    void Update()
    {
        Mesh bakedMesh = new Mesh();
        col.sharedMesh = bakedMesh;
        col.sharedMesh = renderer.sharedMesh;
    }
}
