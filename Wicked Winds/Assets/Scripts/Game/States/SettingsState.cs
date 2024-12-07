using UnityEngine;

public class SettingsState : AState
{

    GameObject Menu, panel, PCyes, PCno;

    public override void Enter()
    {
        Menu = GameObject.Find("Settings menu");
        panel = Menu.transform.Find("Panel").gameObject;
        panel.SetActive(true);

        GameObject playingOnPCbutton = panel.transform.Find("Playing on PC").gameObject;
        PCyes = playingOnPCbutton.transform.Find("Yes").gameObject;
        PCno = playingOnPCbutton.transform.Find("No").gameObject;

        if (GameManager.Instance.playingOnPC)
            PCyes.SetActive(true);
        else
            PCno.SetActive(true);
    }

    public override void Update()
    {
        if (GameManager.Instance.playingOnPC)
        {
            PCyes.SetActive(true);
            PCno.SetActive(false);
        }
        else
        {
            PCyes.SetActive(false);
            PCno.SetActive(true);
        }
    }

    public override void Exit()
    {
        panel.SetActive(false);
    }
}
