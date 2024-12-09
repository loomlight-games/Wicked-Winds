using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthentificationState : AState
{
    GameObject UI, authentification;
    public override void Enter()
    {
        UI = GameObject.Find("UI");
        authentification = UI.transform.Find("Authentification").gameObject;
        authentification.SetActive(true);
    }

    public override void Exit()
    {
        authentification.SetActive(false);
    }
}
