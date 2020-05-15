using CommandLine;

namespace YTViewer.Contracts.Options
{
    internal class BaseOptions
    {
        [Option('a', "addon", Required = true, Default = "Hotspot", HelpText = "Add-on for IP change")]
        public string Addon { get; set; }
    }
}
