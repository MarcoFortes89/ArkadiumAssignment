using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class AnimationManager
{
    static Dictionary<int, AnimationClip> dictionary = new Dictionary<int, AnimationClip>();

    public static void Initialize()
    {
        foreach (AnimationClip item in Resources.LoadAll("Animations", typeof(AnimationClip)).Where(x => x.name.Contains("Match")))
        {
            int index = Convert.ToInt16(Regex.Match(item.name, @"\d+").Value);
            dictionary.Add(index, item);
        }
    }

    public static void AssignMatchAnimation(Tile t, int multiplier)
    {
        if (!dictionary.ContainsKey(multiplier))
            return;
        AnimatorOverrideController aoc = new AnimatorOverrideController(t.GetComponent<Animator>().runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        foreach (var a in aoc.animationClips)
        {
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, a.name == "Matchx1" ? dictionary[multiplier] : null));
        }
        aoc.ApplyOverrides(anims);
        t.GetComponent<Animator>().runtimeAnimatorController = aoc;
    }
}
