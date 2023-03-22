using System;
using UnityEngine;

namespace GEngine.Manager
{
    public class BaseGameManager : Singleton<BaseGameManager>
    {
        [field: SerializeField] public AudioManager AudioManager { get; set; }
        [field: SerializeField] public ScreenFader ScreenFader { get; set; }
        [field: SerializeField] public TimeManager TimeManager { get; set; }

        protected SaveManager SaveManager { get; private set; } = new SaveManager();
        public GameSceneManager GameSceneManager { get; set; } = new GameSceneManager();
        protected override bool DestroyOnLoad => false;

        public void Init()
        {
            OnInit();
        }

        protected virtual void OnInit()
        {
        }
    }
}
