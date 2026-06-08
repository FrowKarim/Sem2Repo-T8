using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace LogicLayer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string TekkenID { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}