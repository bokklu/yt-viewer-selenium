using CommandLine;

namespace YTViewer.Contracts.Options
{
    [Verb("view", HelpText = "View specific youtube video URL")]
    internal class ViewOptions : BaseOptions
    {
        [Option('u', "url", Required = true, HelpText = "Youtube Video URL")]
        public string VideoUrl { get; set; }

        [Option('c', "count", Required = true, Default = 10, HelpText = "View Amount")]
        public int ViewCount { get; set; }
    }
}
