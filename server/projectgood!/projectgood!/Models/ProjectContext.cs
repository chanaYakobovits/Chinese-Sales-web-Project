using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjectChinesesActstion.Models;

public partial class ProjectContext : DbContext
{
    public ProjectContext()
    {
    }

    public ProjectContext(DbContextOptions<ProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> User { get; set; }

    public virtual DbSet<Donator> Donator { get; set; }

    public virtual DbSet<Customer> Customer { get; set; }

    public virtual DbSet<Category> Category { get; set; }

    public virtual DbSet<Gift> Gift { get; set; }

    public virtual DbSet<Order> Order { get; set; }
    
    public virtual DbSet<Winner> Winner { get; set; }

    public virtual DbSet<Admin> Admin { get; set; }

}
