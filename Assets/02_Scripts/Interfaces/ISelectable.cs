public interface ISelectable
{
    public bool Selected { get; }
    public void Select();
    public void Deselect();
}