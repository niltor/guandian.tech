using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comment.Data.Entity
{
    public class Comment : BaseDb
    {
        public Author Author { get; set; }
    }
}
