using System.Collections;
using UnityEngine;

public class Room2Event0 : GameEvent
{
    [field: SerializeField] private CameraController CameraController { get; set; }
    [field: SerializeField] private Door Door { get; set; }
    [field: SerializeField] private Transform Player { get; set; }
    public override IEnumerator Execute()
    {
        CameraController.SetCameraFree();
        CameraController.GoTo(Door.transform.position, 1.5f, null);
        yield return new WaitForSeconds(1.7f);
        Door.Open();
        yield return new WaitForSeconds(Door.AnimationTime + .5f);
        CameraController.GoTo(Player.transform.position, 1.5f, null);
        GameManager.Instance.PlayerData.Room2DoorOpen = true;
    }
}