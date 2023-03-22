using System.Collections;
using UnityEngine;

public class Room6Event0 : GameEvent
{
    [SerializeField] private HoldButton[] _buttons;
    [SerializeField] private PushableObject[] _boxes;
    [field: SerializeField] private Door Door { get; set; }
    public override IEnumerator Execute()
    {
        if (GameManager.Instance.PlayerData.Room6PuzzleSolved)
        {
            Door.InstantOpen();

            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].SetPressed();
                _boxes[i].transform.position = _buttons[i].transform.position;
            }
        }
        yield break;
    }
}
