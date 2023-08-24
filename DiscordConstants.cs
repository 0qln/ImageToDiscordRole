using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToDiscordRoles
{
    /// <summary>
    /// Contains Synchrous function that return discord ui elements. 
    /// </summary>
    public static class DiscordConstants
    {
        private static IWebDriver _driver => Program.Driver;


        public static IWebElement RoleSettingsName() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div/div/div[1]/div/div[2]/div/div[1]/div[2]/div"));

        /// <summary>
        /// Expects to be in the discord role settings and is picking a color.
        /// </summary>
        /// <returns></returns>
        public static IWebElement ColorSetterPopup() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[3]/div/div/div/div[2]/div/input"));

        /// <summary>
        /// Expects to be in the discord role settings.
        /// </summary>
        /// <returns></returns>
        public static IWebElement AddRoleButton() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div/div/div[1]/div/div[1]/div/div[1]/div[2]"));


        /// <summary>
        /// Expects to be on the discord app.
        /// </summary>
        /// <returns></returns>
        public static IWebElement ServerElement() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div/div/nav/ul/div[2]/div[3]"));


        /// <summary>
        /// Expects to be in the discord role settings.
        /// </summary>
        /// <returns></returns>
        public static IWebElement RoleDeletionConfirmation() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[3]/div[2]/div/div/form/div[2]/button[1]"));


        /// <summary>
        /// Expects to be in the discord role settings.
        /// </summary>
        /// <returns></returns>
        public static IWebElement RoleDeletion() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[3]/div/div/div/div[4]/div"));


        /// <summary>
        /// Expects discord to have an unsaved change in the role settings.
        /// </summary>
        /// <returns></returns>
        public static IWebElement SaveButton() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div[2]/div/div/div[2]/button[2]"));


        /// <summary>
        /// Expects to discord to have an unsaved change in the role settings.
        /// </summary>
        /// <returns></returns>
        public static IWebElement SavePopup() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div[2]"));


        /// <summary>
        /// Expects to be in the discord role settings.
        /// </summary>
        /// <returns></returns>
        public static IWebElement ColorPicker() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div/div/div[1]/div/div[2]/div/div[1]/div[5]"))
            .FindElement(By.ClassName("container-1ws606"))
            .FindElement(By.ClassName("customContainer-aOMqLu"))
            .GetFirstChildDiv()
            .GetFirstChildButton();


        /// <summary>
        /// Expects the discord to have a server selected.
        /// </summary>
        /// <returns></returns>
        public static IWebElement ServerHeader() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[1]/nav/div[1]"));


        /// <summary>
        /// Expects the discord the have the server header popup uncollapsed.
        /// </summary>
        /// <returns></returns>
        public static IWebElement ServerHeaderPopup() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[3]/div/div/div/div/div[3]"));


        /// <summary>
        /// Expects the discord the have the server header popup uncollapsed.
        /// </summary>
        /// <returns></returns>
        public static IWebElement ServerHeaderPopupSettings() =>
            ServerHeaderPopup().FindElement(By.Id("guild-header-popout-settings"));


        public static IWebElement QuickSwitcher() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[3]/div[2]/div/div/div/input"));


        public static IWebElement RolesTab() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[1]/div/nav/div/div[3]"));

        public static IWebElement RoleManagerButton() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div/div/div[1]/div/div/div/div/div[2]/div[3]/div/button"));

        public static IWebElement MemberTab() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[1]/div/nav/div/div[27]"));

        public static IWebElement SearchBar() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div[1]/div/div/div/div[2]/div[1]/div[2]/div[2]/div/input"));

        public static IWebElement MemberOptionsButton() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div[1]/div/div/div/div[3]/div[4]/button"));


        public static IWebElement MemberOptionsPopup_Group() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[3]/div/div/div/div[3]/div"));

        public static IWebElement MemberOptionsPopup_Button() =>
            MemberOptionsPopup_Group().FindElements(By.TagName("div"))[1];

        public static IWebElement MemberOptionsPopup_Popup() =>
            MemberOptionsPopup_Group().FindElements(By.TagName("div"))[2];


        /// <summary>
        /// Expects to be in the discord role settings in the members tab with the popup extended and a member selected.
        /// </summary>
        /// <returns></returns>
        public static IWebElement RoleSettingsMemberTabPopupSearchbarConfirm() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[3]/div[2]/div/div/div[3]")).GetFirstChildButton();

        /// <summary>
        /// Expects to be in the discord role settings in the members tab with the popup extended.
        /// </summary>
        /// <returns></returns>
        public static IWebElement RoleSettingsMemberTabPopupSearchbarFirstHit() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[3]/div[2]/div/div/div[2]/div[2]/div/div[2]/div/span"));

        /// <summary>
        /// Expects to be in the discord role settings in the members tab with the popup extended.
        /// </summary>
        /// <returns></returns>
        public static IWebElement RoleSettingsMemberTabPopupSearchbar() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[3]/div[2]/div/div/div[2]/div[1]/div/div/input"));

        /// <summary>
        /// Expects to be in the discord role settings in the members tab.
        /// </summary>
        /// <returns></returns>
        public static IWebElement RoleSettingsMemberTabAdd() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div[1]/div/div[1]/div/div[2]/div/div[1]/div/div/div[3]/button"));

        /// <summary>
        /// Expects to be in the discord role settings in the visuals tab.
        /// </summary>
        /// <returns></returns>
        public static IWebElement RoleSettingsMemberTabFromVisualsTab() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div[1]/div/div[1]/div/div[2]/div/div[1]/div[1]/div[2]/div[4]"));



        /// <summary>
        /// Expects to be in the discord role settings.
        /// </summary>
        /// <returns></returns>
        public static IWebElement RoleSettingsTabBar() =>
            _driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div/div/div[1]/div/div[2]/div/div[1]/div[1]/div[2]"));
    }
}
