using System.Collections;
using UnityEngine;

public class Room2Event1 : GameEvent
{
    [field: SerializeField] private Door Door { get; set; }
    [field: SerializeField] private ClickButton Button { get; set; }
    public override IEnumerator Execute()
    {
        if(GameManager.Instance.PlayerData.Room2DoorOpen)
        {
            Door.InstantOpen();
            Button.SetPressed();
        }
        yield break;
    }
}

