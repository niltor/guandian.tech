using Guandian.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guandian.Areas.Webhooks.Manager
{
    public class BaseManager
    {
        protected readonly ApplicationDbContext _context;
        public BaseManager(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
