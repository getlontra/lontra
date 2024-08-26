using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lontra.Identity;

internal class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser<long>>
{
    public void Configure(EntityTypeBuilder<IdentityUser<long>> builder)
    {
    }
}
