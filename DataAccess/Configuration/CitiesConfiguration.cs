using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.DataAccess.Model;

namespace Server.DataAccess.Configuration
{
    public class CitiesConfiguration : IEntityTypeConfiguration<Cities>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cities> builder)
        {
            // TODO: TÜM STRINGLER İÇİN MAX VALUE DEĞERLERİ, DİĞER CONSTRAİNTLER VS EKLENMEDİ
            // TODO:PRODUCTION PROJESİ İÇİN TEK TEK BELİRLENİP EKLENEBİLİR.     

            builder.Property<int>(x => x.id)
            .IsRequired();

            builder.Property<int>(x => x.RegionsId)
            .HasColumnName("region_id");

            builder.HasKey(x => x.id);
            builder.HasOne(x => x.Regions).WithMany(x => x.Cities).HasForeignKey(x => x.RegionsId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}