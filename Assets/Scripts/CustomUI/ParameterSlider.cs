using System;
using UnityEngine;
using UnityEngine.UI;

public class ParameterSlider : MonoBehaviour
{
    public EventHandler OnSliderChange;

    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SliderChange()
    {
        OnSliderChange?.Invoke(slider.value,EventArgs.Empty);
    }
}
