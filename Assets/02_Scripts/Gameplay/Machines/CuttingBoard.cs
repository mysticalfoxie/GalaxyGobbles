using System.Collections;
using UnityEngine;

public class CuttingBoard : Touchable
{
    [Header("Crafting Visualization")] [SerializeField]
    private GameObject _uiGameObject;

    [SerializeField] private RectTransform _canvas;
    [SerializeField] [Range(0.1F, 5.0F)] private float _scale = 1;

    private bool _menuOpened;

    public override void Awake()
    {
        base.Awake();

        new Item(new(this, GameSettings.GetItemMatch(Identifiers.Value.CuttingBoard), true))
            .ForwardTouchEventsTo(this)
            .SetParent(_canvas.transform)
            .SetLocalPosition(Vector2.zero)
            .SetRotation(Vector2.zero)
            .SetScale(new Vector2(_scale, _scale));
    }

    protected override void OnTouch()
    {
        if (TryHandleOpenMenu()) return;
        TryHandleCloseMenu();
    }

    private bool TryHandleOpenMenu()
    {
        if (_menuOpened) return false;

        _uiGameObject.SetActive(true);
        StartCoroutine(nameof(HandleIngredientSelection));

        return _menuOpened = true;
    }

    private void TryHandleCloseMenu()
    {
        if (!_menuOpened) return;

        _uiGameObject.SetActive(false);

        _menuOpened = false;
    }

    private IEnumerator HandleIngredientSelection()
    {
        yield return SelectionSystem.Instance.WaitForIngredientSelection(ingredient =>
        {
            if (ingredient is null || !ingredient.CanBecomePoison)
                TryHandleCloseMenu();
            else
                OnIngredientSelected(ingredient);
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.HSVToRGB(.03F, .7F, .7F);
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }

    private void OnIngredientSelected(ItemData data)
    {
        TryHandleCloseMenu();
        BottomBar.Instance.Inventory.AddPoison(data);
    }
}