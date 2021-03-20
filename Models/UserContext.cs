using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace calculadora_api.Models
{
    public class ApplicationContext : DbContext
    {

        public DbSet<User> UserItems { get; set; }
        public DbSet<ChequeEmpresarial> ChequeEmpresarialItems { get; set; }
        public DbSet<ParceladoPre> ParceladoPreItems { get; set; }
        public DbSet<Indice> IndiceItems { get; set; }
        public DbSet<Log> LogItems { get; set; }



        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}