using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TranyrLogistics.Models
{
    public class TranyrLogisticsDb : DbContext
    {
        public DbSet<Enquiry> Enquiries { get; set; }

        public DbSet<Shipment> Shipments { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<ShippingTerms> ShippingTerms { get; set; }

        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().HasMany(e => e.Shipments).WithRequired(a => a.Customer).HasForeignKey(e => e.CustomerID);
            modelBuilder.Entity<Group>().HasMany(e => e.Customers).WithOptional(a => a.Group).HasForeignKey(e => e.GroupID);
            modelBuilder.Entity<Shipment>().HasRequired(e => e.ShippingTerms);
            modelBuilder.Entity<Shipment>().HasOptional(e => e.ServiceProvider);
        }
    }
}