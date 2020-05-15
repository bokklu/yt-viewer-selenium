using OpenQA.Selenium.Firefox;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Support.UI;
using YTViewer.Contracts;
using YTViewer.Helpers;
using YTViewer.Infrastructure.Interfaces;
using Serilog;

namespace YTViewer.Infrastructure
{
    internal class MozillaDriver : IMozillaDriver
    {
        public FirefoxDriver Driver { get; }
        public string Uuid { get; }
        public UserAgent UserAgent { get; }

        private string _addonKey;
        private readonly Guid _correlationId;

        internal MozillaDriver(string addonXpiPath, UserAgent userAgent, Guid correlationId)
        {
            _correlationId = correlationId;

            Driver = Create(addonXpiPath, userAgent);
            Uuid = GetExtensionId();
            UserAgent = userAgent;
        }

        private FirefoxDriver Create(string addon, UserAgent userAgent)
        {
            var userAgentDesc = Extensions.GetDescription(userAgent);
            Log.Information($"CorrelationId: {_correlationId}, Addon: {addon}, User-Agent: {userAgentDesc}");

            var firefoxDriverService = FirefoxDriverService.CreateDefaultService(ConfigurationManager.AppSettings["GeckoDriverDirectory"]);
            firefoxDriverService.FirefoxBinaryPath = ConfigurationManager.AppSettings["FirefoxBinaryDirectory"];
            
            var profile = new FirefoxProfile { DeleteAfterUse = true };
            profile.SetPreference("general.useragent.override", userAgentDesc);

            var firefoxOptions = new FirefoxOptions { Profile = profile };
            var firefoxDriver = new ExtendedFirefoxDriver(firefoxDriverService, firefoxOptions);
            
            _addonKey = firefoxDriver.InstallAddOnFromFile(addon);
            Log.Information($" CorrelationId: { _correlationId}, Addon installed...");

            return firefoxDriver;
        }

        public void ReloadAddon()
        {
            Driver.Navigate().GoToUrl("about:addons");
            Driver.FindElementById("category-extension").Click();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("let hb = document.getElementById(\"html-view-browser\");");
            stringBuilder.Append("let al = hb.contentWindow.window.document.getElementsByTagName(\"addon-list\")[0];");
            stringBuilder.Append("let cards = al.getElementsByTagName(\"addon-card\");");
            stringBuilder.Append("for(let card of cards){ card.addon.disable(); card.addon.enable(); }");

            Driver.ExecuteScript(stringBuilder.ToString());
        }

        public void Quit()
        {
            Driver.Quit();
        }

        #region helper methods

        private string GetExtensionId()
        {
            if(_addonKey.Contains("hotspot"))
                new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(condition => Driver.WindowHandles.Count == 2);

            var prefsPath = Driver.Capabilities.GetCapability("moz:profile").ToString();
            var prefs = Extensions.ReadLines($@"{prefsPath}\prefs.js");
            var uuidsLine = prefs.Single(p => p.Contains("extensions.webextensions.uuids"));
            var extensionId = Regex.Match(uuidsLine, $"(?<={_addonKey}\\\\\"\\:\\\\\").[A-Za-z0-9-]+").Value;

            Log.Information($"CorrelationId: {_correlationId}, Acquired ExtensionId: {extensionId}");
            return extensionId;
        }

        #endregion
    }
}