namespace Ef_CodeFirst.DTOs;

public class AddPrescriptionDTO
{
    public AddPatientDTO Patient { get; set; }
    public IEnumerable<MedicamentDTO> Medicaments { get; set; }
    public int IdDoctor { get; set; } 
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}

public class AddPatientDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
}