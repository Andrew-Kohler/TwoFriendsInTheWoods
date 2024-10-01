using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScrObs/Conversation")]
public class Conversation : ScriptableObject
{
    [TextArea(1, 2)] public List<string> lines;
    public List<bool> bubbleChara1;
    
}
