using ApiJwtEfOracle.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiJwtEfOracle.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USERS");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.Username).HasColumnName("USERNAME").HasMaxLength(50);
            builder.Property(x => x.PasswordHash).HasColumnName("PASSWORD_HASH");
            builder.Property(x => x.PasswordSalt).HasColumnName("PASSWORD_SALT");
            builder.Property(x => x.CreatedAt).HasColumnName("CREATED_AT");
            builder.Property(x => x.RefreshToken).HasColumnName("REFRESH_TOKEN");
            builder.Property(x => x.RefreshTokenExpiryTime).HasColumnName("REFRESH_TOKEN_EXP_TIME");
        }
    }
}
