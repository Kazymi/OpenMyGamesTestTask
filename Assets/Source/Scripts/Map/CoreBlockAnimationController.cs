using UnityEngine;

public class CoreBlockAnimationController
{
    private const float AnimationSpeedMin = 0.7f;
    private const float AnimationSpeedMax = 1.1f;

    private readonly Animator _animator;

    public CoreBlockAnimationController(Animator animator)
    {
        _animator = animator;
    }

    public void Play(CoreBlockAnimationType animationType, bool randomSpeed = false)
    {
        if (_animator == null)
        {
            return;
        }
        _animator.Play(animationType.ToString());
        _animator.speed = randomSpeed ? Random.Range(AnimationSpeedMin, AnimationSpeedMax) : AnimationSpeedMin;
    }

    public float GetAnimationDuration(CoreBlockAnimationType animationType)
    {
        if (_animator == null)
        {
            return 0f;
        }
        var controller = _animator.runtimeAnimatorController;
        if (controller == null)
        {
            return 0f;
        }
        foreach (var clip in controller.animationClips)
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
