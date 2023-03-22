using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [field: SerializeField] private string SceneToTeleport { get; set; }
    [field: SerializeField] private int TeleportSpawnPoint { get; set; }
    private GameObject Player { get; set; }
    public void Init(GameObject player)
    {
        Player = player;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Player)
        {
            GameplayScene.Instance.DisableGameplay();
            GameplayScene.Instance.LoadScene(new GameplaySceneData(SceneToTeleport, TeleportSpawnPoint));
        }
    }
}
