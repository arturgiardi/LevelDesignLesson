using System;
using GEngine.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GEngine.Controller
{
    public abstract class MenuScreen : MonoBehaviour
    {
        [field: SerializeField] private MenuComponentAnimator[] Animations { get; set; }
        [field: SerializeField] private float OpenAnimationTime { get; set; } = 0.2f;
        [field: SerializeField] private float CloseAnimationTime { get; set; } = 0.2f;
        protected MenuData MenuData { get; set; }

        protected void Start()
        {
            MenuData = GetMenuData();
            MenuData.SetMenuScreen(this);

            OnStart();
            for (int i = 0; i < Animations.Length; i++)
                Animations[i].Init();
            Open();
        }

        protected MenuData GetMenuData() =>
           GameScene.Instance.GetManager<MenuManager>().GetMenuData(gameObject.scene.name);

        private void Open()
        {
            if (Animations.Length == 0)
            {
                OnOpen();
                return;
            }

            Show(() => OnOpen());
        }

        public void Show(Action callback = null)
        {
            for (int i = 0; i < Animations.Length; i++)
            {
                if (i == 0)
                    Animations[i].Open(OpenAnimationTime, callback);
                else
                    Animations[i].Open(OpenAnimationTime);
            }
        }

        internal void Close(Action callback)
        {
            OnClose();
            if (Animations.Length == 0)
            {
                UnloadScene(callback);
                return;
            }

            Hide(() => UnloadScene(callback));
        }

        public void Hide(Action callback = null)
        {
            for (int i = 0; i < Animations.Length; i++)
            {
                if (i == Animations.Length - 1)
                    Animations[i].Close(CloseAnimationTime, callback);
                else
                    Animations[i].Close(CloseAnimationTime);
            }
        }

        private void UnloadScene(Action callback)
        {
            callback?.Invoke();
            SceneManager.UnloadSceneAsync(gameObject.scene.name);
        }
        protected virtual void OnStart() { }
        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }
    }

    public class MenuData
    {
        public string ScreenName { get; private set; }
        public int CurrentOptionId { get; set; }
        public MenuScreen MenuScreen { get; set; }

        public MenuData(string screenName)
        {
            ScreenName = screenName;
            CurrentOptionId = 0;
            MenuScreen = null;
        }

        public void SetMenuScreen(MenuScreen menuScreen) => MenuScreen = menuScreen;
    }
}

