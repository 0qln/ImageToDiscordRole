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
        private static void DeleteRole(IWebElement element)
        {
            // Bring up popup
            var rightClick = new Actions(_driver).ContextClick(element);
            rightClick.Perform();

            // Click delete
            var delete = new Force().AcquireAsync(DiscordConstants.RoleDeletion).Result;
            delete?.Click();

            // Confirm deletion
            var confirm = new Force().AcquireAsync(DiscordConstants.RoleDeletionConfirmation).Result;
            confirm?.Click();
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
                    ?? throw new ArgumentException("Invalid Password")
            );
        }


        /// <summary>
        /// Navigate the user to the server settings. 
        /// Expects the discord to have a server selected.
        /// TOOD: Check for driver state at the start
        /// </summary>
        public static void NavigateToServerSettings()
        {
            // open popup
            new Force().AcquireAsync(ServerHeader).Result?.Click();

            // navigate to settings
            new Force().AcquireAsync(ServerHeaderPopupSettings).Result?.Click();
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
        public static void Login(Login login)
        {
            // email
            _driver.FindElement(By.Name("email"))
                .SendKeys(login.Email);

            // password
            _driver.FindElement(By.Name("password"))
                .SendKeys(login.Password);

            // confirm
            _driver.FindElement(By.Name("password"))
                .SendKeys(Keys.Enter);

            Console.WriteLine("Login succsesful.");
        }


        /// <summary>
        /// Notice: your server must be contained inside of a folder in order for this
        /// method to see it.
        /// </summary>
        /// <param name="serverName"></param>
        public static async void NavigateToServer(string serverName)
        {
            // Get folders
            var serverElement = await new Force().AcquireAsync(DiscordConstants.ServerElement);
            //Console.WriteLine("Accuired server source");
            var folders = serverElement?.FindElements(By.ClassName("wrapper-38slSD"));

            if (folders is null)
                throw new ArgumentNullException();

            WaitForUI();

            // Fetch all servers out of the folders
            foreach (var folder in folders)
            {
                folder.Click();
                var group = folder.FindElement(By.TagName("ul"));
                var groupServers = group.FindElements(By.ClassName("listItem-3SmSlK"));

                // Iterate all servers in the folder
                foreach (var server in groupServers)
                {
                    string name = server.FindElement(By.ClassName("blobContainer-ikKyFs")).GetAttribute("data-dnd-name");
                    if (name == serverName)
                    {
                        server.Click();
                        return;
                    }
                }
            }
        }

        public static void CreateNewRole()
        {
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div/div/div[1]/div/div[1]/div/div[1]/div[2]")).Click();
        }


        /// <summary>
        /// Presses the "Save Changes" button and returns when all changes are saved.
        /// Expects discord to have an unsaved change in the role settings.
        /// </summary>
        public static async Task SaveChanges()
        {
            // Save
            var saveButton = await new Force().AcquireAsync(DiscordConstants.SaveButton);
            saveButton?.Click();

            // Wait until the changes have been saved
            await new Force { Interval = 100 }.NotAcquireAsync(() =>
                _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div[2]")));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task CreateRole(string name, string color)
        {
            // Open the role settings
            CreateNewRole();

            var roleSettings =
                GetOnlyChild(GetOnlyChild(

                _driver.FindElement(By.
                XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div/div/div[1]/div/div[2]")),

                By.TagName("div")), By.TagName("div"));


            var nameElement = _driver.FindElement(By.
                XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div/div/div[1]/div/div[2]/div/div[1]/div[2]/div/input"));
            nameElement.Clear();
            nameElement.SetValue(name);

            WaitForUI(10);

            var colorPickerElement = await new Force().AcquireAsync(DiscordConstants.ColorPicker);
            colorPickerElement.Click();

            var colorSetterElement = await new Force().AcquireAsync(() =>
                _driver.FindElement(By
                .XPath("/html/body/div[2]/div[2]/div[1]/div[3]/div/div/div/div[2]/div/input")))
                ?? throw new NullReferenceException();
            colorSetterElement.Clear();
            colorSetterElement.SetValue(color);
            colorSetterElement.SendKeys(Keys.Enter);

            WaitForUI(10);
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
        public static void DeleteAllRolesByName(string name)
        {
            while (TryGetRole(name, out IWebElement? element))
                if (element is not null)
                    DeleteRole(element);
        }

        public static void NavigateToRoll(string name, bool inverse) => (!inverse
                ? GetRole(name)

                : GetAllRoles().Result
                .Where(x => GetNameOfRole(x) != name)
                .First())

                .Click();


        /// <summary>
        /// Navigate to the role tab.
        /// </summary>
        /// <returns></returns>
        public static void NavigateRoleTab()
        {
            // Go roles tab
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[1]/div/nav/div/div[3]"))
                .Click();

            WaitForUI();

            // Go Role manager button
            _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div/div/div[1]/div/div/div/div/div[2]/div[3]/div/button"))
                .Click();

            WaitForUI();
        }


    }
}
