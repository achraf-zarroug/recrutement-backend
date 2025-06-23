using backen_dotnet.Dtos.Interview;
using backen_dotnet.Models;

namespace backen_dotnet.Interfaces
{
    public interface IInterviewRepository
    {
        Task<bool> IsInterviewSlotAvailable(DateTime interviewDateTime, int offreId);
        Task<bool> ScheduleInterview(string candidatId, int offreId, DateTime interviewDateTime);
        Task<Interview?> UpdateAsync(int id, UpdateInterviewDto updateDto);
        Task<Interview?> DeleteAsync(int id);
    }
}
