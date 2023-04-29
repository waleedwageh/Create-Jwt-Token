using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt.Models
{
    public class ApplicationDBContext:IdentityDbContext<ApplicationUsers>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> option) : base (option)
        {

        }
    }
}
