using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace YTViewer.Infrastructure
{
    internal class ExtendedFirefoxDriver : FirefoxDriver
    {
        public ExtendedFirefoxDriver(FirefoxDriverService driverService, FirefoxOptions options) : base(driverService, options)
        {
            CommandExecutor.CommandInfoRepository.TryAddCommand("installAddOn", new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/moz/addon/install"));
        }

        public string InstallAddOnFromFile(string addOnFileToInstall)
        {
            if (string.IsNullOrEmpty(addOnFileToInstall))
            {
                throw new ArgumentNullException("addOnFileToInstall", "Add-on file name must not be null or the empty string");
            }

            if (!File.Exists(addOnFileToInstall))
            {
                throw new ArgumentException("File " + addOnFileToInstall + " does not exist", "addOnFileToInstall");
            }

            var addOnBytes = File.ReadAllBytes(addOnFileToInstall);
            var base64AddOn = Convert.ToBase64String(addOnBytes);
            return InstallAddOn(base64AddOn);
        }

        public string InstallAddOn(string base64EncodedAddOn)
        {
            if (string.IsNullOrEmpty(base64EncodedAddOn))
            {
                throw new ArgumentNullException("base64EncodedAddOn", "Base64 encoded add-on must not be null or the empty string");
            }

            var parameters = new Dictionary<string, object>();
            parameters["addon"] = base64EncodedAddOn;
            return Execute("installAddOn", parameters).Value.ToString();
        }
    }
}
