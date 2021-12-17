using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshInspectorGUI : MonoBehaviour
{
    private BonesView _bonesView;
    
    // Start is called before the first frame update
    void OnGUI ()
    {
        // Make a background box
        GUI.Box(new Rect(10,10,100,90), "Menu");
    
        // Make the first button.
        if(GUI.Button(new Rect(20,40,80,20), "View Bones"))
        {
            Debug.Log("Bones off");
            _bonesView.viewBones = !_bonesView.viewBones;
        }
    
        // Make the second button.
        if(GUI.Button(new Rect(20,70,80,20), "Level 2")) 
        {
            Debug.Log("Dong");
        }
    }

    private void OnDrawGizmos()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
