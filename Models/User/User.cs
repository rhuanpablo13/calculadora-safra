using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace calculadora_api.Models
{
    public class User
    {
        public User() { }

        public User(int Id, string Name, string Profile) {
            this.Id = Id;
            this.Name = Name;
            this.Profile = Profile;
            CreatedDate = new DateTime();
        }

        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }

    }
}