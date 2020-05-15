using OpenQA.Selenium.Firefox;
using Serilog;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using YTViewer.Application.Validators;
using YTViewer.Contracts;
using YTViewer.Contracts.Options;
using YTViewer.Infrastructure;
using YTViewer.Infrastructure.Interfaces;
using YTViewer.Infrastructure.Repos;

namespace YTViewer.Application
{
    internal class CommandLineParser
    {
        public void View(ViewOptions viewOptions)
        {
            var viewOptionsValidator = new ViewOptionsValidator();
            var viewOptionsValidatorResult = viewOptionsValidator.Validate(viewOptions);
            if (!viewOptionsValidatorResult.IsValid)
            {
                Log.Error(string.Join(",", viewOptionsValidatorResult.Errors.Select(ve => ve.ErrorMessage)));
                return;
            }

            var correlationId = Guid.NewGuid();
            var userAgentsEnums = Enum.GetValues(typeof(UserAgent));
            var enumInt = new Random().Next(1, userAgentsEnums.Length);
            var userAgentEnum = (UserAgent) userAgentsEnums.GetValue(enumInt);
            var addonEnum = (Addon) Enum.Parse(typeof(Addon), viewOptions.Addon, true);
            var addonXpiPath = GetAddonPath(addonEnum);
            var mozDriver = new MozillaDriver(addonXpiPath, userAgentEnum, correlationId);
            var addonRepo = GetAddonRepository(addonEnum, mozDriver.Driver, mozDriver.Uuid, mozDriver.UserAgent, viewOptions, correlationId);

            addonRepo.Connect();
            addonRepo.VerifyIpAddress();

            Log.Information($"Quitting..., CorrelationId: {correlationId}");
            mozDriver.Quit();

            Console.ReadLine();
        }

        private static string GetAddonPath(Addon addonEnum)
        {
            switch (addonEnum)
            {
                case Addon.HOTSPOT: return ConfigurationManager.AppSettings["HotspotXpiPath"];
                case Addon.HOXXVOX: return ConfigurationManager.AppSettings["HoxxVoxXpiPath"];
                default: throw new Exception("Exception while retrieving addon xpi.");
            }
        }

        private static IAddonRepository GetAddonRepository(Addon addonEnum, FirefoxDriver driver, string uuid, UserAgent userAgentEnum, ViewOptions viewOptions, Guid correlationId)
        {
            switch (addonEnum)
            {
                case Addon.HOTSPOT: return new HotspotRepository(driver, uuid, userAgentEnum, viewOptions, correlationId);
                default: throw new Exception("Exception while retrieving addon repo.");
            }
        }
    }
}
