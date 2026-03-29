using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PasswordTerminal : TerminalInteract
{
    public TMP_InputField password;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getPassword()
    {
        return password.text;
    }
}
