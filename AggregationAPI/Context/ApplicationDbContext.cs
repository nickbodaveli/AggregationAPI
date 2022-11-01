using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using System;
using Microsoft.EntityFrameworkCore;
using AggregationAPI.Models;

namespace AggregationAPI.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Dataset> Datasets => Set<Dataset>();

    }
}
