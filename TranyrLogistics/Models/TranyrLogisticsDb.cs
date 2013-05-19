using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using TranyrLogistics.Models.Enquiries;
using TranyrLogistics.Models.Groups;

namespace TranyrLogistics.Models
{
    public class TranyrLogisticsDb : DbContext
    {
        public DbSet<Enquiry> Enquiries { get; set; }

        public DbSet<Shipment> Shipments { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<ShippingTerms> ShippingTerms { get; set; }

        public DbSet<Quotation> Quotations { get; set; }

        public DbSet<CustomerConfirmation> CustomerConfirmations { get; set; }

        public DbSet<ServiceProvider> ServiceProviders { get; set; }

        public DbSet<ShipmentDocument> ShipmentDocuments { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<ShipmentTracker> ShipmentTrackers { get; set; }

        public List<Group> CustomerGroups
        { 
            get {
                var groups = this.Groups;

                List<Group> filteredGroups = new List<Group>();
                foreach (Group group in Groups)
                {
                    if (group is CustomerGroup)
                    {
                        filteredGroups.Add(group);
                    }
                }

                return filteredGroups;
            }
        }

        public List<Group> ServiceProviderGroups
        {
            get
            {
                var groups = this.Groups;

                List<Group> filteredGroups = new List<Group>();
                foreach (Group group in Groups)
                {
                    if (group is ServiceProviderGroup)
                    {
                        filteredGroups.Add(group);
                    }
                }

                return filteredGroups;
            }
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            modelBuilder.Entity<Customer>().HasMany(e => e.Shipments).WithRequired(a => a.Customer).HasForeignKey(e => e.CustomerID);
            modelBuilder.Entity<Customer>().HasRequired(e => e.Country).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerGroup>().HasMany(e => e.Customers).WithRequired(a => a.CustomerGroup).HasForeignKey(e => e.CustomerGroupID);
            modelBuilder.Entity<ServiceProviderGroup>().HasMany(e => e.ServiceProviders).WithRequired(a => a.ServiceProviderGroup).HasForeignKey(e => e.ServiceProviderGroupID);

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