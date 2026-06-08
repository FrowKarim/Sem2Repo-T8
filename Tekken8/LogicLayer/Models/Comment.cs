using System;

namespace LogicLayer.Models
{
    public class Comment
    {
        public int Id { get; set; }
        
        public int TekkenID { get; set; }

        public int MoveId { get; set; }

        public int UserId { get; set; }

        public string CommentText { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public User User { get; set; }
    }
}