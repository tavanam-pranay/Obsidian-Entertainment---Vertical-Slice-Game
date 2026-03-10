using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Playables;
#endif
using UnityEngine;
using UnityEngine.UI;

public class MemChipInventory : MonoBehaviour
{
    public List<MemoryChipSO> memChipList;
    public MemChipAbilitySO selectedViewAbility;
    public ArmSO selectedArm;
    [HideInInspector] public AbilityController abilityController; // Connection established in the AbilityController itself
    public AttachmentBehavior attachmentBehavior;

    [Header("Memory Chip Counts")]
    private int utilChipCount;
    private int mobilChipCount;
    private int combatChipCount;

    [Header("Memory Chip UI Stuff")]
    public TextMeshProUGUI utilChipCountText;
    public TextMeshProUGUI mobilChipCountText;
    public TextMeshProUGUI combatChipCountText;

    [Header("Ability Preview UI")]
    public TextMeshProUGUI abilityNameText;
    public Image abilityIconText;
    public TextMeshProUGUI abilityFlavText;
    public TextMeshProUGUI abilityDescText;
    public GameObject costsList;
    public TextMeshProUGUI utilChipCost;
    public TextMeshProUGUI mobilChipCost;
    public TextMeshProUGUI combatChipCost;
    public TextMeshProUGUI reqsCompleteText;
    public Button unlockButton;

    private void Start()
    {
        chipCount();
    }

    public void chipCount()
    {
        utilChipCount = 0;
        mobilChipCount = 0;
        combatChipCount = 0;

        foreach (MemoryChipSO memChip in memChipList)
        {
            switch (memChip.chipType)
            {
                case ChipType.Utility:
                    utilChipCount++;
                    break;
                case ChipType.Mobility:
                    mobilChipCount++;
                    break;
                case ChipType.Combat:
                    combatChipCount++;
                    break;
            }
        }
    }

    // Called whenever the menu is opened or a change is made to update the UI displays
    public void OnChange()
    {
        utilChipCountText.text = utilChipCount.ToString(); 
        mobilChipCountText.text = mobilChipCount.ToString();
        combatChipCountText.text = combatChipCount.ToString();
    }

    public void ViewAbility(MemChipAbilitySO ability)
    {
        selectedViewAbility = ability;
        selectedArm = null;

        abilityNameText.text = ability.abilityName.ToString();
        abilityIconText.sprite = ability.abilityIcon;
        abilityFlavText.text = ability.abilityFlavText.ToString();
        abilityDescText.text = ability.abilityDesc.ToString();

        utilChipCost.text = ability.utilityChipReq.ToString();
        mobilChipCost.text = ability.mobilityChipReq.ToString();
        combatChipCost.text = ability.combatChipReq.ToString();

        //Check if the player already has the ability
        bool abilityObtained = false;
        for(int i = 0; i < abilityController.memoryChipAbilities.Count; i++)
        {
            if (abilityController.memoryChipAbilities[i] == ability)
            {
                abilityObtained = true;
                break;
            }
        }

        //Check if all requirements to unlock an arm or ability are met (ability has not been unlocked, and all chip requirements are met)
        if (!abilityObtained && utilChipCount >= ability.utilityChipReq && mobilChipCount >= ability.mobilityChipReq && combatChipCount >= ability.combatChipReq)
        {
            reqsCompleteText.gameObject.SetActive(true);
            unlockButton.interactable = true;
        }
        else
        {
            reqsCompleteText.gameObject.SetActive(false);
            unlockButton.interactable = false;
        }
    }

    public void ViewArm(ArmSO arm)
    {
        selectedArm = arm;
        selectedViewAbility = null;

        abilityNameText.text = arm.armName.ToString();
        abilityIconText.sprite = arm.armIcon;
        abilityFlavText.text = arm.armFlavText.ToString();
        abilityDescText.text = arm.armDesc.ToString();

        utilChipCost.text = arm.utilityChipReq.ToString();
        mobilChipCost.text = arm.mobilityChipReq.ToString();
        combatChipCost.text = arm.combatChipReq.ToString();

        //Check if the player already has the arm
        bool armObtained = false;
        if (attachmentBehavior.hasGrabber && arm.armIndexForPlayer == 0)
        {
            armObtained = true;
        }
        else if(attachmentBehavior.hasCannon && arm.armIndexForPlayer == 1)
        {
            armObtained = true;
        }
        else if(attachmentBehavior.hasMagnet && arm.armIndexForPlayer == 2)
        {
            armObtained = true;
        }


        //Check if all requirements to unlock an arm or ability are met (ability has not been unlocked, and all chip requirements are met)
        if (!armObtained && utilChipCount >= arm.utilityChipReq && mobilChipCount >= arm.mobilityChipReq && combatChipCount >= arm.combatChipReq)
        {
            reqsCompleteText.gameObject.SetActive(true);
            unlockButton.interactable = true;
        }
        else
        {
            reqsCompleteText.gameObject.SetActive(false);
            unlockButton.interactable = false;
        }
    }

    public void UnlockAbility()
    {
        if (selectedViewAbility)
        {
            utilChipCount -= selectedViewAbility.utilityChipReq;
            mobilChipCount -= selectedViewAbility.mobilityChipReq;
            combatChipCount -= selectedViewAbility.combatChipReq;

            // Adds the ability to the current list of active abilities
            abilityController.addAbility(selectedViewAbility);
            selectedViewAbility = null;
        }

        if (selectedArm)
        {
            utilChipCount -= selectedArm.utilityChipReq;
            mobilChipCount -= selectedArm.mobilityChipReq;
            combatChipCount -= selectedArm.combatChipReq;

            // Adds the ability to the current list of active abilities
            attachmentBehavior.addArm(selectedArm);
            selectedArm = null;
        }
        unlockButton.interactable = false;
        OnChange();
    }

    public void addChip(MemoryChipSO chip)
    {
        memChipList.Add(chip);
        chipCount();
        OnChange();
    }

    public void Deselection()
    {
        selectedArm = null;
        selectedViewAbility = null;
    }
}
