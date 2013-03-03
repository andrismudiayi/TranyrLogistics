using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TranyrLogistics.Models
{
    public class TranyrLogisticsDb : DbContext
    {
        public DbSet<Enquiry> Enquiries { get; set; }

        public DbSet<Shipment> Shipments { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<ShippingTerms> ShippingTerms { get; set; }

        public DbSet<ServiceProvider> ServiceProviders { get; set; }

        public DbSet<ShipmentDocument> ShipmentDocuments { get; set; }

        public DbSet<Country> Countries { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            modelBuilder.Entity<Customer>().HasMany(e => e.Shipments).WithRequired(a => a.Customer).HasForeignKey(e => e.CustomerID);
            modelBuilder.Entity<Customer>().HasRequired(e => e.Country).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>().HasMany(e => e.Customers).WithOptional(a => a.Group).HasForeignKey(e => e.GroupID);

            modelBuilder.Entity<Shipment>().HasRequired(e => e.ShippingTerms);
            modelBuilder.Entity<Shipment>().HasRequired(e => e.ServiceProvider);
            modelBuilder.Entity<Shipment>().HasRequired(e => e.OriginCountry).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Shipment>().HasRequired(e => e.DestinationCountry).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Enquiry>().HasRequired(e => e.OriginCountry).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Enquiry>().HasRequired(e => e.DestinationCountry).WithMany().WillCascadeOnDelete(false);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}