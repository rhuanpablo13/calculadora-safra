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

        public User(int Id, string Username, string Profile) {
            this.Id = Id;
            this.Username = Username;
            this.Profile = Profile;
            CreatedDate = new DateTime();
        }

        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Username { get; set; }
        public string Profile { get; set; }

    }
}