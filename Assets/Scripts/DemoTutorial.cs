using DG.Tweening;
using TMPro;
using UnityEngine;

public class DemoTutorial : MonoBehaviour
{
    [field: SerializeField] private Transform[] TutorialWindows { get; set; }
    [field: SerializeField] private TMP_Text GeneralTutorialText { get; set; }
    [field: SerializeField] private TMP_Text KeyBoardTutorialText { get; set; }
    [field: SerializeField] private TMP_Text JoystickTutorialText { get; set; }
    [field: SerializeField] private TutorialTextData TutorialTextData { get; set; }
    private GameplayScene Scene { get; set; }
    private PlayerData PlayerData { get; set; }
    private InputManager InputManager { get; set; }
    public void Init(GameplayScene scene, PlayerData playerData, InputManager inputManager)
    {
        InitWindows();
        Scene = scene;
        PlayerData = playerData;
        InputManager = inputManager;
    }

    private void InitWindows()
    {
        foreach (var item in TutorialWindows)
        {
            item.gameObject.SetActive(true);
            item.transform.localScale = Vector3.zero;
        }
    }

    public void ShowTutorial(TutorialType type)
    {
        if (PlayerData.PlayerSawTutorial(type))
            return;

        Scene.DisableGameplay();
        OpenTutorialWindow(type);
    }

    private void OpenTutorialWindow(TutorialType type)
    {
        SetTutorialTexts(type);
        for (int i = 0; i < TutorialWindows.Length; i++)
        {
            if (i == 0)
                TutorialWindows[i].DOScale(Vector3.one, .2f).OnComplete(
                    () =>
                    {
                        EnableInputs();
                        PlayerData.CompleteTutorial(type);
                    });
            else
                TutorialWindows[i].DOScale(Vector3.one, .2f);
        }
    }

    private void SetTutorialTexts(TutorialType type)
    {
        var texts = TutorialTextData.GetTutorial(type);
        GeneralTutorialText.text = texts.GeneralText;
        KeyBoardTutorialText.text = texts.KeyboardText;
        JoystickTutorialText.text = texts.JoystickText;
    }

    private void CloseTutorial()
    {
        for (int i = 0; i < TutorialWindows.Length; i++)
        {
            if (i == 0)
                TutorialWindows[i].DOScale(Vector3.zero, .2f).OnComplete(() =>
                {
                    DisableInputs();
                    Scene.EnableGameplay();
                });
            else
                TutorialWindows[i].DOScale(Vector3.zero, .2f);
        }
    }

    private void EnableInputs()
    {
        InputManager.RegisterAttackAction(CloseTutorial);
        InputManager.RegisterInteractionStart(CloseTutorial);
    }

    private void DisableInputs()
    {
        InputManager.UnregisterAttackAction(CloseTutorial);
        InputManager.UnregisterInteractionStart(CloseTutorial);
    }
}

public enum TutorialType
{
    Push = 0,
    Dash = 1,
    Attack = 2
}
