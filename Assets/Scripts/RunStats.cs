using Nekki.Vector.Core.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunStats : MonoBehaviour
{
    public Text LabelAnimation;

    public string AnimationName { get; private set; }

    private void Clear()
    {
        AnimationName = string.Empty;
        LabelAnimation.text = string.Empty;
    }

    private void Awake()
    {
        Clear();
    }

    private void FixedUpdate()
    {
        if (LevelMainController.current != null)
        {
            UpdateAnimation();
        }
    }

    private void UpdateAnimation()
    {
        ModelHuman player = LevelMainController.current.Location.GetUserModel();
        if (player != null && player.ControllerAnimations != null && player.ControllerAnimations.Animation != null)
        {
            AnimationName = player.ControllerAnimations.CurrentFrame + " - " + player.ControllerAnimations.Animation.Name;
            LabelAnimation.text = AnimationName;
        }
        else
        {
            AnimationName = "null";
            LabelAnimation.text = AnimationName;
        }
    }

}
