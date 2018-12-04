using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsService.Models
{
   // [Table(Name = "tblComments")]
    public class Comment : IEquatable<Comment>
    {
       // [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int CommentId { get; set; }
        public string CommentText{ get; set; }
        public int UserId { get; set; }
        public int DishId { get; set; }

        public bool Equals(Comment other)
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
