using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public List<MemChipAbilitySO> memoryChipAbilities;
      
    public delegate void abilitiesDelegate(GameObject player, AbilityController controller);
    abilitiesDelegate abilitiesToExecute = null;

    public float cooldownTimer;

    [Header("Ability - IFF Ping: Parameters")]
    [Range(1, 50)] public int freqMegahertz;
    public int freqUpLimit = 50;
    public int freqLowLimit = 1;
    public TextMeshProUGUI freqMhzText;
    public GameObject iffPanel;

    [SerializeField] protected MemChipInventory memChipInventory;

    // Start is called before the first frame update
    void Start()
    {
        //Establish a link to the memory chip inventory and abilities
        memChipInventory.abilityController = this;

        //DEBUG ONLY: Adds all the ExecuteAbility functions inside the Ability Scriptable Objects within the list into the delegate
        //foreach (var ability in memoryChipAbilities)
        //{
        //    abilitiesToExecute += ability.ExecuteAbility;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (memoryChipAbilities.Count > 0)
        {
            if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime; // Counts down the cooldown timer in real time
                                                                    //Executes every Ability's ExecuteAbility function every frame
            else abilitiesToExecute.Invoke(this.gameObject, this);
        }

        #region ABILITY - IFF PING-RELEVANT CODE
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            //Debug.Log("Scroll Wheel used!");
            if (scroll > 0)
            {
                if (freqMegahertz < freqUpLimit) freqMegahertz += 1; // INcrement current IFF frequency when scrolling down
            }
            else
            {
                if (freqMegahertz > freqLowLimit) freqMegahertz -= 1; // DEcrement current IFF frequency when scrolling down
            }
            freqMhzText.text = freqMegahertz.ToString(); // Convert new value to string in Megahertz units
        }
        #endregion

        #region ABILITY - XXXXX-RELEVANT CODE 
        #endregion
    }

    public void addAbility(MemChipAbilitySO ability)
    {
        memoryChipAbilities.Add(ability);
        abilitiesToExecute += ability.ExecuteAbility;
        switch (ability.abilityID)
        {
            case 0:
                iffPanel.SetActive(true); break;
        }
    }

    
}
