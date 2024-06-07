namespace Ef_CodeFirst.DTOs;

public class PatientDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
    public IEnumerable<PrescriptionDTO> Prescriptions { get; set; }
}

public class MedicamentDTO
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
}

public class DoctorDTO
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
}

public class PrescriptionDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public IEnumerable<MedicamentDTO> Medicaments { get; set; }
    public DoctorDTO Doctor { get; set; }
}