public class GameplayStateMachineManager : BaseStateMachineManager<GameplayBaseState>
{
    public void OnPausePressed() => CurrentState?.OnPausePressed();
}

public abstract class GameplayBaseState : BaseState
{
    protected GameplayScene Scene { get; set; }
    protected GameplayStateMachineManager Manager { get; set; }
    public GameplayBaseState(GameplayStateMachineManager manager, GameplayScene scene)
    {
        Manager = manager;
        Scene = scene;
    }

    public virtual void OnPausePressed() { }
}

public class GameplayInitState : GameplayBaseState
{
    public GameplayInitState(GameplayStateMachineManager manager, GameplayScene scene) : base(manager, scene)
    {
    }

    public override void Init()
    {
        //checar por eventos de inicialização
        //se tiver executa-los
        Manager.SwapState(new GameplayDefaultState(Manager, Scene));
    }
}

public class GameplayDefaultState : GameplayBaseState
{
    public GameplayDefaultState(GameplayStateMachineManager manager, GameplayScene scene) : base(manager, scene)
    {
    }

    public override void Init()
    {
        Scene.Camera.FollowObject(Scene.Player.transform);
        Scene.Player.EnableControl();
        var enemies = Scene.Enemies;
        foreach (var item in enemies)
            item?.EnableBehaviour();
    }

    public override void OnPausePressed()
    {
        return;
        Scene.OpenPauseMenu();
        Manager.PushState(new GameplayPausedState(Manager, Scene));
    }
}

public class GameplayPausedState : GameplayBaseState
{
    public GameplayPausedState(GameplayStateMachineManager manager, GameplayScene scene) : base(manager, scene)
    {
    }

    public override void Init()
    {
        Scene.Player.Pause();
        var enemies = Scene.Enemies;
        foreach (var item in enemies)
            item?.Pause();
    }

    public override void OnPausePressed()
    {
        Scene.ClosePauseMenu();

        Scene.Player.DisPause();
        var enemies = Scene.Enemies;
        foreach (var item in enemies)
            item?.DisPause();

        Manager.PopState();
    }
}

public class GameplayStandByState : GameplayBaseState
{
    public GameplayStandByState(GameplayStateMachineManager manager, GameplayScene scene) : base(manager, scene)
    {
    }

    public override void Init()
    {
        Scene.Player.DisableGameplay();
        var enemies = Scene.Enemies;
        foreach (var item in enemies)
            item?.DisableBehaviour();
    }
}
