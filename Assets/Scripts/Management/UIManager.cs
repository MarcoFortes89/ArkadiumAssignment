using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Dictionary<int, Sprite> multipliers = new Dictionary<int, Sprite>();
    TextMeshProUGUI Multiplier,Timer,Score;
    Image fillMultiplier;
    GameObject fill,pausepanel;
    TimeSpan alarm = TimeSpan.FromSeconds(5);

    private void Start()
    {
        Multiplier=transform.Find("Multiplier").GetComponent<TextMeshProUGUI>();
        Timer=transform.Find("TimerPanel/Timer").GetComponent<TextMeshProUGUI>();
        Score = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        pausepanel = transform.Find("Pause").gameObject;
        MatchManager matchManager = FindObjectOfType<MatchManager>();
        matchManager.OnMultiplierChange += EventMultiplierChanged;
        matchManager.OnSecondChange += EventSecondsChange;
        matchManager.OnScoreUpdated += EventScoreUpdated;
        matchManager.OnMultiplierDecay += EventMultiplierDecay;
        matchManager.OnPause += EventPauseClicked;
        Multiplier.text = "x1";
        fill = transform.Find("Multiplier/Border").gameObject;
        fillMultiplier = transform.Find("Multiplier/Border/MultiplierBar").GetComponent<Image>();
        foreach (Texture2D item in Resources.LoadAll("Texture", typeof(Texture2D)))
        {
            int index = Convert.ToInt16(Regex.Match(item.name, @"\d+").Value);
            Rect r = new Rect(0, 0, item.width, item.height);
            multipliers.Add(index, Sprite.Create(item, r, new Vector2(0, 0)));
        }
    }

    private void EventPauseClicked(object sender, EventArgs e)
    {
       pausepanel.SetActive(true);
    }

    private void EventMultiplierDecay(object sender, EventArgs e)
    {
        fillMultiplier.fillAmount=(float)sender;
    }

    private void EventScoreUpdated(object sender, EventArgs e)
    {
        Score.text = $"Score: {sender}";
    }

    private void EventSecondsChange(object sender, EventArgs e)
    {
        TimeSpan ts = (TimeSpan)sender;
        string text = ts.Minutes.ToString() + ":" + ts.Seconds.ToString("00");
        if (ts <= alarm)
        {
            Timer.color = Color.red;
            Core.Audio.Play(ESfxType.Timeout);
        }
        Timer.text = text;
    }

    private void EventMultiplierChanged(object sender, EventArgs e)
    {
        Multiplier.text="x"+sender.ToString();
        Multiplier.GetComponent<Animator>().SetTrigger("UpdatedValue");
        int multiplier = (int)sender;
        fill.SetActive(multiplier > 1);
        if(multipliers.ContainsKey(multiplier))
            fillMultiplier.sprite=multipliers[multiplier];
    }
}