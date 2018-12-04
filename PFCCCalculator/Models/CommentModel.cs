using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFCCCalculatorService.Models
{
    public class CommentModel
    {
            public int CommentId { get; set; }
            public string CommentText { get; set; }
            public int UserId { get; set; }
            public int DishId { get; set; }
    }
}
