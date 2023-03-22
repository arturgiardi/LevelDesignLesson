using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GEngine.Controller
{
    public class NavigationMenuController
    {
        private Stack<MenuData> menuStack = new Stack<MenuData>();
        public MenuData MenuData => menuStack.Peek();
        internal void Init()
        {
            Clear();
        }

        public void Clear()
        {
            menuStack.Clear();
        }

        public void PushMenu(MenuData menuScreen)
        {
            if (menuStack.Count > 0)
                menuStack.Peek().MenuScreen.Close(() => _PushMenu(menuScreen));
            else
                _PushMenu(menuScreen);
        }

        private void _PushMenu(MenuData menuScreen)
        {
            menuStack.Push(menuScreen);
            LoadMenu(menuScreen.ScreenName);
        }

        public void PopMenu()
        {
            if (menuStack.Count == 0)
                throw new InvalidOperationException("A stack de menus está vazia");

            menuStack.Pop().MenuScreen.Close(() =>
            {
                if (menuStack.Count > 0)
                    LoadMenu(menuStack.Peek().ScreenName);
            });
        }

        private static void LoadMenu(string menuScreen)
        {
            SceneManager.LoadScene(menuScreen, LoadSceneMode.Additive);
        }
    }
}