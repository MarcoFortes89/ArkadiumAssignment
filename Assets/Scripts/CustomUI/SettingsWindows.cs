using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindows : MonoBehaviour
{
    ParameterSlider sfx_slider, music_slider;

    private void Start()
    {
        sfx_slider = transform.Find("SfxParameter/Slider").GetComponent<ParameterSlider>();
        music_slider = transform.Find("MusicParameter/Slider").GetComponent<ParameterSlider>();
        sfx_slider.GetComponent<Slider>().value=Core.Audio.SfxVolume;
        music_slider.GetComponent<Slider>().value = Core.Audio.MusicVolume;
        sfx_slider.OnSliderChange += EventSfxChange;
        music_slider.OnSliderChange += EventMusicChange;
    }

    private void EventMusicChange(object sender, EventArgs e)
    {
        Core.Audio.MusicVolume = (float)sender;
    }

    private void EventSfxChange(object sender, EventArgs e)
    {
        Core.Audio.SfxVolume = (float)sender;
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
