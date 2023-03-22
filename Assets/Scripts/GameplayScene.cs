using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GEngine.Controller;
using UnityEngine;

public class GameplayScene : GameScene<SceneData>
{
    [field: SerializeField] private GameObject PauseMenu { get; set; }
    [field: SerializeField] private GameEvent InitEvent { get; set; }
    [field: SerializeField] private SpawnPoint[] SpawnPoints { get; set; }
    private DemoTutorial DemoTutorial { get; set; }
    private new GameplaySceneData SceneData => (GameplaySceneData)base.SceneData;
    private new GameManager GameManager => (GameManager)base.GameManager;
    private InputManager InputManager => GameManager.InputManager;
    private GameplayStateMachineManager StateMachine { get; set; }
    public PlayerController Player { get; set; }
    public CameraController Camera { get; set; }
    private List<EnemyController> _enemies;
    public IList<EnemyController> Enemies => _enemies.AsReadOnly();
    private Dictionary<string, ScenePoint> _scenePoints;
    private Dictionary<string, CharacterAI> _charactersAI;
    private CutsceneController CutsceneController { get; set; } = new CutsceneController();
    public new static GameplayScene Instance => (GameplayScene)GameScene.Instance;

    protected override void Init()
    {
        _charactersAI = new Dictionary<string, CharacterAI>();
        //ClosePauseMenu();
        RegisterInputs();
        InitPlayer();
        InitCamera();
        InitEnemies();
        InitScenePoints();
        InitTeleports();
        InitTutorial();
    }

    private void InitTutorial()
    {
        DemoTutorial = FindObjectOfType<DemoTutorial>();
        DemoTutorial.Init(this, GameManager.PlayerData, GameManager.InputManager);
        var tutorialAreas = FindObjectsOfType<TutorialArea>();
        foreach (var item in tutorialAreas)
            item.Init(DemoTutorial, Player.gameObject);
    }

    protected override void OnSceneLoaded()
    {
        InitStateMachine();
        if (InitEvent)
            ExecuteEvent(InitEvent);
    }

    private void RegisterInputs()
    {
        InputManager.RegisterPauseAction(PausePressed);
    }

    private void UnregisterInputs()
    {
        InputManager.UnregisterPauseAction(PausePressed);
    }

    private void InitPlayer()
    {
        Player = FindObjectOfType<PlayerController>();
        var spawnPoint = GetSpawnPoint();
        Player.Init(InputManager, spawnPoint.Position.position, spawnPoint.Direction, GameManager.PlayerData);
        _charactersAI.Add("Player", Player.AI);

    }

    private SpawnPoint GetSpawnPoint()
    {
        if (base.SceneData is GameplaySceneData)
            return SpawnPoints[SceneData.SpawnPointIndex];
        else
            return SpawnPoints[0];
    }

    private void InitCamera()
    {
        Camera = FindObjectOfType<CameraController>();
        Camera.Init(Player.transform);
    }

    private void InitEnemies()
    {
        _enemies = new List<EnemyController>();
        _enemies = FindObjectsOfType<EnemyController>().ToList();
        foreach (var item in _enemies)
        {
            item.Init(Player.gameObject, RemoveEnemy);
            if (item.Id != string.Empty)
                _charactersAI.Add(item.Id, item.AI);
        }
    }

    private void InitScenePoints()
    {
        _scenePoints = new Dictionary<string, ScenePoint>();
        var scenePoints = FindObjectsOfType<ScenePoint>().ToList();
        foreach (var item in scenePoints)
        {
            if (item.Id != string.Empty)
                _scenePoints.Add(item.Id, item);
        }
    }

    private void InitTeleports()
    {
        var teleports = FindObjectsOfType<Teleport>();
        foreach (var item in teleports)
            item.Init(Player.gameObject);
    }

    private void InitStateMachine()
    {
        StateMachine = new GameplayStateMachineManager();
        StateMachine.PushState(new GameplayInitState(StateMachine, this));
    }

    private void RemoveEnemy(EnemyController enemy) => _enemies.Remove(enemy);

    public void ExecuteEvent(GameEvent gameEvent)
    {
        StartCoroutine(_ExecuteEvent(gameEvent));
    }

    private IEnumerator _ExecuteEvent(GameEvent gameEvent)
    {
        DisableGameplay();
        yield return gameEvent.Execute();
        EnableGameplay();
    }

    public CharacterAI GetCharacterAI(string id) => _charactersAI[id];
    public Vector3 GetScenePointPosition(string id) => _scenePoints[id].transform.position;
    private void PausePressed() => StateMachine?.OnPausePressed();
    public void OpenPauseMenu() => PauseMenu.SetActive(true);
    public void ClosePauseMenu() => PauseMenu.SetActive(false);
    public void DisableGameplay() => StateMachine.SwapState(new GameplayStandByState(StateMachine, this));
    public void EnableGameplay() => StateMachine.SwapState(new GameplayDefaultState(StateMachine, this));

    public new void LoadScene(SceneData sceneData, float fadeOutTime = .5f)
    {
        GameManager.PlayerData.CurrentHp = Player.PlayerCharacter.Hp;
        base.LoadScene(sceneData);
    }
    private void OnDestroy()
    {
        UnregisterInputs();
    }
}

public class GameplaySceneData : SceneData
{
    public int SpawnPointIndex { get; set; }
    public GameplaySceneData(string sceneName, int teleportPointIndex, bool changeBgm = false, bool continueBgm = false, AudioClip bgm = null, float fadeInTime = 0.5F) : base(sceneName, changeBgm, continueBgm, bgm, fadeInTime)
    {
        SpawnPointIndex = teleportPointIndex;
    }
}

[System.Serializable]
public class SpawnPoint
{
    [field: SerializeField] public Vector2 Direction { get; set; }
    [field: SerializeField] public Transform Position { get; set; }
}