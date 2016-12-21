// <copyright file="Program.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter
{
    using System.IO;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Program start
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
