using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyApp.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
        
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Core Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Core Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}

public static class SqliteInMemoryDatabase
{
    private static object key = new();
    static SqliteInMemoryDatabase()
    {
        lock (key)
        {
            Connection = new SqliteConnection("Filename=:memory:");
            // We want to keep the connection open
            // and reuse it since our database is in-memory
            // Warning: don't use this with multiple clients
            // and closing the connection destroy the database
            Connection.Open();
        }
    }
        
    public static SqliteConnection Connection { get; }
}
