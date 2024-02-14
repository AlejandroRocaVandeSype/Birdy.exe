using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Color fillCol;

    // for testing, depending on how it is implemented in game may need to delete
    void Start()
    {
        slider.maxValue = 10;
        slider.value = 0;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    
    public void setMaxCorruption(int corruption)
    {
        slider.maxValue = corruption;
        slider.value = 1;

        fill.color = gradient.Evaluate(0f);
    }

    public void setCorruption(int corruption)
    {
        slider.value += corruption;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }


    public void IncreaseCorruption()
    {
        setCorruption(1);
    }

}
