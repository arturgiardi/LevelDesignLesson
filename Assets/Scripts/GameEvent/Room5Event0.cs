using System.Collections;
using UnityEngine;

public class Room5Event0 : GameEvent
{
    [field: SerializeField] private HoldButton Button { get; set; }
    [field: SerializeField] private PushableObject Box { get; set; }
    [SerializeField] private Turret[] _turrets;
    public override IEnumerator Execute()
    {
        if (GameManager.Instance.PlayerData.Room5PuzzleSolved)
        {
            foreach (var item in _turrets)
                item.TurnOff();
            Button.SetPressed();
            Box.transform.position = Button.transform.position;
        }
        yield break;
    }
}
