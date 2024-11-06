using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StudentApi.Controllers;

namespace StudentApi.Data.Config
{
    public class UserTypeConfig : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.ToTable("UserTypes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description);

            builder.HasData(new List<UserType>()
            {
               new UserType { Id = 1, Name="Student",Description="asdf" },
               new UserType { Id = 2, Name="Staff",Description="asdf" },
               new UserType { Id = 3, Name="Parents",Description="asdf" },
               new UserType { Id = 4, Name="Sir",Description="asdf" }
            });
        }

    }
    
}
