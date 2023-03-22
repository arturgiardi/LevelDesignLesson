using GEngine.Controller;
using UnityEngine;

namespace GEngine.Manager
{
    public class MenuManager : MonoBehaviour
    {
        private NavigationMenuController NavigationMenuManager { get; set; } = new NavigationMenuController();
        private AdditiveMenuController AdditiveMenuManager { get; set; } = new AdditiveMenuController();
        public MenuData DefaultMenuData => NavigationMenuManager.MenuData;


        public void Init()
        {
            NavigationMenuManager.Init();
            AdditiveMenuManager.Init();
        }

        public void Clear()
        {
            NavigationMenuManager.Clear();
            AdditiveMenuManager.Clear();
        }

        public MenuData GetMenuData(string screenName)
        {
            var menuData = NavigationMenuManager.MenuData;
            if (menuData != null && menuData.ScreenName == screenName)
                return menuData;

            return AdditiveMenuManager.GetAdditiveMenuData(screenName);
        }

        public void PushNavigationMenu(MenuData menuScreen)
        {
            NavigationMenuManager.PushMenu(menuScreen);
        }

        public void PopNavigationMenu()
        {
            NavigationMenuManager.PopMenu();
        }

        public void AddAdditiveMenu(MenuData menuScreen)
        {
            AdditiveMenuManager.OpenAdditiveMenu(menuScreen);
        } 
        public void RemoveAdditiveMenu(string menuId) => AdditiveMenuManager.RemoveAdditiveMenu(menuId);
        public bool ContainsAdditiveMenu(string screenName) => AdditiveMenuManager.Contains(screenName);
    }
}