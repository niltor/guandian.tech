using Guandian.Data;

namespace Guandian.Manager
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
