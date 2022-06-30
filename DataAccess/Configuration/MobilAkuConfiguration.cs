using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.DataAccess.Model;

namespace Server.DataAccess.Configuration
{
    public class MobilAkuConfiguration : IEntityTypeConfiguration<MobilAku>
    {
        public void Configure(EntityTypeBuilder<MobilAku> builder)
        {
            // TODO: TÜM STRINGLER İÇİN MAX VALUE DEĞERLERİ, DİĞER CONSTRAİNTLER VS EKLENMEDİ
            // TODO:PRODUCTION PROJESİ İÇİN TEK TEK BELİRLENİP EKLENEBİLİR.     

            builder.Property<int>(x => x.id)
            .IsRequired();

            builder.Property<int>(x => x.CitiesId)
            .HasColumnName("city_id");

            builder.Property<int>(x => x.RegionsId)
            .HasColumnName("region_id");


            builder.HasKey(x => x.id);
            builder.HasOne(x => x.Cities)
            .WithMany(x => x.MobilAku)
            .HasPrincipalKey(x => x.id)
            .HasForeignKey(x => x.CitiesId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasKey(x => x.id);
            builder.HasOne(x => x.Regions)
            .WithMany(x => x.MobilAku)
            .HasPrincipalKey(x => x.id)
            .HasForeignKey(x => x.RegionsId)
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}