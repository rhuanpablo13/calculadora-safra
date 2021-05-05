using System;
using System.Collections.Generic;
using System.Linq;
using calculadora_api.Models;
using calculadora_api.Services;

namespace calculadora_api.Repositories
{
    public class UserRepository
    {

        
        private readonly ApplicationContext _context;


        public UserRepository(ApplicationContext context)
        {            
            _context = context;
        }

        public void Adicionar(User entidade)
        {
            _context.Add(entidade);
        }

        public User Get(string username)
        {
            return _context.UserItems.Where(u => u.Username == username).FirstOrDefault();            
        }


        public List<User> UserList()
        {            
            return _context.UserItems.ToList();
        }


    }
}