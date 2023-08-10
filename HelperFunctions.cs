using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToDiscordRoles
{
    public static class HelperFunctions
    {
        private static IWebDriver _driver => Program.Driver;


        public static IWebElement GetOnlyChild(IWebElement parent, By by)
            => parent.FindElements(by).First();

        public static IEnumerable<IWebElement> GetDivChild(IWebElement parent, Func<IWebElement, bool> predicate) 
            => parent.FindElements(By.TagName("div")).Where(predicate);


        public static IWebElement GetRole(string name)
            => GetAllRoles().Result.Where(item => GetNameOfRole(item) == name).First();

        public static string GetNameOfRole(IWebElement role)
            => role.GetAttribute("aria-label");


        public static IWebElement SelectNewRole()
        {
            return GetAllRoles().Result.Where(item => GetNameOfRole(item) == "neue Rolle").First();
        }
        public static bool TryGetNewRole(out IWebElement? role)
        {
            try
            {
                role = SelectNewRole();
                return true;
            }
            catch
            {
                role = null;
                return false;
            }
        }


        public static bool VerifyLogin(Login login) => new EmailAddressAttribute().IsValid(login);


        public static bool TryGetRole(string name, out IWebElement? role)
        {
            try
            {
                role = GetRole(name);
                return true;
            }
            catch
            {
                role = null;
                return false;
            }
        }

        public static async Task<IEnumerable<IWebElement>> GetAllRoles()
        {
            var roleList = await
            new Force().AcquireAsync(() =>
                _driver.FindElement(By
                .XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div/div/div[1]/div/div[1]/div/div[2]/div[1]")));

            return roleList?.FindElements(By.TagName("div"))
                ?? throw new NullReferenceException("There are no roles on this server.");
        }



        /// <summary>
        /// Try no to use these functions. Use a Force instead.
        /// </summary>
        public static void WaitForUI() => Thread.Sleep(WaitForUITime);
        /// <summary>
        /// Try no to use these functions. Use a Force instead.
        /// </summary>
        public static void WaitForUI(int milliseconds) => Thread.Sleep(milliseconds);
        public static int WaitForUITime { get; set; } = 300;
    }
}
