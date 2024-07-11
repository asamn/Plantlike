using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    
    public Slider slider;

    public void SetMaxXP(float xpValue){
        slider.maxValue = xpValue;
        slider.value = 0f;
    }

    public void SetXP(float xpValue){
        slider.value = xpValue;
    }
}
