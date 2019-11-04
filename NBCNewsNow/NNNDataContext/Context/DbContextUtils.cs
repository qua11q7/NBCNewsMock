using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NNNDataContext.Context
{
    public class DbContextUtils
    {
        private static IConfigurationRoot _configuration;

        public static IConfigurationRoot GetConfiguration(IHostingEnvironment env = null)
        {
            if (_configuration == null)
            {
                var dataAssemblyDirectoryPath = Path.GetDirectoryName(typeof(NewsContext).GetTypeInfo().Assembly.Location);
                var directoryInfo = new DirectoryInfo(dataAssemblyDirectoryPath);
                while (!Directory.GetFiles(directoryInfo.FullName).Any(filePath => filePath.ToLower().EndsWith(".sln")))
                {
                    directoryInfo = directoryInfo.Parent;
                }
                var webApiDirectory = Path.Combine(directoryInfo.FullName, DbConstants.WebApiProjectName);

                if (!File.Exists(Path.Combine(webApiDirectory, "appsettings.json")))
                {
                    if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")))
                    {
                        webApiDirectory = Directory.GetCurrentDirectory();
                    }
                    else
                    {
                        throw new Exception("Could not find setting file in " + Path.Combine(webApiDirectory, "appsettings.json"));
                    }
                }

                var environmentName = env?.EnvironmentName ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var builder = new ConfigurationBuilder()
                    .SetBasePath(webApiDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: true);

                _configuration = builder.Build();
            }
            return _configuration;
        }

        public static string GetConnectionString(IHostingEnvironment env = null) =>
            GetConfiguration(env).GetConnectionString(DbConstants.ConnectionStringName);

        public static DbContextOptionsBuilder BuildDbContextOptions(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer(GetConnectionString(), sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
    }
}
