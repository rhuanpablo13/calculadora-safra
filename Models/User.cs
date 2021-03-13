using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace calculadora_api.Models
{
    public class User : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public User(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }


        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Profile { get; set; }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public bool IsAuthenticated() {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public IEnumerable<Claim> GetClaimsIdentity() {
            return  _accessor.HttpContext.User.Claims;
        }

    }
}