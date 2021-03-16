using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace calculadora_api.Models
{
    public class User : IUser
    {
        public User()
        {
        }


        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Profile { get; set; }

        public string Name => "Rhuan";
        // public string Name => _accessor.HttpContext.User.Identity.Name;

        public bool IsAuthenticated()
        {
            // return _accessor.HttpContext.User.Identity.IsAuthenticated;
            return false;
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            // return  _accessor.HttpContext.User.Claims;
            return null;
        }

    }
}