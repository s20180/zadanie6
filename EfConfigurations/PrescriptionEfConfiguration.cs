using Ef_CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ef_CodeFirst.EfConfigurations;

public class PrescriptionEfConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.HasKey(p => p.IdPrescription);
        builder.Property(p => p.IdPrescription).ValueGeneratedOnAdd();

        builder.Property(p => p.Date).IsRequired();
        builder.Property(p => p.DueDate).IsRequired();

        builder.HasOne<Doctor>(p => p.IdDoctorNavigation)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.IdDoctor);

        builder.HasOne<Patient>(p => p.IdPatientNavigation)
            .WithMany(pat => pat.Prescriptions)
            .HasForeignKey(p => p.IdPatient);
    }
}