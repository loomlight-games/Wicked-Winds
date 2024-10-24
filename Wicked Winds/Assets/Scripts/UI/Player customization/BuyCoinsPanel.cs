using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuyCoinsPanel : MonoBehaviour
{
    public event EventHandler<int> PayCoinsEvent;

    /// <summary>
    /// Handles panel activation
    /// </summary>
    public void UpdatePanel (){
        // Flips activation
        gameObject.SetActive(!gameObject.activeSelf);
    }

    /// <summary>
    /// Invokes event to buy coins
    /// </summary>
    /// <param name="coins"></param>
    public void PayCoins(int coins){
        // Invoke event
        PayCoinsEvent?.Invoke(this,coins);
    }
}
