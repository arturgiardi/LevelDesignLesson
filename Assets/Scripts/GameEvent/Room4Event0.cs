using System.Collections;
using UnityEngine;

public class Room4Event0 : GameEvent
{
    [field: SerializeField] private ClickButton LeftButton { get; set; }
    [field: SerializeField] private ClickButton RightButton { get; set; }
    [field: SerializeField] private Door LeftDoor { get; set; }
    [field: SerializeField] private Door RightDoor { get; set; }
    [field: SerializeField] private GameObject Abyss { get; set; }
    public override IEnumerator Execute()
    {
        var right = GameManager.Instance.PlayerData.Room4RightButtonClicked;
        var left = GameManager.Instance.PlayerData.Room4LeftButtonClicked;
        if (left)
        {
            LeftDoor.InstantOpen();
            LeftButton.SetPressed();
        }

        if (right)
        {
            RightDoor.InstantOpen();
            RightButton.SetPressed();
        }

        Abyss.SetActive(!right || !left);
        yield break;
    }
}