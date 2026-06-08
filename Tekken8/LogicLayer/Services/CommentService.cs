using LogicLayer.Interfaces;
using LogicLayer.Models;
using System;
using System.Collections.Generic;

namespace LogicLayer.Services
{
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public List<Comment> GetCommentsByMoveId(int moveId)
        {
            if (moveId <= 0)
            {
                throw new ArgumentException("MoveId must be greater than 0.", nameof(moveId));
            }

            return _commentRepository.GetCommentsByMoveId(moveId);
        }

        public Comment GetCommentById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Comment id must be greater than 0.", nameof(id));
            }

            return _commentRepository.GetCommentById(id);
        }

        public void AddComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            if (comment.MoveId <= 0)
            {
                throw new ArgumentException("MoveId must be greater than 0.", nameof(comment.MoveId));
            }

            if (comment.UserId <= 0)
            {
                throw new ArgumentException("UserId must be greater than 0.", nameof(comment.UserId));
            }

            if (string.IsNullOrWhiteSpace(comment.CommentText))
            {
                throw new ArgumentException("Comment text is required.", nameof(comment.CommentText));
            }

            comment.CreatedAt = DateTime.Now;

            _commentRepository.AddComment(comment);
        }

        public void UpdateComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            if (comment.Id <= 0)
            {
                throw new ArgumentException("Comment id must be greater than 0.", nameof(comment.Id));
            }

            if (string.IsNullOrWhiteSpace(comment.CommentText))
            {
                throw new ArgumentException("Comment text is required.", nameof(comment.CommentText));
            }

            comment.UpdatedAt = DateTime.Now;

            _commentRepository.UpdateComment(comment);
        }

        public void DeleteComment(int commentId)
        {
            if (commentId <= 0)
            {
                throw new ArgumentException("Comment id must be greater than 0.", nameof(commentId));
            }

            _commentRepository.DeleteComment(commentId);
        }
    }
}