using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Color fillCol;
    public Animator damageAnimation;

    private float _maxStorage = 10;

    // for testing, depending on how it is implemented in game may need to delete
    void Start()
    {
        slider.maxValue = _maxStorage;
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

        damageAnimation.SetTrigger("Damage");

        if(slider.value >= _maxStorage)
        {
            // GAME OVER
            GameManager.Instance.gameStage = GameManager.GameStage.GameOver;
        }
    }

}
