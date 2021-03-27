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



        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}