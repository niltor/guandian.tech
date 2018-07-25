using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comment.Data.Entity;

namespace Comment.Models
{
    public class NewsListViewModel
    {
        public List<News> NewsList { get; set; }
    }
}
