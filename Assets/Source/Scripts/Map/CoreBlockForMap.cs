using System;
using DG.Tweening;
using KazymiStateMachine;
using KazymiStateMachine.Conditions;
using UnityEngine;
using Random = UnityEngine.Random;


//Чуть поясню почему я добавил именно StateMachine. На опыте скажу что обычно такие обьекты имеют больше 2х состояний, это просто заготовка на будущее
//если в будущем планируется расширение, к примеру добавить новое состояние, это будет сделать в разы прощею. Просто опыт и попытка решить проблемы которые возникали у меня раньше. 
public class CoreBlockForMap : SwipeableMapBlock
{
    [SerializeField] private Animator _blockAnimator;

    private CoreBlockAnimationController _coreBlockAnimationController;
    private State _idleState;
    private State _destroyState;
    private StateMachine _stateMachine;

    public override void Initialize()
    {
        base.Initialize();
        _coreBlockAnimationController = new CoreBlockAnimationController(_blockAnimator);
        InitializeStateMachine();
    }

    private void OnEnable()
    {
        MatchedEvent += OnMatchedHandler;
    }

    private void OnDisable()
    {
        MatchedEvent -= OnMatchedHandler;
    }

    private void OnMatchedHandler(Action onCompleted)
    {
        _stateMachine.SetState(_destroyState);
        DOVirtual.DelayedCall(_coreBlockAnimationController.GetAnimationDuration(CoreBlockAnimationType.Destroy),
            () => onCompleted?.Invoke());
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    private void InitializeStateMachine()
    {
        _idleState = new CoreBlockAnimationState(_coreBlockAnimationController, CoreBlockAnimationType.Idle, false);
        _destroyState =
            new CoreBlockAnimationState(_coreBlockAnimationController, CoreBlockAnimationType.Destroy, false);
        _stateMachine = new StateMachine(_idleState);
    }
}

//это единственный стейт в данной машине, но по похожей логике можно придумать и добавить любой другой.
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