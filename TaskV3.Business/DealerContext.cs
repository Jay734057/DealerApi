using Microsoft.AspNetCore.Http;
using System;
using TaskV3.Core.Interfaces.Business;

namespace TaskV3.Business
{
    public class DealerContext : IDealerContext
    {
        private readonly IHttpContextAccessor _context;

        public DealerContext(IHttpContextAccessor context)
        {
            _context = context;
        }

        public int DealerId
        {
            get
            {
                return Convert.ToInt32(_context.HttpContext.User?.Identity.Name);
            }
        }
    }
}
