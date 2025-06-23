using backen_dotnet.Data;
using backen_dotnet.Dtos.Account;
using backen_dotnet.Dtos.Interview;
using backen_dotnet.Interfaces;
using backen_dotnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backen_dotnet.Repository
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly ApplicationDbContext _context;
        public InterviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<bool> IsInterviewSlotAvailable(DateTime interviewDateTime, int offreId)
        {
            // Vérifier si un entretien existe déjà pour l'Offre à la même heure
            var interviewExists = await _context.Interviews
                                                 .AnyAsync(i => i.OffreId == offreId && i.AppointmentDateTime == interviewDateTime);
            return !interviewExists; 
        }

        public async Task<bool> ScheduleInterview(string candidatId, int offreId, DateTime interviewDateTime)
        {
            if (!await IsInterviewSlotAvailable(interviewDateTime, offreId))
            {
                return false; 
            }

            var interview = new Models.Interview
            {
                AppUserId = candidatId,
                OffreId = offreId,
                AppointmentDateTime = interviewDateTime,
                Status = "Scheduled"
            };

           await _context.Interviews.AddAsync(interview);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<Interview?> UpdateAsync(int id, UpdateInterviewDto updateDto)
        {
            var existingInterview = await _context.Interviews.FirstOrDefaultAsync(x => x.InterviewId == id);
            if (existingInterview == null)
            {
                return null;
            }

            existingInterview.Link = updateDto.Link;
           

            await _context.SaveChangesAsync();

            return existingInterview;
        }
        public async Task<Interview?> DeleteAsync(int id)
        {
            var interviewModel = await _context.Interviews.FirstOrDefaultAsync(x => x.InterviewId == id);
            if (interviewModel == null)
            {
                return null;
            }
            _context.Interviews.Remove(interviewModel);
            await _context.SaveChangesAsync();
            return interviewModel;
        }
    }
}
