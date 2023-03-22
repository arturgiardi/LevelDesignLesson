using DG.Tweening;
using GEngine.Controller;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    [field: SerializeField] private Button RetryButton { get; set; }
    [field: SerializeField] private Button ResetButton { get; set; }
    [field: SerializeField] private Button QuitGameButton { get; set; }
    [field: SerializeField] private Transform GameOverWindow { get; set; }

    private GameplayScene _scene;
    public void Init(GameplayScene scene)
    {
        _scene = scene;
        RetryButton.onClick.AddListener(() => Retry());
        ResetButton.onClick.AddListener(() => ResetGame());
        QuitGameButton.onClick.AddListener(() => QuitGame());
        InitWindows();
    }

    private void InitWindows()
    {
        GameOverWindow.gameObject.SetActive(true);
        GameOverWindow.localScale = Vector3.zero;
    }

    public void Show()
    {
        GameOverWindow.DOScale(Vector3.one, .2f);
    }

    private void QuitGame()
    {
        Debug.Log("Click");
        DisableButtons();
        Application.Quit();
    }

    private void Retry()
    {
        Debug.Log("Click");
        DisableButtons();
        _scene.Retry();
    }

    private void ResetGame()
    {
        Debug.Log("Click");
        DisableButtons();
        _scene.LoadScene(new SceneData("Init"));
    }

    private void DisableButtons()
    {
        RetryButton.interactable = false;
        ResetButton.interactable = false;
        QuitGameButton.interactable = false;
    }
}
