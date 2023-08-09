﻿using ImageToDiscordRoles;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V113.Network;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Timers;


public static class Program
{
    private static IWebDriver _driver = new ChromeDriver();


    public static async Task Main(string[] args)
    {
        OpenDiscord();
        
        Console.WriteLine("Enter the name of your server: ");
        await NavigateToServer("Autismus Clan"/*Console.ReadLine() ?? ""*/);

        NavigateServerSettings();
        
        await NavigateRoleTab();


        Console.WriteLine("Application finished executing. (Press any key to exit)");
        Console.Read();
        _driver.Dispose();
    }


    private static Task NavigateRoleTab() => Task.Run(() =>
    {
        // Go roles tab
        _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[1]/div/nav/div/div[3]"))
            .Click();

        // Go Role manager button
        _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div/div/div[1]/div/div/div/div/div[2]/div[3]/div/button"))
            .Click();

        // delete new role that was created from discord 
        var list = _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div[2]/div/div/div[2]/div/div/div[1]/div/div[1]/div/div[2]/div[1]"))
            .FindElements(By.TagName("div"));

        foreach (var item in list)
        {
            Console.WriteLine($"{item.GetAttribute("aria-label")}");
        }
    });

    private static void OpenDiscord()
    {
        _driver.Navigate().GoToUrl("https://discord.com/login");

        Console.WriteLine("Enter your login: [Email, Password]");
        Login login = new Login(
            Email: Console.ReadLine() ?? throw new ArgumentException("Invalid Email"),
            Password: Console.ReadLine() ?? throw new ArgumentException("Invalid Password")
        );
        EmailAddressAttribute email = new EmailAddressAttribute();
        if (!email.IsValid(login.Email)) throw new ArgumentException("Invalid Email");
        Console.WriteLine("Login Verified.");


        LoginDiscord(login);
        Console.WriteLine("Login succsesful.");
    }

    private static void NavigateServerSettings()
    {
        // open popup
        var acquirer = new WebElementAcquirer();
        var serverDropDown = acquirer.AcquireAsync(() =>_driver.FindElement(
            By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[1]/nav/div[1]"))).Result;
        serverDropDown?.Click();

        // navigate to settings
        var buttonContainer = _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[3]/div/div/div/div/div[3]"));
        var button = buttonContainer.FindElement(By.Id("guild-header-popout-settings"));
        button.Click();
    }

    /// <summary>
    /// Notice: your server must be contained inside of a folder in order for this
    /// method to see it.
    /// </summary>
    /// <param name="serverName"></param>
    private static async Task NavigateToServer(string serverName)
    {
        // Get folders
        var serverElement = await new WebElementAcquirer().AcquireAsync(GetServerElement);
        Console.WriteLine("Accuired server source");
        var folders = serverElement?.FindElements(By.ClassName("wrapper-38slSD"));

        if (folders is null)
            throw new ArgumentNullException();

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
                    server.Click();
            }
        }
    }

    /// <summary>
    /// This method will throw an error if it gets called before the IWebElement is loaded
    /// </summary>
    /// <returns></returns>
    private static ValueTask<IWebElement> GetServerElement() => new ValueTask<IWebElement>(
        _driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div/div/nav/ul/div[2]/div[3]")));


    private static void LoginDiscord(Login login)
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
    }
}