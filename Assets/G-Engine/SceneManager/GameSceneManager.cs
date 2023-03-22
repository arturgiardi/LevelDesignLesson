using System;
using GEngine.Controller;
using GEngine.Database;
using UnityEngine.SceneManagement;

namespace GEngine.Manager
{
    [Serializable]
    public class GameSceneManager 
    {
        public SceneData CurrentSceneData { get; private set; }
        private ScreenFader ScreenFader => GameManager.ScreenFader;
        private BaseGameManager GameManager => BaseGameManager.Instance;

        public void LoadScene(SceneData sceneData, float fadeOutTime = .5f)
        {
            CurrentSceneData?.OnSceneClose();
            CurrentSceneData = sceneData;

            CurrentSceneData.FadeSound(fadeOutTime * .75f);

            ScreenFader.FadeOut(fadeOutTime, () =>
            {
                SceneManager.LoadScene(sceneData.SceneName);
            });
        }
    }
}