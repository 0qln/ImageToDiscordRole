using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageToDiscordRoles.HelperFunctions;
using static ImageToDiscordRoles.DiscordConstants;

namespace ImageToDiscordRoles
{
    public static class Discord
    {
        private static IWebDriver _driver => Program.Driver;


        /// <summary>
        /// Delete a role.
        /// Expects the discord to be in the role settings.
        /// </summary>
        /// <param name="element"></param>
        private static async Task DeleteRole(IWebElement element)
        {
            try
            {
                // Bring up popup
                new Actions(_driver).ContextClick(element).Perform();

                // Click delete
                var e = (await new Force().AcquireAsync(RoleDeletion));
                Thread.Sleep(100);
                e.Click();

                // Confirm deletion
                (await new Force().AcquireAsync(RoleDeletionConfirmation)).Click();
            }
            catch
            {
                Console.WriteLine("Failed to delete role");
            }
        }

        /// <summary>
        /// Delete a role.
        /// Expects the discord to be in the role settings.
        /// </summary>
        /// <param name="callersElement"></param>
        private static async void DeleteRole(Func<IWebElement> callersElement)
        {
            var element = callersElement.Invoke();

            // Bring up popup
            new Actions(_driver).ContextClick(element).Perform();

            // Click delete
            var e = (await new Force().AcquireAsync(RoleDeletion));
            Thread.Sleep(100);
            e.Click();

            // Confirm deletion
            (await new Force().AcquireAsync(RoleDeletionConfirmation)).Click();
        }


        /// <summary>
        /// Ask the user for login and store it.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Login GatherUserLogin()
        {
            Console.WriteLine("Enter your login: [Email, Password]");

            return new Login(

                Email: Console.ReadLine()
                    ?? throw new ArgumentException("Invalid Email"),

                Password: Console.ReadLine()
                    ?? throw new ArgumentException("Invalid Password"));
        }


        /// <summary>
        /// Navigate the user to the server settings. 
        /// Expects the discord to have a server selected.
        /// TOOD: Check for driver state at the start
        /// </summary>
        public static async Task NavigateToServerSettings()
        {
            // open popup
            (await new Force().AcquireAsync(ServerHeader)).Click();

            // navigate to settings
            (await new Force().AcquireAsync(ServerHeaderPopupSettings)).Click();
        }


        public static async Task StartUp()
        {
            // Reload website until bug is gone
            do _driver.Navigate().GoToUrl("https://discord.com/login");
            while (_driver.FindElements(By.ClassName("appMount-2yBXZl")).Count == 0);

            // Wait for website to load
            await new Force().AcquireAsync(() => _driver.FindElement(By.TagName("div")));
        }

        /// <summary>
        /// Login into discord using <paramref name="login"/>
        /// </summary>
        /// <param name="login"></param>
        public static async Task Login(Login login)
        {
            // email
            _driver.FindElement(By.Name("email")).SendKeys(login.Email);

            // password
            _driver.FindElement(By.Name("password")).SendKeys(login.Password);

            // confirm
            _driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);


            Console.WriteLine("Login succsesful.");

            // wait for website to load
            await new Force().AcquireAsync(ServerElement);
        }


        /// <summary>
        /// Notice: your server must be contained inside of a folder in order for this
        /// method to see it.
        /// </summary>
        /// <param name="serverName"></param>
        public static void NavigateToServer(string serverName)
        {
            new Actions(_driver).KeyDown(Keys.Control).SendKeys("k").KeyUp(Keys.Control).Perform();
            QuickSwitcher().SendKeys(serverName);
            QuickSwitcher().SendKeys(Keys.Enter);
        }


        /// <summary>
        /// Presses the "Save Changes" button and returns when all changes are saved.
        /// Expects discord to have an unsaved change in the role settings.
        /// </summary>
        public static async Task SaveChanges()
        {
            // Save
            try
            {
                (await new Force().AcquireAsync(SaveButton)).Click();
            }
            catch
            {
                Console.WriteLine(
                    "Failed to save changes. There might be other processes waiting for the changes to get saved. " +
                    "Try clicking the `Save Changes` button manually in the UI.");
            }

            // Wait until the changes have been saved
            await new Force().NotAcquireAsync(SavePopup);
        }

        /// <summary>
        /// Expects to be in the role settings
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task CreateRole(string name, string color, string? grantToUser = null)
        {
            // Open the role settings
            (await new Force().AcquireAsync(AddRoleButton)).Click();

            // set name
            var nameElement = (await new Force().AcquireAsync(RoleSettingsName)).GetFirstChildInput();
            // give the ui time to load the text, before checking if the value is still empty
            Thread.Sleep(100);
            while (nameElement.GetAttribute("value") != "")
            {
                nameElement.SendKeys(Keys.Backspace);
                Thread.Sleep(1);
            }
            nameElement.SetValue(name);

            // Open color setter
            (await new Force().AcquireAsync(ColorPicker)).Click();

            // manage colors
            var colorSetterElement = await new Force().AcquireAsync(ColorSetterPopup);
            colorSetterElement.Clear();
            colorSetterElement.SetValue(color);
            colorSetterElement.SendKeys(Keys.Enter);

            //await SaveChanges();

            // grant the role immediatly to a user
            if (grantToUser is not null)
            {
                // Go member tab
                (await new Force().AcquireAsync(RoleSettingsMemberTabFromVisualsTab)).Click();

                // press add
                (await new Force().AcquireAsync(RoleSettingsMemberTabAdd)).Click();

                // Search for user
                (await new Force().AcquireAsync(RoleSettingsMemberTabPopupSearchbar)).SendKeys(grantToUser);

                // select first hit
                (await new Force().AcquireAsync(RoleSettingsMemberTabPopupSearchbarFirstHit)).Click();

                // confirm
                (await new Force().AcquireAsync(RoleSettingsMemberTabPopupSearchbarConfirm)).Click();
            }
        }


        /// <summary>
        /// Expects to be navigated on the role tab.
        /// </summary>
        public static void DeleteEmptyRoles()
        {
            while (TryGetNewRole(out IWebElement? element))
                if (element is not null)
                    DeleteRole(element);
        }
        /// <summary>
        /// Expects to be navigated on the role tab.
        /// </summary>
        public static async Task DeleteAllRolesByName(string? name)
        {
            IWebElement? element;
            while ((element = NextRole(name).Result) is not null)
            {
                await DeleteRole(element);

                // Wait for element to get deleted
                Thread.Sleep(1100);
                //await new Force().NotAcquireAsync(element);
                //await new Force().NotAcquireAsync(new Func<IWebElement>(() => EnsureElement(element)));
            }
        }

        public static void NavigateToRoll(string name, bool inverse) 
            => (!inverse 
                ? GetRole(name)

                : GetAllRoles().Result
                .Where(x => GetNameOfRole(x) != name)
                .First())

                .Click();


        /// <summary>
        /// Navigate to the role tab.
        /// </summary>
        /// <returns></returns>
        public static async Task NavigateRoleTab()
        {
            // Go roles tab
            (await new Force().AcquireAsync(RolesTab)).Click();

            // Go Role manager button
            (await new Force().AcquireAsync(RoleManagerButton)).Click();
        }

        /// <summary>
        /// Assusmes to be in the server settings
        /// </summary>
        public static async Task NavigateMemberTab()
        {
            // Go member tab
            (await new Force().AcquireAsync(MemberTab)).Click();
        }

        /// <summary>
        /// Assumes to be on the member tab
        /// </summary>
        /// <param name="name"></param>
        public static async Task SearchUser(string name)
        {
            // Enter search bar
            (await new Force().AcquireAsync(SearchBar)).SendKeys(name);
        }

        /// <summary>
        /// Assumes to be in a member search enviroment
        /// </summary>
        /// <returns></returns>
        public static async Task OpenRoleMenu()
        {
            (await new Force().AcquireAsync(MemberOptionsButton)).Click();
            (await new Force().AcquireAsync(MemberOptionsPopup_Button)).Click();
        }

        /// <summary>
        /// Assusmes to be in the server settings </br>
        /// (Does not work with ZeroWidthSpace char)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        public static async Task GrantRoles(string user, string name)
        {
            await NavigateMemberTab();
            await SearchUser(user);
            await OpenRoleMenu();

            foreach (var role in GetAllRolesInRolePopup().Skip(1))
            {
                Console.WriteLine(role.name);
                if (role.name == name) 
                    new Actions(_driver)
                        .Click(role.button)
                        .Perform();
            }
        }


    }
}
