using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialState : AState
{
    GameObject menu, panel, howToPlay, contenedorPC, contenedorMovil, contenedor;

    public override void Enter()
    {
        // Buscar los elementos en la jerarquía
        menu = GameObject.Find("TutorialPanel");
        panel = menu.transform.Find("Panel").gameObject;
        howToPlay = panel.transform.Find("ScrollView/LevelPages/HowToPlayPage").gameObject;

        // Encontrar los contenedores específicos
        contenedor = howToPlay.transform.Find("Contenedor").gameObject;
        contenedorPC = contenedor.transform.Find("ContenedorPC").gameObject;
        contenedorMovil = contenedor.transform.Find("ContenedorMovil").gameObject;

        // Activar el panel principal
        panel.SetActive(true);

        // Mostrar el contenedor adecuado según la variable playingOnPC
        if (GameManager.Instance.playingOnPC)
        {
            contenedorPC.SetActive(true);
            contenedorMovil.SetActive(false);
        }
        else
        {
            contenedorPC.SetActive(false);
            contenedorMovil.SetActive(true);
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        // Ocultar el panel completo al salir del estado
        panel.SetActive(false);
    }
}
