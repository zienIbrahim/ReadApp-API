using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace APIAngular.Models
{


    public class DbZContext : DbContext
    {


        public DbZContext(DbContextOptions<DbZContext> options) : base(options){}


        public DbSet<User> User { get; set; }


    }


}
