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

    public Collider inspectedObject;
    
    private static Vector4 GUIButtons = new Vector4(20, 40, 80, 20); 
    private static Vector4 GUIBox = new Vector4(10, 10, GUIButtons.z + 20, 90);

    public Plane hitplane;
    public GameObject grabbedObj;
    public float grabbedObjDist;
    public bool isGrabbing;
    
    private bool _showGUI;
    private bool _clickedGUI;
    
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
            B.AddComponent<SphereCollider>().enabled = false;
            B.GetComponent<SphereCollider>().isTrigger = true;
            B.GetComponent<SphereCollider>().radius = 0f;
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
        if (_showGUI)
        {
            // Make a background box
            GUI.Box(new Rect(GUIBox.x, GUIBox.y, GUIBox.z, GUIBox.w), "Menu");

            // Make the first button.
            if (NewBox(0, "Edit Bones"))
            {
                viewBones = !viewBones;
                //showGUI = false;
                if (viewBones)
                {
                    Debug.Log("Bones On");

                    foreach (var B in bones)
                    {
                        B.GetComponent<SphereCollider>().enabled = true;
                    }

                    inspectedObject.enabled = false;

                    return;
                }

                Debug.Log("Bones Off");
                foreach (var B in bones)
                {
                    inspectedObject.enabled = true;
                    B.GetComponent<SphereCollider>().enabled = false;
                }
            }

            if (NewBox(1, "Exit"))
            {
                viewBones = false;
                _showGUI = false;
                inspectedObject.enabled = true;
                inspectedObject = null;
                foreach (var B in bones)
                {
                    B.GetComponent<SphereCollider>().enabled = false;
                }
            }
        }
    }
    public bool NewBox(int number, string name)
    {
        return GUI.Button(new Rect(GUIButtons.x,GUIButtons.y + (GUIButtons.w * number), GUIButtons.z,GUIButtons.w), 
            name);
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        bool successfulHit = false;
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (context.started)
        {
            //Debug.Log("mouseX: " + Mouse.current.position.ReadValue().x + "\n" + "mouseY: " + Mouse.current.position.ReadValue().y);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Bone"))
                {
                    grabbedObj = hit.collider.gameObject;
                    isGrabbing = true;
                    if (hit.collider.transform.parent != null)
                    {
                        // hit.collider.transform.parent = null;
                        //var camCam : float = Mathf.Atan2(p2.y-p1.y, p2.x-p1.x)*180 / Mathf.PI;
                        /*var distance = hit.distance;
                    
                        Vector3 mousePos3D = new Vector3(Mouse.current.position.ReadValue().x,
                            Mouse.current.position.ReadValue().y, distance);
                        Vector3 objPos = cam.ScreenToWorldPoint(mousePos3D);
                        hit.collider.gameObject.transform.position = new Vector2(objPos.x,objPos.y);*/
                        //hit.collider.gameObject.transform.SetParent(parent.transform);
                        hitplane = new Plane(cam.transform.forward,hit.collider.transform.position); //create new plane, 
                        //facing up, at the script owner's position
                        //GameObject cursor = GameObject.Find("Cursor");
                    }
                    Debug.Log("Darn");
                }
                
                if (hit.collider.GetComponent<BonesView>())
                {
                    inspectedObject = hit.collider;
                    _showGUI = true;
                    Debug.Log("Ding");
                }
                
                successfulHit = true;
            }
            
            if (successfulHit == false)
            {
                Debug.Log("Dong");
            }
        }

        if (context.canceled)
        {
            isGrabbing = false;
        }
    }
    
    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("BONG");
        }
    }

    //Debug.Log(name + " Game Object Clicked!");

    // Update is called once per frame
    void Update()
    {
        if (isGrabbing)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (hitplane.Raycast(ray, out grabbedObjDist))
            {
               grabbedObj.transform.position = ray.GetPoint(grabbedObjDist); //set the position
            }
        }
    }
}
