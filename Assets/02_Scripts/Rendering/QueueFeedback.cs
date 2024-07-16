using UnityEngine;

public class QueueFeedback : MonoBehaviour
{
    private GameObject _gameObject;
    private bool _visibleO;
    
    [SerializeField] public bool _visible;

    public void OnEnable()
    {
        _gameObject = Instantiate(GameSettings.Data.PRE_Checkmark);
        _gameObject.transform!.SetParent(transform);
        _gameObject.transform.SetPositionAndRotation(transform.position.AddZ(-1), Quaternion.Euler(30, 0, 0));
        _gameObject.transform.SetGlobalScale(GameSettings.Data.CheckmarkScale);
        _gameObject.SetActive(false);
        
    }

    public void Show()
    {
        if (_visible) return;
        _gameObject.SetActive(_visible = true);
    }

    public void Hide()
    {
        if (!_visible) return;
        _gameObject.SetActive(_visible = false);
    }

    public void Update()
    {
        HandleStateCheck();
    }

    private void HandleStateCheck()
    {
        if (_visible) 
            Show();
        else 
            Hide();
    }

    public void OnDisable()
    {
        if (_gameObject is not null)
            Destroy(_gameObject);
    }
}