using System;
using UnityEngine;

public class CameraStateMachineManager : BaseStateMachineManager<CameraBaseState>
{
    public void Update() => CurrentState?.Update();

    public void GoTo(Vector3 position, float time, Action callback) => CurrentState?.GoTo(position, time, callback);
}

public abstract class CameraBaseState : BaseState
{
    protected CameraObject Camera { get; set; }
    protected CameraStateMachineManager Manager { get; set; }
    public CameraBaseState(CameraObject camera, CameraStateMachineManager manager)
    {
        Camera = camera;
        Manager = manager;
    }

    public virtual void Update() { }

    public virtual void GoTo(Vector3 position, float time, Action callback) { }
}

public class CameraInitState : CameraBaseState
{
    private Transform ObjectToFocus { get; set; }
    public CameraInitState(CameraObject camera, CameraStateMachineManager manager, Transform objectToFocus) : base(camera, manager)
    {
        ObjectToFocus = objectToFocus;
    }
    public override void Init()
    {
        if (ObjectToFocus)
            Camera.SetCameraPosition(ObjectToFocus.position);
        Manager.SwapState(new CameraFollowState(Camera, Manager, ObjectToFocus));
    }
}

public class CameraFreeState : CameraBaseState
{
    public CameraFreeState(CameraObject camera, CameraStateMachineManager manager) : base(camera, manager)
    {
    }

    public override void GoTo(Vector3 position, float time, Action callback)
    {
        Camera.GoTo(position, time, callback);
    }
}

public class CameraFollowState : CameraBaseState
{
    protected Transform ObjectToFollow { get; set; }
    public CameraFollowState(CameraObject camera, CameraStateMachineManager manager, Transform objectToFollow) : base(camera, manager)
    {
        ObjectToFollow = objectToFollow;
    }

    public override void Update()
    {
        Camera.Follow(ObjectToFollow);
    }
}
