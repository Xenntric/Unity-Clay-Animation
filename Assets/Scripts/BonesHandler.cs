using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonesHandler : MonoBehaviour
{
    public Transform rootBone;
    public SkinnedMeshRenderer smr;
    public List<Transform> bones;

    private Quaternion relRot;
    private void OnEnable()
    {
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        bones = new List<Transform>(smr.bones);
        rootBone = smr.rootBone;
        bones.RemoveAll(item => !item.CompareTag("Bone"));
        
    }

    // Update is called once per frame
    void Update()
    {
        updateBonesRot();
    }

    void updateBonesRot()
    {
        foreach (var B in bones)
        {
            if (B.parent != null && B.parent.CompareTag("Bone") && B.parent.childCount > 1)
            {
                var rotateSpeed = 1f;
                relRot = Quaternion.Inverse(B.parent.transform.rotation) * B.transform.rotation;
                Vector3 targetDirection = B.parent.transform.position - B.transform.position;
                Quaternion targetLook =       Quaternion.LookRotation(targetDirection);
                Quaternion childTargetRot =   Quaternion.RotateTowards(B.transform.rotation, targetLook, rotateSpeed*Time.deltaTime);
                Quaternion targetRot =        Quaternion.Inverse(relRot) *childTargetRot;
                B.parent.transform.rotation = Quaternion.RotateTowards(B.parent.transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            }
        }
    }
}
