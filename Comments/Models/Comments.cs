using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.Models
{
    [Table(Name = "tblComments")]
    public class Comment
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int CommentId { get; set; }
        public string CommentText{ get; set; }
        public int UserId { get; set; }
        public int DishId { get; set; }
    }
}
