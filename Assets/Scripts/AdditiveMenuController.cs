using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GEngine.Controller
{
    public class AdditiveMenuController
    {
        private Dictionary<string, MenuData> _additiveMenus = new Dictionary<string, MenuData>();

        public void Init()
        {
            Clear();
        }

        public void Clear()
        {
            foreach (var item in _additiveMenus)
                item.Value.MenuScreen.Close(null);
            _additiveMenus.Clear();
        }

        public void OpenAdditiveMenu(MenuData menuScreen)
        {
            _additiveMenus.Add(menuScreen.ScreenName, menuScreen);
            SceneManager.LoadScene(menuScreen.ScreenName, LoadSceneMode.Additive);
        }

        public void RemoveAdditiveMenu(string menuId)
        {
            _additiveMenus[menuId].MenuScreen.Close(null);
            _additiveMenus.Remove(menuId);
        }

        public MenuData GetAdditiveMenuData(string screenName)
        {
            if (Contains(screenName))
                return _additiveMenus[screenName];
            else
                throw new InvalidOperationException($"{screenName} não existe");
        }

        public bool Contains(string screenName) => _additiveMenus.ContainsKey(screenName);
    }
}