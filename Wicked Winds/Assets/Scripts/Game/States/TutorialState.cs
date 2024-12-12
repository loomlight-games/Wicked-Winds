using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialState : AState
{
    GameObject Menu, panel, handleControls;
    public override void Enter()
    {
        Menu = GameObject.Find("TutorialPanel");
        panel = Menu.transform.Find("Panel").gameObject;
        panel.SetActive(true);
        //handleControls = GameObject.Find("Handled controls");
        //handleControls.SetActive(false);
    }
    public override void Update()
    {
        base.Update();
    }
    public override void Exit() 
    {
        panel.SetActive(false);
    }    
}
