using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BonesView : MonoBehaviour
{
    public bool viewBones;
    public Color handleColour;
    public Transform rootBone;
    public SkinnedMeshRenderer smr;
    public List<Transform> bones;
    public Transform bonePos;
    public Camera cam;

    public Vector2 mousePos;
    
    private static Vector4 GUIButtons = new Vector4(20, 40, 80, 20); 
    private static Vector4 GUIBox = new Vector4(10, 10, GUIButtons.z + 20, 90);
   

    private bool showGUI;
    private bool clickedGUI;

    private int countlol;

    // Start is called before the first frame update
    void OnEnable()
    {
        var color = new Color(1, 0.8f, 0.4f, 1);
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        bones = new List<Transform>(smr.bones);
        rootBone = smr.rootBone;
        bones.RemoveAll(item => !item.CompareTag("Bone"));
        
        Handles.color = Color.magenta;

        foreach (var B in bones)
        {
            var p1 = B.position;
            B.AddComponent<SphereCollider>();
            B.GetComponent<SphereCollider>().isTrigger = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (!viewBones) return;
        foreach (var B in bones)
        {
            if (B.parent == null || !B.parent.CompareTag("Bone"))
                continue;
            //Debug.Log("Ding");
            
            B.GetComponent<SphereCollider>().enabled = viewBones;
            var endpoint = B.parent.position + B.parent.rotation * B.localPosition;

            var p1 = B.position;
            var p2 = endpoint;
            var thickness = 4;
            var handleSize = HandleUtility.GetHandleSize(p1);
                
            Handles.DrawBezier(p1,p2,p1,p2, Color.red,null,thickness);
            Handles.DrawSolidDisc(p1,cam.transform.position,handleSize * .1F);
            B.GetComponent<SphereCollider>().radius = handleSize * .0075F;
        }
    }

    void OnGUI()
    {
        if (showGUI)
        { 
            // Make a background box
            GUI.Box(new Rect(GUIBox.x,GUIBox.y, GUIBox.z,GUIBox.w), "Menu");

            // Make the first button.
            if (GUI.Button(new Rect(GUIButtons.x,GUIButtons.y, GUIButtons.z,GUIButtons.w), 
                "View Bones"))
            {
                viewBones = !viewBones;
                //showGUI = false;
                if (viewBones)
                {
                    Debug.Log("Bones On");
                    return;
                } Debug.Log("Bones Off");
                
            }
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    { 
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (context.performed)
        {
            //Debug.Log("mouseX: " + Mouse.current.position.ReadValue().x + "\n" + "mouseY: " + Mouse.current.position.ReadValue().y);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<BonesView>())
                {
                    showGUI = true;
                    Debug.Log("Ding");
                }
            }
            if (hit.collider == null || !hit.collider.GetComponent<BonesView>())
            {
                Debug.Log("Dong");
            }
            
            //Ray ray = Camera.main.ScreenPointToRay(context.ReadValue<Vector3>());
        }

        /*else if (context.canceled)
        {
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("Ding");

                if (!hit.collider.GetComponent<BonesView>())
                {
                    clickedGUI = false;
                    showGUI = false;
                    //Debug.Log("Dang");

                }
            }
        }*/
        
        //Debug.Log("Dong");
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }

    //Debug.Log(name + " Game Object Clicked!");

    // Update is called once per frame
    void Update()
    {
        foreach (var B in bones)
        {
            
        }
    }
}
