using Ef_CodeFirst.EfConfigurations;
using Ef_CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace Ef_CodeFirst.Context;

public class AppDbContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    public AppDbContext() {}
    
    public AppDbContext(DbContextOptions options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.ApplyConfiguration(new DoctorEfConfiguration());
        modelBuilder.ApplyConfiguration(new MedicamentEfConfiguration());
        modelBuilder.ApplyConfiguration(new PatientEfConfiguration());
        modelBuilder.ApplyConfiguration(new PrescriptionEfConfiguration());
        modelBuilder.ApplyConfiguration(new PrescriptionMedicamentEfConfiguration());
        
    }
}