using System;
using System.ComponentModel.DataAnnotations;

namespace calculadora_api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Profile { get; set; }
        public Boolean Status { get; set; }
    }
}