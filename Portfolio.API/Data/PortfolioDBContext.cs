﻿namespace Portfolio.API.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;

    public class PortfolioDBContext : IdentityDbContext<ApplicationUser>
    {
        public PortfolioDBContext(DbContextOptions options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}