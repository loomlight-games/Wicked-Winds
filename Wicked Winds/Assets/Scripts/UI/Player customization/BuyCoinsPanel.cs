using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuyCoinsPanel : MonoBehaviour
{
    public event EventHandler<int> PayCoinsEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Handles panel activation
    /// </summary>
    public void UpdatePanel (){
        // This is active -> close
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        // This is not active -> show panel
        else
            gameObject.SetActive(true);
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
