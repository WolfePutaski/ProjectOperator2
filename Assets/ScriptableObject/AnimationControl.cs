using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    [System.Serializable]
    public class FaceType
    {
        [SerializeField] public string faceName;
        [SerializeField] List<string> something;
    }

    [SerializeField] List<FaceType> faceTypes;

    //[SerializeField] List<SCO_FaceType> faceTypes_ScriptableObject;

    public void GetFaceType(int i)
    {
        print(faceTypes[i].faceName);
    }

    public void GetFaceType(string name)
    {
        foreach(FaceType a in faceTypes)
        {
            if(a.faceName==name)
            {
                print(a.faceName);
                break;
            }
        }
    }
}
