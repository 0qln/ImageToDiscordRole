﻿using ImageToDiscordRoles;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.DevTools.V113.Network;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Timers;
using static ImageToDiscordRoles.Discord;
using static ImageToDiscordRoles.DiscordProfile;

public static class Program
{
    public static readonly IWebDriver Driver;


    static Program ()
    {
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.EnableVerboseLogging = false;
        service.SuppressInitialDiagnosticInformation = true;
        service.HideCommandPromptWindow = true;

        ChromeOptions options = new ChromeOptions { };
        //options.AddArgument("--headless");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--disable-crash-reporter");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--disable-in-process-stack-traces");
        options.AddArgument("--disable-logging");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--log-level=3");
        options.AddArgument("--output=/dev/null");

        Driver = new ChromeDriver(service, options);
    }

    
    public static async Task Main(string[] args)
    {
        try
        {
            await StartUp();

            await Login(GatherUserLogin());
            Console.WriteLine("Continue");

            NavigateToServer("Server von Oq_ djwhkakjd");
            await NavigateToServerSettings();
            await NavigateRoleTab();

            Thread.Sleep(1000);

            for (int i = 0; i < 10; i++)
            {
                await CreateRole("neue Rolle", "#000000");
            }
            await CreateRole("[padding] neue Rolle", "#FFFFFF");
            await SaveChanges();

            await DeleteAllRolesByName("neue Rolle");

            //DrawImage(new Bitmap(@"D:\Programmmieren\Projects\ImageToDiscordRoles\cat.png"), "Oq_");

            Console.WriteLine("Application finished executing. (Press any key to exit)"); Console.Read();
        }
        finally
        {
            Driver.Dispose();
        }
    }

}