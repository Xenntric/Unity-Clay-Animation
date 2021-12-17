using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//[CustomEditor(typeof(MeshView))]

//[ExecuteInEditMode]
public class MeshView : MonoBehaviour
{
    
    public SkinnedMeshRenderer SMR;
    public Mesh originalMesh;
    public Mesh clonedMesh;
    
    public Vector3[] vertices;
    public int[] triangles;

    public bool isCloned;
    public Camera cam;
    // Start is called before the first frame update
    void OnEnable()
    {
        
        SMR = GetComponentInChildren<SkinnedMeshRenderer>();
        originalMesh = SMR.sharedMesh;
        clonedMesh = new Mesh(); //2

        clonedMesh.name = "clone of mesh";
        clonedMesh.vertices = originalMesh.vertices;
        clonedMesh.triangles = originalMesh.triangles;
        clonedMesh.normals = originalMesh.normals;
        clonedMesh.uv = originalMesh.uv;

        vertices = clonedMesh.vertices; //4
        triangles = clonedMesh.triangles;
        isCloned = true; //5
        //Debug.Log("Init & Cloned");
    }

    private void OnDrawGizmos()
    {
        foreach (var V in vertices)
        {
            //Handles.DrawSolidDisc(V, cam.transform.position, HandleUtility.GetHandleSize(V)*.05F);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
