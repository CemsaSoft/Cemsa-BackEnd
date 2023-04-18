using Cemsa_BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Cemsa_BackEnd
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        
    }
}

