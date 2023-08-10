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
        /// Expects to discord to have an unsaved change in the role settings.
        /// </summary>
        /// <returns></returns>
        public static IWebElement SaveButton() =>
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div[2]/div/div/div[2]/button[2]"));


        /// <summary>
        /// Expects to be in the discord role settings.
        /// </summary>
        /// <returns></returns>
        public static IWebElement ColorPicker() =>
            _driver.FindElement(By.ClassName("colorPicker-1a1lPd"));


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


    }
}
