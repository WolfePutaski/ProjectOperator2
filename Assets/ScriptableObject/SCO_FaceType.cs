using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SCO_FaceType_", menuName = "ScriptableObject_FaceType")]
public class SCO_FaceType : ScriptableObject
{
    [SerializeField] string faceName;
    [SerializeField] List<string> something;
}
