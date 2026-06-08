using LogicLayer.Models;
using System.Collections.Generic;

namespace LogicLayer.Interfaces
{
    public interface ICommentRepository
    {
        List<Comment> GetCommentsByMoveId(int moveId);
        Comment GetCommentById(int id);
        void AddComment(Comment comment);
        void UpdateComment(Comment comment);
        void DeleteComment(int commentId);
    }
}