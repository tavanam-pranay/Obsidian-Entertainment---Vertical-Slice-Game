using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MemChipInventory : MonoBehaviour
{
    public List<MemoryChipSO> memChipList;
    public MemChipAbilitySO selectedViewAbility;
    [HideInInspector] public AbilityController abilityController; // Connection established in the AbilityController itself

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

        abilityNameText.text = ability.abilityName.ToString();
        abilityIconText.sprite = ability.abilityIcon;
        abilityFlavText.text = ability.abilityFlavText.ToString();
        abilityDescText.text = ability.abilityDesc.ToString();

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

    public void UnlockAbility()
    {
        utilChipCount -= selectedViewAbility.utilityChipReq;
        mobilChipCount -= selectedViewAbility.mobilityChipReq;
        combatChipCount -= selectedViewAbility.combatChipReq;

        // Adds the ability to the current list of active abilities
        abilityController.addAbility(selectedViewAbility);
        selectedViewAbility = null;
        unlockButton.interactable = false;
        OnChange();
    }

    public void addChip(MemoryChipSO chip)
    {
        memChipList.Add(chip);
        chipCount();
        OnChange();
    }
}
