using System.Collections;
using UnityEngine;

public class Room6Event1 : GameEvent
{
    [SerializeField] private HoldButton[] _buttons;
    [field: SerializeField] private Door Door { get; set; }

    public override IEnumerator Execute()
    {
        bool canOpen = true;
        foreach (var item in _buttons)
        {
            if (!item.IsPressed)
            {
                canOpen = false;
                break;
            }
        }

        if (canOpen)
        {
            GameManager.Instance.PlayerData.Room6PuzzleSolved = true;
            if(!Door.IsOpen)
                Door.Open();
        }
        else
        {
            GameManager.Instance.PlayerData.Room6PuzzleSolved = false;
            if(Door.IsOpen)
                Door.Close();
        }

        yield break;
    }
}
