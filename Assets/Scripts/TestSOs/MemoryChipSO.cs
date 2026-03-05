using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChipType{
    Utility,
    Mobility,
    Combat
}
[CreateAssetMenu(fileName = "Memory Chip Scriptable Object")]
public class MemoryChipSO : ScriptableObject
{
    public ChipType chipType;
    public string chipName;
    [TextArea(3, 10)] public string chipDesc;
}
