using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Customer))]
public class CustomerTester : MonoBehaviour
{
    private Customer _customer;
    private SpeciesData _speciesO;
    private GameObject _chairO;
    
    [Header("Data")] 
    [SerializeField] private SpeciesData _species;
    [SerializeField] private GameObject _chair;
    [SerializeField] private Direction _side;
    [SerializeField] private Orientation _orientation;
    
    [Header("Pose Toggle")]
    [SerializeField] private bool _default;
    [SerializeField] private bool _seated;


#if UNITY_EDITOR
    public void Update()
    {
        if (HandleCustomerReference()) return;

        HandleSpecies();
        HandlePositioning();
        
        if (HandleSeated()) return;
        HandleDefault();
    }

    private void HandleSpecies()
    {
        if (_speciesO == _species) return;
        _speciesO = _species;

        if (!_customer) return;
        _customer.Renderer.InitializeCustomerSprites(_species);
    }

    private void HandlePositioning()
    {
        if (!_chair) return;
        if (_chairO == _chair) return;
        _chairO = _chair;

        transform.position = _chair.transform.position + (_orientation == Orientation.Horizontal
            ? _species.ChairOffsetHorizontal
            : _species.ChairOffsetVertical);
    }

    private bool HandleSeated()
    {
        if (!_seated) return false;
        _default = false;
        _customer.Renderer.SpriteRenderer.sprite = _species.SittingSprite;
        if (!_chair) return true;
        _customer.Renderer.SpriteRenderer.flipX = _side == Direction.Left;
        return true;
    }

    private void HandleDefault()
    {
        if (!_default) return;
        _seated = false;
        _customer.Renderer.SpriteRenderer.sprite = _species.FrontSprite;
    }

    private bool HandleCustomerReference()
    {
        if (_customer) return false;
        _customer = GetComponent<Customer>();
        if (!_customer) return true;
        _customer.InitializeInEditorMode();
        return true;
    }
#endif
}
