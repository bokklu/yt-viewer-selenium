using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Serilog;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using YTViewer.Contracts;
using YTViewer.Contracts.Options;
using YTViewer.Helpers;
using YTViewer.Infrastructure.Interfaces;

namespace YTViewer.Infrastructure.Repos
{
    internal class HotspotRepository : IAddonRepository
    {
        private const int elemWaitTime = 5;
        private const int lowerBoundViewTime = 35;
        private const int upperBoundViewTime = 55;
        private const string ipifyConn = "https://api.ipify.org?format=json";
        private static string ExtensionUrlPage(string addonUuid) => $"moz-extension://{addonUuid}/panel/index.html";

        private readonly string _addonUuid;
        private readonly UserAgent _userAgent;
        private readonly FirefoxDriver _driver;
        private readonly ViewOptions _viewOptions;
        private readonly Guid _correlationId;

        public HotspotRepository(FirefoxDriver driver, string addonUuid, UserAgent userAgent, ViewOptions viewOptions, Guid correlationId)
        {
            _driver = driver;
            _viewOptions = viewOptions;
            _addonUuid = addonUuid;
            _userAgent = userAgent;
            _correlationId = correlationId;
        }

        public void VerifyIpAddress()
        {
            _driver.Navigate().GoToUrl(ipifyConn);
            var spanElems = Extensions.FindElementCount(_driver, By.TagName("span"), TimeSpan.FromSeconds(10).Seconds);
            string browserIp = string.Empty;

            foreach (var spanElem in spanElems)
            {
                var elemText = spanElem.Text;
                var maybeIp = Regex.Match(elemText, "\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}").Value;

                if (maybeIp != string.Empty)
                {
                    browserIp = maybeIp;
                    break;
                }
            }

            Log.Information($"CorrelationId: {_correlationId}, Browser IP: {browserIp}");
        }

        public void Connect()
        {
            #region extension connection
            new WebDriverWait(_driver, TimeSpan.FromSeconds(10)).Until(condition => _driver.WindowHandles.Count == 2);

            Log.Information($"CorrelationId: {_correlationId}, Window Handles Check Successful...");
            Log.Information($"CorrelationId: {_correlationId}, Navigating to {ExtensionUrlPage(_addonUuid)},");

            _driver.Navigate().GoToUrl(ExtensionUrlPage(_addonUuid)); //sometimes this never loads

            Log.Information($"CorrelationId: {_correlationId}, Navigation Successful...");

            _driver.SwitchTo().Window(_driver.WindowHandles.Last());
            _driver.Close();
            _driver.SwitchTo().Window(_driver.WindowHandles.First());
            
            var nextBtn = Extensions.FindElement(_driver, By.ClassName("next"), elemWaitTime);
            for (var i = 1; i <= 4; i++) { nextBtn.Click(); }

            var doneBtn = Extensions.FindElement(_driver, By.XPath("//div[text() = 'Done']"), elemWaitTime);
            doneBtn.Click();

            var connectBtn = Extensions.FindElement(_driver, By.XPath("//div[@class='button disconnected']"), elemWaitTime);
            connectBtn.Click();
            #endregion

            Log.Information($"CorrelationId: {_correlationId}, Navigating to YouTube Video: {_viewOptions.VideoUrl}");
            _driver.Navigate().GoToUrl(_viewOptions.VideoUrl);
            Log.Information($"CorrelationId: {_correlationId}, Navigation Successful...");

            Watch();

            var randSeconds = new Random().Next(lowerBoundViewTime, upperBoundViewTime);
            Thread.Sleep(randSeconds * 200);
        }

        public void Refresh()
        {
            var skipTrialBtn = Extensions.FindElement(_driver, By.XPath("//span[text() = 'No thanks, continue as free']"), elemWaitTime);
            skipTrialBtn.Click();
        }

        private void Watch()
        {
            var startIndexNew = 1;
            var startIndexOld = 16;
            var startIndexMobile = 25;
            var userAgentValue = (int) _userAgent;

            if (userAgentValue >= startIndexNew && userAgentValue < startIndexOld)
            {
                var progressBarElem = Extensions.FindElement(_driver, By.XPath("//div[@class='ytp-progress-bar ']"), TimeSpan.FromMinutes(1).Seconds);

                if (progressBarElem != null)
                    Log.Information($"CorrelationId: {_correlationId}, YTP Progress Bar found...");
                else if (_driver.Url.Contains("moz-extension"))
                {
                    _driver.Navigate().GoToUrl(_viewOptions.VideoUrl);
                    Extensions.FindElement(_driver, By.XPath("//div[@class='ytp-progress-bar ']"), TimeSpan.FromMinutes(1).Seconds);
                }
                    
                var videoPlayer = Extensions.FindElement(_driver, By.XPath("//div[@id='movie_player']/div[26]/div[2]/div/button"), elemWaitTime);
                videoPlayer.Click();
            }
            else if (userAgentValue >= startIndexOld && userAgentValue < startIndexMobile)
            {
                var progressBarElem = Extensions.FindElement(_driver, By.XPath("//div[@class='ytp-progress-bar ']"), TimeSpan.FromMinutes(1).Seconds);
                
                if (progressBarElem != null)
                    Log.Information($"CorrelationId: {_correlationId}, YTP Progress Bar found...");
                else if (_driver.Url.Contains("moz-extension"))
                {
                    _driver.Navigate().GoToUrl(_viewOptions.VideoUrl);
                    Extensions.FindElement(_driver, By.XPath("//div[@class='ytp-progress-bar ']"), TimeSpan.FromMinutes(1).Seconds);
                }

                var videoPlayer = Extensions.FindElement(_driver, By.XPath("//button[@class='ytp-play-button ytp-button']"), elemWaitTime);
                videoPlayer.Click();
            }
            else
            {
                var playerContainerElem = Extensions.FindElement(_driver, By.XPath("//div[@id='player-container-id']"), TimeSpan.FromMinutes(1).Seconds);

                if (playerContainerElem != null)
                    Log.Information($"CorrelationId: {_correlationId}, Player Container ID found...");
                else if (_driver.Url.Contains("moz-extension"))
                {
                    _driver.Navigate().GoToUrl(_viewOptions.VideoUrl);
                    Extensions.FindElement(_driver, By.XPath("//div[@id='player-container-id']"), TimeSpan.FromMinutes(1).Seconds);
                }
            }

        }
    }
}