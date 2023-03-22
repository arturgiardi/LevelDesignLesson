using System;
using System.Collections.Generic;
using GEngine.Manager;
using UnityEngine;

namespace GEngine.Controller
{
    public abstract class GameScene : Singleton<GameScene>
    {
        [field: SerializeField] private AudioClip Bgm { get; set; }
        [field: SerializeField] private List<MonoBehaviour> Managers { get; set; }
        protected SceneData SceneData { get; set; }
        protected GameSceneManager GameSceneManager => GameManager.GameSceneManager;
        protected ScreenFader ScreenFader => GameManager.ScreenFader;
        protected AudioManager AudioManager => GameManager.AudioManager;
        protected BaseGameManager GameManager => BaseGameManager.Instance;
        private Dictionary<Type, MonoBehaviour> ManagerDictionary { get; set; }

        protected void Start()
        {
            SceneData = GameSceneManager.CurrentSceneData;
            CreateManagersDictionary();
            PlayBgm();
            Init();
            FadeIn();
        }
        private void FadeIn()
        {
            ScreenFader.FadeIn(SceneData.FadeInTime, () => OnSceneLoaded());
        }

        private void PlayBgm()
        {
            if (SceneData.ChangeBgm)
                AudioManager.PlayAudio(GameAudioType.Bgm, SceneData.Bgm);
            else
                AudioManager.PlayAudio(GameAudioType.Bgm, Bgm);
        }

        public void LoadScene(SceneData sceneData, float fadeOutTime = .5f)
        {
            GameSceneManager.LoadScene(sceneData, fadeOutTime);
        }

        private void CreateManagersDictionary()
        {
            foreach (var item in Managers)
                ManagerDictionary.Add(item.GetType(), item);
        }

        public T GetManager<T>() where T : MonoBehaviour
        {
            foreach (var item in ManagerDictionary)
            {
                if (item is T)
                    return (T)item.Value;
            }
            throw new InvalidOperationException($"Não foi encontrado um objeto do tipo {typeof(T)}");
        }

        protected virtual void Init() { }
        protected virtual void OnSceneLoaded() { }
        public virtual void OnSceneClose() { }
    }

    public abstract class GameScene<T> : GameScene where T : SceneData
    {
        protected override bool DestroyOnLoad => true;
        protected new T SceneData => (T)base.SceneData;
    }

    public class SceneData
    {
        public string SceneName { get; private set; }
        public float FadeInTime { get; private set; } = .5f;
        public GameScene Scene { get; set; }
        public bool ChangeBgm { get; internal set; }
        public bool ContinueBgm { get; internal set; }
        public AudioClip Bgm { get; internal set; }
        public BaseGameManager GameManager => BaseGameManager.Instance;
        public AudioManager AudioManager => GameManager.AudioManager;


        public SceneData(string sceneName,
            bool changeBgm = false,
            bool continueBgm = false,
            AudioClip bgm = null,
            float fadeInTime = .5f)
        {
            SceneName = sceneName;
            FadeInTime = fadeInTime;
            ChangeBgm = changeBgm;
            ContinueBgm = continueBgm;
            Bgm = bgm;
        }


        public void OnSceneClose()
        {
            Scene?.OnSceneClose();
        }

        public virtual void FadeSound(float fadeTime)
        {
            FadeBgm(fadeTime);
        }

        protected void FadeBgm(float fadeTime)
        {
            if (ContinueBgm)
                return;

            AudioManager.FadeAndStop(GameAudioType.Bgm, fadeTime);
        }
    }
}