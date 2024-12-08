using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFeedback
{
    private string[] cloudFeedback = new string[]
    {
        "Cloudy is crying, maybe a man disappointed her.",
        "Looks like Cloudy is sad; maybe someone hurt her feelings.",
        "Cloudy is weeping; did someone break her heart?",
        "Cloudy can't stop crying�she must be really upset.",
        "Cloudy is shedding tears, probably missing someone dear.",
        "Cloudy is very upset "
    };



    public void RandomCloudFeedBack()
    {
        // Elegir un mensaje aleatorio del array
        int randomIndex = Random.Range(0, cloudFeedback.Length);
        string randomMessage = cloudFeedback[randomIndex];
    }
}
