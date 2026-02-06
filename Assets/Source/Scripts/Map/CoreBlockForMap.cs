using System;
using KazymiStateMachine;
using KazymiStateMachine.Conditions;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoreBlockForMap : SwipeableMapBlock
{
    [SerializeField] private Animator _blockAnimator;

    private CoreBlockAnimationController _coreBlockAnimationController;
    private State _idleState;
    private State _destroyState;
    private StateMachine _stateMachine;

    private void Awake()
    {
        _coreBlockAnimationController = new CoreBlockAnimationController(_blockAnimator);
        InitializeStateMachine();
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    private void InitializeStateMachine()
    {
        _idleState = new CoreBlockAnimationState(_coreBlockAnimationController, CoreBlockAnimationType.Idle,false);
        _destroyState = new CoreBlockAnimationState(_coreBlockAnimationController, CoreBlockAnimationType.Destroy,false);
        _stateMachine = new StateMachine(_idleState);
    }
}

public class CoreBlockAnimationState : State
{
    private readonly CoreBlockAnimationController _animationController;
    private readonly CoreBlockAnimationType _animationType;
    private readonly bool _randomAnimationSpeed;

    public CoreBlockAnimationState(CoreBlockAnimationController animationController,
        CoreBlockAnimationType animationType, bool randomAnimationSpeed = true)
    {
        _animationController = animationController;
        _animationType = animationType;
        _randomAnimationSpeed = randomAnimationSpeed;
    }

    public override void OnStateEnter()
    {
        _animationController.Play(_animationType, _randomAnimationSpeed);
    }
}