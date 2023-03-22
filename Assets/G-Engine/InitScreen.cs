using System.Collections;
using GEngine.Controller;
using UnityEngine;

namespace GEngine.Manager
{
    public class InitScreen : MonoBehaviour
    {
        [field: SerializeField] BaseGameManager GameManager { get; set; }
        [field: SerializeField] private string StartScene { get; set; } = "StartScreen";

        private IEnumerator Start()
        {
            if (BaseGameManager.Instance)
                Destroy(BaseGameManager.Instance.gameObject);

                yield return null;

                var gm = Instantiate(GameManager);
                gm.Init();
                gm.GameSceneManager.LoadScene(new SceneData(StartScene));
        }
    }
}
