using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room4Event1 : GameEvent
{
    private enum ButtonType { Left, Right }
    [field: SerializeField] private ButtonType Type { get; set; }
    [field: SerializeField] private ClickButton Button { get; set; }
    [field: SerializeField] private Door Door { get; set; }
    [field: SerializeField] private CameraController CameraController { get; set; }
    [field: SerializeField] private PlayerController Player { get; set; }
    [field: SerializeField] private Tilemap Abyss { get; set; }
    [field: SerializeField] private Transform BridgeCenter { get; set; }

    public override IEnumerator Execute()
    {
        Door.Open();

        if (Type == ButtonType.Left)
            GameManager.Instance.PlayerData.Room4LeftButtonClicked = true;
        else
            GameManager.Instance.PlayerData.Room4RightButtonClicked = true;

        var right = GameManager.Instance.PlayerData.Room4RightButtonClicked;
        var left = GameManager.Instance.PlayerData.Room4LeftButtonClicked;

        if (right && left)
        {
            CameraController.SetCameraFree();
            CameraController.GoTo(BridgeCenter.position, 1.5f, null);
            yield return new WaitForSeconds(1.7f);
            yield return SpawnBridge();
            yield return new WaitForSeconds(1f);
            CameraController.GoTo(Player.transform.position, 1.5f, null);
        }
    }

    private IEnumerator SpawnBridge()
    {
        var duration = 2f;
        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            var alpha = elapsedTime / duration;
            Abyss.color = new Color(Abyss.color.r, Abyss.color.g, Abyss.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Abyss.gameObject.SetActive(false);
    }
}