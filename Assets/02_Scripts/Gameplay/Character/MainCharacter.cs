using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainCharacter : Singleton<MainCharacter>, ITouchable
{
    private readonly List<GameObject> _activeQuestionMarks = new();
    private Transform _target;
    private Vector3 _maxPosition;
    private Vector3 _minPosition;
    private SpriteRenderer _renderer;
    private GameObject[] _questionMarks;
    private bool _running;

    private readonly Dictionary<Guid, (Transform Target, Action Callback)> _queue = new();

    [Header("Sprites")] [SerializeField] private Sprite _side;
    [SerializeField] private Sprite _back;
    [SerializeField] private Sprite _front;

    [Header("Move Animation")] [Tooltip("A conversion multiplier from UU (unity units) to t (time in ms). UU * x / 1000 = t")] [SerializeField]
    private float _distanceToDurationMod;

    [SerializeField] private AnimationInterpolation _interpolation;
    [SerializeField] private float _confirmationDistanceThreshold;

    [Header("Bounds")] [SerializeField] private float _maxX;
    [SerializeField] private float _minX;
    [SerializeField] private float _gizmoY;

    public void OnEnable()
    {
        _maxPosition = transform.TransformPoint(new Vector3(_maxX, 0, 0));
        _minPosition = transform.TransformPoint(new Vector3(_minX, 0, 0));
        _renderer = this.GetRequiredComponent<SpriteRenderer>();
        _renderer.sprite = _front;
    }

    public void OnTouch() => EasterEgg();

    public void MoveTo(Transform target, Action callback)
    {
        var id = Guid.NewGuid();
        _queue.Add(id, (target, callback));

        var distance = Mathf.Abs(target.transform.position.x - transform.position.x);
        if (distance < _confirmationDistanceThreshold && _queue.Count == 1) return;

        AddQueueFeedback(target);
    }

    private static void AddQueueFeedback(Transform target)
    {
        var queueFeedback = target.GetComponentInChildren<QueueFeedback>();
        if (!queueFeedback) return;
        queueFeedback.Show();
    }

    public void Update()
    {
        if (_queue.Count > 0 && !_running)
            StartAnimation(_queue.First().Key);
    }

    private void StartAnimation(Guid id)
    {
        _running = true;
        _target = _queue[id].Target;
        var start = transform.position.x;
        var end = ClampTargetPosition(_target).x;
        var distance = Mathf.Abs(end - start);
        var duration = distance * _distanceToDurationMod / 1000;

        AnimationBuilder
            .CreateNew(start, end, duration)
            .SetInterpolation(_interpolation)
            .OnUpdate(OnAnimationTick)
            .OnComplete(() => OnAnimationComplete(id))
            .SetPlayOnce()
            .Build()
            .Start();
    }

    private void OnAnimationTick((float c, float t) value)
    {
        transform.SetGlobalPositionX(value.c);
        _renderer.sprite = _side;
        _renderer.flipX = _target.position.x > transform.position.x;
    }

    private void OnAnimationComplete(Guid id)
    {
        _renderer.sprite = _target.position.z > transform.position.z ? _back : _front;
        _renderer.flipX = false;
        RemoveQueueFeedback(id);
        _queue[id].Callback.Invoke();
        _queue.Remove(id);
        _running = false;
    }

    private void RemoveQueueFeedback(Guid id)
    {
        var queueFeedback = _queue[id].Target.GetComponentInChildren<QueueFeedback>();
        if (!queueFeedback) return;
        queueFeedback.Hide();
    }


    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        var p1 = new Vector3(_minX, _gizmoY, 0);
        var p2 = new Vector3(_maxX, _gizmoY, 0);
        Gizmos.DrawLine(transform.TransformPoint(p1), transform.TransformPoint(p2));
    }


    private Vector3 ClampTargetPosition(Transform target)
    {
        var cx = Mathf.Min(Mathf.Max(target.position.x, _minPosition.x), _maxPosition.x);
        return target.position.SetX(cx);
    }

    #region Dont mind me down there...

    private void EasterEgg()
    {
        var questionMark = this
            .GetChildren()
            .First()
            .GetChildren()
            .Where(x => !_activeQuestionMarks.Contains(x))
            .GetRandom();

        if (questionMark is null) return;

        _activeQuestionMarks.Add(questionMark);
        questionMark.transform.localScale = Vector3.zero;
        questionMark.SetActive(true);

        MoveIn();
        return;

        void MoveIn()
        {
            AnimationBuilder
                .CreateNew(0, 1, 0.2F)
                .OnUpdate(x => questionMark.transform.localScale = new Vector3(x.c, x.c, x.c))
                .SetInterpolation(AnimationInterpolation.EaseInQuart)
                .SetPlayOnce()
                .OnComplete(MoveOut)
                .Build()
                .Start();
        }

        void MoveOut()
        {
            AnimationBuilder
                .CreateNew(1, 0, 0.2F)
                .SetPlayOnce()
                .SetInterpolation(AnimationInterpolation.EaseInQuart)
                .OnUpdate(x => questionMark.transform.localScale = new Vector3(x.c, x.c, x.c))
                .OnComplete(OnComplete)
                .Build()
                .Start();
        }

        void OnComplete()
        {
            _activeQuestionMarks.Remove(questionMark);
            questionMark.SetActive(false);
        }

        #endregion
    }
}