using UnityEngine;
using UnityEngine.UI;

public class HUDBar : MonoBehaviour
{
    Slider slider;

    void Start(){
        slider = GetComponent<Slider>();
    }

    public void SetMaxValue(float maxValue){
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }

    public void SetValue(float value){
        slider.value = value;
    }
}
