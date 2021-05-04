using System;
using System.Collections.Generic;
using System.Linq;
using calculadora_api.Models;
using calculadora_api.Services;

namespace calculadora_api.Repositories
{
    public class UserRepository
    {

        private readonly User _user;

        private readonly ApplicationContext _context;


        public UserRepository(User user, ApplicationContext context)
        {
            _user = user;
            _context = context;
        }

        public void Adicionar(User entidade)
        {
            Console.WriteLine(entidade.Name);
        }

        public User Get(string username)
        {
            IQueryable<User> users = _context.UserItems.Where(u => u.Name == username);
            
            return users.Where(x => x.Name.ToLower() == username.ToLower()).FirstOrDefault();
        }


        public static List<User> UserList()
        {
            var users = new List<User>();
            DateTime localDate = DateTime.Now;

            users.Add(new User { Id = 1, Name = "admin", Profile = "admin", CreatedDate = localDate });
            users.Add(new User { Id = 2, Name = "editor", Profile = "employee", CreatedDate = localDate });
            users.Add(new User { Id = 3, Name = "consult", Profile = "consult", CreatedDate = localDate });

            return users;
        }


    }
}