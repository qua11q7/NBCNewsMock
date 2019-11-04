using Microsoft.EntityFrameworkCore;
using NNNDataModel;
using NNNDataModel.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace NNNDataContext.Context
{
    public class NewsContext : DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                DbContextUtils.BuildDbContextOptions(optionsBuilder);
            }
        }

        public DbSet<News> News { get; set; }
        public DbSet<UserLog> Logs { get; set; }
        public DbSet<UserTraceLog> Traces { get; set; }
    }
}
