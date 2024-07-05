using System;
using System.Collections;
using UnityEngine;

public class CustomerPoisonRenderer : Singleton<CustomerPoisonRenderer>
{
    public const string ANIMATION_CLIP_NAME = "< Custom Animation >";
    
    private bool _playing;
    private Customer _poisonedCustomer;
    private Table _neighborTable;
    private Item _poisonCloud;
    private Transform _poisonTransform;
    private GameObject _follower;

    [Header("Customer Poison Animation")]
    [SerializeField] [Range(0.001F, 1000F)] private float _animationSpeed = 1.0F;

    public event EventHandler MovingStarted;
    public event EventHandler MovingEnded;
    public event EventHandler PoisonHidden;

    public override void Awake()
    {
        base.Awake();
        
        _poisonCloud = new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.PoisonCloud)));
    }
    
    public void StartPoisonAnimation(Customer poisonedCustomer)
    {
        if (_playing) return;
        _playing = true;

        _poisonedCustomer = poisonedCustomer;
        _neighborTable = poisonedCustomer.Table.NeighbourTable;
        
        StartAnimation();
    }

    private void StartAnimation()
    {
        var startPosition3D =  _poisonedCustomer.Table.transform.position;
        var targetPosition3D = _neighborTable.transform.position;
        var startPosition2D = Raycaster.Instance.Get2DPositionFrom3D(startPosition3D); 
        var targetPosition2D = Raycaster.Instance.Get2DPositionFrom3D(targetPosition3D);
        _follower = Instantiate(GameSettings.Data.PRE_RectTransform);
        var rectTransform = _follower.GetComponent<RectTransform>();
        rectTransform.position = startPosition2D;
        _poisonCloud.Show().Follow(_follower);

        var moveAnimation = CreateMoveAnimation(startPosition2D, targetPosition2D);
        var asyncOperation = StartMoveAnimationInternal(moveAnimation);
        StartCoroutine(asyncOperation);
    }

    private IEnumerator StartMoveAnimationInternal(Animation moveAnimation)
    {
        MovingStarted?.Invoke(this, EventArgs.Empty);
        moveAnimation.Play(ANIMATION_CLIP_NAME);
        yield return new WaitWhile(() => moveAnimation.isPlaying && moveAnimation.isActiveAndEnabled);
        OnMoveAnimationFinished();
    }

    private Animation CreateMoveAnimation(Vector2 startPosition2D, Vector2 targetPosition2D)
    {
        var clip = new AnimationClip { legacy = true };
        var speed = 1.0F / (_animationSpeed <= 0 ? 1.0F : _animationSpeed); // 1 / 2 = 0.5  (ex.: 2 speed mod => double time => faster)  
        var xCurve = AnimationCurve.EaseInOut(0, startPosition2D.x, speed, targetPosition2D.x);
        var yCurve = AnimationCurve.EaseInOut(0, startPosition2D.y, speed, targetPosition2D.y);
        clip.SetCurve("", typeof(RectTransformWrapper), $"{nameof(RectTransformWrapper.X)}", xCurve);
        clip.SetCurve("", typeof(RectTransformWrapper), $"{nameof(RectTransformWrapper.Y)}", yCurve);
        var followerAnimation = _follower.AddComponent<Animation>();
        followerAnimation.AddClip(clip, ANIMATION_CLIP_NAME);
        return followerAnimation;
    }

    private void OnMoveAnimationFinished()
    {
        _poisonCloud.StopFollowing();
        _neighborTable = null;
        _poisonedCustomer = null;
        Destroy(_follower);
        MovingEnded?.Invoke(this, EventArgs.Empty);
        StartCoroutine(nameof(HidePoison));
    }

    private IEnumerator HidePoison()
    {
        yield return new WaitForSeconds(GameSettings.Data.PoisonHideDelay);
        _poisonCloud.Hide();
        PoisonHidden?.Invoke(this, EventArgs.Empty);
        _playing = false;
    }
}