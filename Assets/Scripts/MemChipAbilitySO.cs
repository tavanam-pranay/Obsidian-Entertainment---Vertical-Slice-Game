using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MemChip Ability Scriptable Object")]
public class MemChipAbilitySO : ScriptableObject
{
    public string abilityName;
    [TextArea(3, 10)] public string abilityFlavText;
    [TextArea(3, 10)] public string abilityDesc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
