using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.DataAccess.Model;

namespace Server.DataAccess.Configuration
{
    public class RegionsConfiguration : IEntityTypeConfiguration<Regions>
    {
        public void Configure(EntityTypeBuilder<Regions> builder)
        {
            // TODO: TÜM STRINGLER İÇİN MAX VALUE DEĞERLERİ, DİĞER CONSTRAİNTLER VS EKLENMEDİ
            // TODO:PRODUCTION PROJESİ İÇİN TEK TEK BELİRLENİP EKLENEBİLİR.     

            builder.Property<int>(x => x.id)
            .IsRequired();

            builder.HasKey(x => x.id);
            builder.HasMany(x => x.Cities)
            .WithOne(x => x.Regions)
            .HasPrincipalKey(x => x.id)
            .HasForeignKey(x => x.RegionsId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasKey(x => x.id);
            builder.HasMany(x => x.MobilAku)
            .WithOne(x => x.Regions)
            .HasPrincipalKey(x => x.id)
            .HasForeignKey(x => x.RegionsId)
            .OnDelete(DeleteBehavior.Cascade);


        }
    }
}