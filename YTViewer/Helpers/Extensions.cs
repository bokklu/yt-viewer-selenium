using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YTViewer.Helpers
{
    public class Extensions
    {
        #region Generic Helpers
        public static IEnumerable<string> ReadLines(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static string GetDescription(Enum value)
        {
            return value.GetType()
                        .GetMember(value.ToString())
                        .FirstOrDefault()
                        ?.GetCustomAttribute<DescriptionAttribute>()
                        ?.Description ?? value.ToString();
        }
        #endregion

        #region Selenium Helpers
        public static IWebElement FindElement(IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(drv => 
                {
                    try
                    {
                        drv.FindElement(by);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });
            }
            return driver.FindElement(by);
        }

        public static ReadOnlyCollection<IWebElement> FindElements(IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(drv =>
                {
                    try
                    {
                        drv.FindElements(by);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });
            }
            return driver.FindElements(by);
        }

        public static ReadOnlyCollection<IWebElement> FindElementCount(IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(drv =>
                {
                    try
                    {
                        return drv.FindElements(by).Count > 0 ? true : false;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });
            }
            return driver.FindElements(by);
        }
        #endregion
    }
}
