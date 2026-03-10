using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "MemChip Abilities/MemChip Ability - IFF Ping")]
public class Ability_IFFPing : MemChipAbilitySO
{
    [Header("Ability-Specific Stuff")]

    public GameObject pingPrefab;

    public override void ExecuteAbility(GameObject player, AbilityController controller)
    {
        if (Input.GetKeyDown(abilityKeybind))
        {
            // Calculate rotation of ping "projectile"
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - player.transform.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
         
            // Creates the "ping"
            GameObject ping = Instantiate(pingPrefab, player.transform.position, Quaternion.Euler(0f, 0f, angle - 90f));

            //Set the ping's frequency to the one set via player input in the AbilityController
            IFFPing_Projectile pingCode = ping.GetComponent<IFFPing_Projectile>();
            pingCode.pingFrequency = controller.freqMegahertz;

            Debug.Log("Sent IFF ping!");

            // Sets cooldown time
            controller.cooldownTimer = cooldownTime;
        }

        //Input.mouseScrollDelta.y
    }
}
