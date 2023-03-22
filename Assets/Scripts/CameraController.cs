using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [field: SerializeField] private CameraObject Camera { get; set; }
    private CameraStateMachineManager StateMachineManager { get; set; }

    public void Init(Transform objectToFocus)
    {
        ConfigStateMachine(objectToFocus);
    }

    private void ConfigStateMachine(Transform objectToFocus)
    {
        StateMachineManager = new CameraStateMachineManager();
        StateMachineManager.PushState(new CameraInitState(Camera, StateMachineManager, objectToFocus));
    }

    private void Update()
    {
        StateMachineManager?.Update();
    }

    public void FollowObject(Transform objectToFocus) =>
        StateMachineManager.SwapState(new CameraFollowState(Camera, StateMachineManager, objectToFocus));

    public void SetCameraFree() =>
        StateMachineManager.SwapState(new CameraFreeState(Camera, StateMachineManager));

    public void GoTo(Vector3 position, float time, Action callback)=> StateMachineManager?.GoTo(position, time, callback);
}
