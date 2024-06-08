

using System.Linq;
using Ef_CodeFirst.Context;
using Ef_CodeFirst.DTOs;
using Ef_CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfExample.Controllers;

[Route("api/")]
[ApiController]
public class PrescriptionController : ControllerBase
{
    private readonly AppDbContext _context;
    public PrescriptionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("patient/{idPatient}")]
    public async Task<IActionResult> GetPrescriptionsAsync(int idPatient)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.IdMedicamentNavigation)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.IdDoctorNavigation)
            .Where(p => p.IdPatient == idPatient)
            .Select(p => new PatientDTO()
            {
                IdPatient = p.IdPatient,
                FirstName = p.FirstName,
                LastName =  p.LastName,
                Birthdate = p.Birthdate,
                Prescriptions = p.Prescriptions.Select(pre => new PrescriptionDTO()
                {
                    IdPrescription = pre.IdPrescription,
                    Date = pre.Date,
                    DueDate = pre.DueDate,
                    Doctor = new DoctorDTO()
                    {
                        IdDoctor = pre.IdDoctorNavigation.IdDoctor,
                        FirstName = pre.IdDoctorNavigation.FirstName
                    },
                    Medicaments = pre.PrescriptionMedicaments.Select(pm => new MedicamentDTO()
                    {
                        IdMedicament = pm.IdMedicamentNavigation.IdMedicament,
                        Name = pm.IdMedicamentNavigation.Name,
                        Dose = pm.Dose,
                        Description = pm.IdMedicamentNavigation.Description
                    })
                })
            })
            .FirstAsync();
        return Ok(patient);
    }

    [HttpPut("prescription")]
    public async Task<IActionResult> AddPrescription(AddPrescriptionDTO addPrescriptionDto)
    {
        var patient = await _context.Patients
            .FindAsync(addPrescriptionDto.Patient.IdPatient);

        if (patient is null)
        {
            var maxId = await _context.Patients.MaxAsync(p => p.IdPatient);
            var newPatient = new Patient()
            {
                FirstName = addPrescriptionDto.Patient.FirstName,
                LastName = addPrescriptionDto.Patient.LastName,
                Birthdate = addPrescriptionDto.Patient.Birthdate
            };
            await _context.Patients.AddAsync(newPatient);
            await _context.SaveChangesAsync();
            patient = await _context.Patients
                .FindAsync(addPrescriptionDto.Patient.IdPatient);
        }

        foreach (var medicament in addPrescriptionDto.Medicaments)
        {
            var medicamentRow = await _context.Medicaments
                .FindAsync(medicament.IdMedicament);
            if (medicamentRow is null) return NotFound("Nie znaleziono leku o Id: " + medicament.IdMedicament);
        }

        if (addPrescriptionDto.Medicaments.Count() > 10) return Conflict("Recepta moze zawierac max 10 lekow.");

        if (addPrescriptionDto.DueDate < addPrescriptionDto.Date) return Conflict("Data waznosci nie moze byc wczesniejsza niz data wystawienia.");

        var newPrescription = new Prescription()
        {
            IdDoctor = addPrescriptionDto.IdDoctor,
            IdPatient = patient.IdPatient,
            Date = addPrescriptionDto.Date,
            DueDate = addPrescriptionDto.DueDate
        };
        await _context.Prescriptions.AddAsync(newPrescription);
        await _context.SaveChangesAsync();

        foreach (var medicament in addPrescriptionDto.Medicaments)
        {
            var newPrescriptionMedicament = new PrescriptionMedicament()
            {
                IdMedicament = medicament.IdMedicament,
                IdPrescription = newPrescription.IdPrescription,
                Details = medicament.Description,
                Dose = medicament.Dose
            };
            await _context.PrescriptionMedicaments.AddAsync(newPrescriptionMedicament);
            await _context.SaveChangesAsync();
        }
        
        return Ok();
    }
}
