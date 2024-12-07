using UnityEngine;
using UnityEngine.UI;

public class SettingsState : AState
{

    GameObject Menu, panel, PCyes, PCno;

    Slider musicSlider, effectsSlider;

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

        effectsSlider = panel.transform.Find("Effects slider").GetComponent<Slider>();
        musicSlider = panel.transform.Find("Music slider").GetComponent<Slider>();
    }

    public override void Update()
    {
        effectsSlider.value = SoundManager.Instance.effectsVolume;
        musicSlider.value = SoundManager.Instance.musicVolume;

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
