using System.Collections;
using UnityEngine;

public class Room5Event1 : MonoBehaviour
{
    [SerializeField] private Turret[] _turrets;
    [field: SerializeField] private HoldButton Button { get; set; }

    public void Execute()
    {
        foreach (var item in _turrets)
        {
            if (Button.IsPressed)
            {
                GameManager.Instance.PlayerData.Room5PuzzleSolved = true;
                item.TurnOff();
            }
            else
            {
                GameManager.Instance.PlayerData.Room5PuzzleSolved = false;
                item.TurnOn();
            }
        }
    }
}