using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using calculadora_api.Dao;

namespace calculadora_api.Models
{
    public class ApplicationContext : DbContext
    {

        public DbSet<User> UserItems { get; set; }
        public DbSet<ChequeEmpresarialDao> ChequeEmpresarialItems { get; set; }
        public DbSet<ParceladoPreDao> ParceladoPreItems { get; set; }
        public DbSet<Indice> IndiceItems { get; set; }
        public DbSet<Log> LogItems { get; set; }



        // protected override void OnModelCreating(ModelBuilder builder)
        // {
        //     ;
        //     foreach (var property in builder.Model.GetEntityTypes()
        //         .SelectMany(t => t.GetProperties())
        //         .Where(p => p.ClrType == typeof(float) || p.ClrType == typeof(float?)))
        //     {
              
        //         property.Relational().ColumnType = "float(18,2)";

              
        //     }
        // }


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}