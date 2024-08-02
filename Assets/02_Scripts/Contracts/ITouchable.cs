public interface ITouchable
{
    public void OnTouch();
    public virtual void OnRelease() { }
    public virtual void OnPush() { }
}