using System;
using Microsoft.EntityFrameworkCore;

namespace calculadora_api.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> UserItems { get; set; }
        public DbSet<ChequeEmpresarial> ChequeEmpresarialItems { get; set; }
        public DbSet<ParceladoPre> ParceladoPreItems { get; set; }
        public DbSet<Indice> IndiceItems { get; set; }
        public DbSet<Log> LogItems { get; set; }



        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //     => optionsBuilder.UseSqlServer(@"Server=tcp:calculadora-db-safra2.database.windows.net,1433;Database=UserAPI;MultipleActiveResultSets=true;User ID=admin-calculadora-db-safra;Password=lZPw@fMQ^fLg");
        
    }
}