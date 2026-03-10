using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Arm")]
public class ArmSO : ScriptableObject
{
    public string armName;
    [TextArea(3, 10)] public string armFlavText;
    [TextArea(3, 10)] public string armDesc;
    public Sprite armIcon;
    public int utilityChipReq;
    public int mobilityChipReq;
    public int combatChipReq;
    public int armIndexForPlayer;
}
