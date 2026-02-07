using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoreBlockAnimationController
{
    private readonly Animator _animator;
    private const float AnimationSpeedMin = 0.7f;
    private const float AnimationSpeedMax = 1.1f;

    public CoreBlockAnimationController(Animator animator)
    {
        _animator = animator;
    }

    public void Play(CoreBlockAnimationType animationType, bool randomSpeed = false)
    {
        _animator.Play(animationType.ToString());
        _animator.speed = randomSpeed ? Random.Range(AnimationSpeedMin, AnimationSpeedMax) : AnimationSpeedMin;
    }

    public float GetAnimationDuration(CoreBlockAnimationType animationType)
    {
        var controller = _animator.runtimeAnimatorController;
        var clips = controller.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationType.ToString())
            {
                return clip.length;
            }
        }
        
        Debug.LogWarning($"Анимация с именем '{animationType}' не найдена!");
        return 0f;
    }
}