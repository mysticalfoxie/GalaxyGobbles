public abstract class SelectableMonoBehaviour : TouchableMonoBehaviour
{
    public bool Selected { get; private set; }
    
    public virtual void Start()
    {
        SelectionSystem.Instance.Register(this);
    }

    public virtual bool IsSelectable() => true;

    public void Select()
    {
        if (Selected) return;
        Selected = true;
        OnSelected();        
    }

    public void Deselect()
    {
        if (!Selected) return;
        Selected = false;
        OnDeselected();
    }

    protected virtual void OnSelected() { }
    protected virtual void OnDeselected() { }
}