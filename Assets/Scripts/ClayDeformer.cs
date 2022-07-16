using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayDeformer : MonoBehaviour
{
    public Transform rootBone;
    public SkinnedMeshRenderer smr;
    public List<Transform> bones;
    public Transform bonePos;
    public List<float> _boneStartingDistances;

    // Start is called before the first frame update
    void OnEnable()
    {
        var color = new Color(1, 0.8f, 0.4f, 1);
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        bones = new List<Transform>(smr.bones);
        _boneStartingDistances = new List<float>();
        rootBone = smr.rootBone;
        bones.RemoveAll(item => !item.CompareTag("Bone"));

        foreach (var bone in bones)
        {
            for (int i = 0; i < bone.childCount; i++)
            {
                _boneStartingDistances.Add(Vector3.Distance(bone.transform.position,
                    bone.GetChild(i).transform.position));

            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        foreach (var Bone in bones)
        {
            for (int i = 0; i < Bone.childCount; i++)
            {
                if (Vector3.Distance(Bone.transform.position,
                    Bone.GetChild(i).transform.position) > _boneStartingDistances[i]
                    && Bone.GetChild(i).transform.localScale.x > 0)
                {
                    
                    var oldDist = _boneStartingDistances[i];
                    var newDist = Vector3.Distance(Bone.transform.position, 
                        Bone.GetChild(i).transform.position);
                    
                    var scale = Bone.GetChild(i).transform.localScale;
                    var pScale = Bone.parent.transform.localScale;
                    
                    scale = new Vector3(oldDist/newDist,oldDist/newDist,oldDist/newDist);
                    pScale = new Vector3(scale.x * .75F, scale.y * .75F, scale.z * .75F);
                    Bone.GetChild(i).transform.localScale = scale;
                }
            }
        }
    }
}
