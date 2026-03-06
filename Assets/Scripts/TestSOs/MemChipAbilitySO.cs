using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemChipAbilitySO : ScriptableObject
{
    public string abilityName;
    [TextArea(3, 10)] public string abilityFlavText;
    [TextArea(3, 10)] public string abilityDesc;
    public int utilityChipReq;
    public int mobilityChipReq;
    public int combatChipReq;
    public KeyCode abilityKeybind;
    public float cooldownTime;

    public virtual void ExecuteAbility(GameObject player, AbilityController controller){ }
}
