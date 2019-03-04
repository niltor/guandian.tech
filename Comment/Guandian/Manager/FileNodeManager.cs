using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guandian.Data;

namespace Guandian.Manager
{
    public class FileNodeManager : BaseManager
    {
        public FileNodeManager(ApplicationDbContext context) : base(context)
        {
        }
    }
}
