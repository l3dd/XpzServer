using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Andoria.Server
{
    /// <summary>
    /// Main entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args">Entries parameters</param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Defined the behavior of current app
        /// </summary>
        /// <param name="args">Entries args</param>
        /// <returns>Returned a configured web host</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
