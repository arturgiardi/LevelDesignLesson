using System;
using System.Collections.Generic;
using GEngine.Manager;
using UnityEngine;

public class GameManager : BaseGameManager
{
    [field: SerializeField] public InputManager InputManager { get; private set; }
    [field: SerializeField] public PlayerData PlayerData { get; private set; } = new PlayerData();
    public static new GameManager Instance => (GameManager)BaseGameManager.Instance;

    protected override void OnInit()
    {
        PlayerData.Init();
    }
}


[System.Serializable]
public class PlayerData
{
    [field: SerializeField] public int MaxHp { get; private set; }
    public int CurrentHp { get; set; }
    public bool Room2DoorOpen { get; set; }
    public bool Room4LeftButtonClicked { get; set; }
    public bool Room4RightButtonClicked { get; set; }
    public bool Room5PuzzleSolved { get; set; }
    public bool Room6PuzzleSolved { get; set; }
    private List<TutorialType> TutorialsCompleted { get; set; } = new List<TutorialType>();

    public void Init()
    {
        FullRecoverHP();
    }

    internal void CompleteTutorial(TutorialType type) => TutorialsCompleted.Add(type);

    internal void FullRecoverHP()
    {
        CurrentHp = MaxHp;
    }

    internal bool PlayerSawTutorial(TutorialType type) => TutorialsCompleted.Contains(type);
}