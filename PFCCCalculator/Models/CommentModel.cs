

using System;
using System.ComponentModel.DataAnnotations;

namespace PFCCCalculatorService.Models
{
    public class CommentModel : IEquatable<CommentModel>
    {
           public int CommentId { get; set; }
           [Required(ErrorMessage = "Введите комментарий")]
           public string CommentText { get; set; }
           public string UserId { get; set; }
           public int DishId { get; set; }
    

        public bool Equals(CommentModel other)
        {
            return other != null &&
                CommentId == other.CommentId &&
                CommentText == other.CommentText &&
                UserId == other.UserId &&
                DishId == other.DishId;
        }
        public override int GetHashCode()
        {
            return this.CommentId.GetHashCode();
        }
    }
}
