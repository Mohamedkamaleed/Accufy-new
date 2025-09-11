using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.CategoryID);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            // Self-referencing relationship
            //builder.HasOne(c => c.ParentCategory)
            //    .WithMany(c => c.ChildCategories)
            //    .HasForeignKey(c => c.ParentCategoryID)
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.Restrict);

            // Unique index on Name
            builder.HasIndex(c => c.Name)
                .IsUnique();

            // Optional: Index for ParentCategoryID for better query performance
            builder.HasIndex(c => c.ParentCategoryID);
        }
    }
}
