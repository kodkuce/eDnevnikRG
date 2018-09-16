using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace eDnevnikWEBAPI
{
    public class Program
    {
        
         public static void Main(string[] args)
        {
            MojWebHost(args).Run();
        }

        public static IWebHost MojWebHost(string[] args)
        {
            string prt = Environment.GetEnvironmentVariable("PORT");

            return new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .UseUrls("http://0.0.0.0:5001")
            //.UseUrls("http://0.0.0.0:"+prt)
            .Build();
        }
        
        // public static void Main(string[] args)
        // {
        //     CreateWebHostBuilder(args).Build().Run();
        // }

        // public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //     WebHost.CreateDefaultBuilder(args)
        //         .UseStartup<Startup>();
    }
}
