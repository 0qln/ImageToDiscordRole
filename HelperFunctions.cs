using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageToDiscordRoles.DiscordConstants;


namespace ImageToDiscordRoles
{
    public static class HelperFunctions
    {
        private static IWebDriver _driver => Program.Driver;



        public static IEnumerable<(IWebElement button, string name)> GetAllRolesInRolePopup()
        {
            foreach (var div in MemberOptionsPopup_Group().FindElements(By.TagName("div"))) 
            {
                if (div.GetAttribute("class") == "item-5ApiZt labelContainer-35-WEd colorDefault-2_rLdz")
                    yield return (div, div.Text);
            }
        }


        public static IWebElement GetFirstChildInput(this IWebElement parent)
            => parent.FindElements(By.TagName("input")).First();

        public static IWebElement GetFirstChildButton(this IWebElement parent)
            => parent.FindElements(By.TagName("button")).First();

        public static IWebElement GetFirstChildDiv(this IWebElement parent)
            => parent.FindElements(By.TagName("div")).First();


        public static IEnumerable<IWebElement> GetDivChild(IWebElement parent, Func<IWebElement, bool> predicate) 
            => parent.FindElements(By.TagName("div")).Where(predicate);


        public static IWebElement GetRole(string name)
            => GetAllRoles().Result.Where(item => GetNameOfRole(item) == name).First();

        public static IWebElement GetRole(string name, int index)
        {
            return GetAllRoles().Result.Where(item => GetNameOfRole(item) == name).ToList()[index];
        }

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


        public static IWebElement EnsureElement(IWebElement element)
        {
            element.MarkForIDExistenceCheck();
            return _driver.FindElement(By.Id(WebElementExtensions.CustomAttribute));
        }


        public static bool VerifyLogin(Login login) => new EmailAddressAttribute().IsValid(login);


        public static bool TryGetRole(string name, out Func<IWebElement?> role)
        {
            IEnumerable<IWebElement> roles;
            int count, index = 0;

            try
            {
                role = () => GetRole(name, index);
                return true;
            }
            catch
            {
                role = null;
                return false;
            }

            do
            {
                try
                {
                    role = () => GetRole(name, index);
                    return true;
                }
                catch
                {
                    roles = GetAllRoles().Result;
                    count = roles.Count();
                    index++;
                    if (index >= count) index = 0;

                    Console.WriteLine(index);

                    role = null;
                }
            }
            while (count > 0);

            return false;
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
