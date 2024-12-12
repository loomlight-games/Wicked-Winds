using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialState : AState
{
    GameObject Menu, panel;
    public override void Enter()
    {
        Menu = GameObject.Find("TutorialPanel");
        panel = Menu.transform.Find("Panel").gameObject;
        panel.SetActive(true);

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
