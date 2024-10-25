using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtShopPlayerState : AState
{
    public override void Enter()
    {

    }

    public override void Update()
    {
        // Rotates item
        PlayerManager.Instance.transform.Rotate(0, 360 * PlayerManager.Instance.rotatorySpeedAtShop * Time.deltaTime, 0);
    }

    public override void Exit()
    {
        
    }
}
