using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MemChip Abilities/MemChip Ability - Compaction")]
public class Ability_Compaction : MemChipAbilitySO
{
    [Header("Ability-Specific Stuff")]

    public int baseMoveSpeed = 5; // Make sure to adjust this to the player's moveSpeed variable set in PlayerMovement.cs

    public override void ExecuteAbility(GameObject player, AbilityController controller)
    {
        if (Input.GetKeyDown(abilityKeybind))
        {
            if (player.tag == "Player")
            {
                player.tag = "Player-Hidden"; // Change tag so enemies do not detect the player

                PlayerMovement movementScript = player.GetComponent<PlayerMovement>();
                movementScript.moveSpeed = 0;
                movementScript.canFlip = false;
            }
            else if (player.tag == "Player-Hidden")
            {
                player.tag = "Player"; // Change tag back so enemies can now detect the player again

                PlayerMovement movementScript = player.GetComponent<PlayerMovement>();
                movementScript.moveSpeed = baseMoveSpeed;
                movementScript.canFlip = true;
            }

            controller.cooldownTimer = cooldownTime;
        }
    }
}
