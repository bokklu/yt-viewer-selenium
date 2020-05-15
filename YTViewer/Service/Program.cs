using CommandLine;
using Serilog;
using System;
using YTViewer.Application;
using YTViewer.Contracts.Options;

namespace YTViewer.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {NewLine} {Exception}";

            try
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.File("log.txt", outputTemplate: outputTemplate)
                    .WriteTo.Console(outputTemplate: outputTemplate)
                    .CreateLogger();

                Parser.Default.ParseArguments<ViewOptions>(args).WithParsed(opts => new CommandLineParser().View(opts));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}
