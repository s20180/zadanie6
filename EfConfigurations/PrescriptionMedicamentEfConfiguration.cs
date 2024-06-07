using Ef_CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ef_CodeFirst.EfConfigurations;

public class PrescriptionMedicamentEfConfiguration : IEntityTypeConfiguration<PrescriptionMedicament>
{
    public void Configure(EntityTypeBuilder<PrescriptionMedicament> builder)
    {
        builder.HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });

        builder.Property(pm => pm.Dose).IsRequired();
        builder.Property(pm => pm.Details).IsRequired().HasMaxLength(100);
        
        builder.HasOne<Prescription>(pm => pm.IdPrescriptionNavigation)
            .WithMany(p => p.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdPrescription)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Medicament>(pm => pm.IdMedicamentNavigation)
            .WithMany(m => m.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdMedicament);
    }
}