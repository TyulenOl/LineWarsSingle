public interface IState
{
    public void OnLogic();
    public void OnPhysics();
    public void OnEnter();
    public void OnExit();
}