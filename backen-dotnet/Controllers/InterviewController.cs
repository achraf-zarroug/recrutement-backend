using Azure.Core;
using backen_dotnet.Data;
using backen_dotnet.Dtos.Account;
using backen_dotnet.Dtos.Interview;
using backen_dotnet.Interfaces;
using backen_dotnet.Models;
using backen_dotnet.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backen_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly ApplicationDbContext _context;
         private readonly IMailService _mailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOffreRepository _offreRepository;
        public InterviewController(IInterviewRepository interviewRepository,
                                   ApplicationDbContext context,
                                   IMailService mailService,
                                    UserManager<AppUser> userManager,
                                    IOffreRepository offreRepository)
        {
            _interviewRepository = interviewRepository;
            _context = context;
            _mailService = mailService;
            _userManager = userManager;
            _offreRepository = offreRepository;
        }

        // Créer un entretien
        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleInterview([FromBody] InterviewRequest request)
        {
            var existingInterview = await _context.Interviews
       .FirstOrDefaultAsync(i => i.AppUserId == request.AppUserId && i.OffreId == request.OffreId);
            var appUser = await _userManager.FindByIdAsync(request.AppUserId);
            var offre = await _offreRepository.GetByIdAsync(request.OffreId);
            if (existingInterview != null)
            {
                // Retourne un message d'erreur si l'entretien existe déjà
                return BadRequest("An interview already exists for this user and offer.");
            }

            var isSlotAvailable = await _interviewRepository.ScheduleInterview(request.AppUserId, request.OffreId, request.AppointmentDateTime);

            if (!isSlotAvailable)
            {
                return BadRequest("Le créneau horaire est déjà occupé.");
            }
                    var mailData = new MailData()
        {
            FromEmail = "chrouf.est@gmail.com",
            ToEmails = appUser.Email,
            Subject = "Your Interview Has Been Scheduled - " + offre.Titre,
            Body = @"
            <p>Dear " + appUser.UserName + @",</p>
            <p>We are pleased to inform you that your interview for the position of <strong>" + offre.Titre + @"</strong> at our company has been scheduled. Below are the details:</p>
            <ul>
                <li><strong>Date:</strong> " + request.AppointmentDateTime.ToString("MMMM dd, yyyy") + @"</li>
                <li><strong>Time:</strong> " + request.AppointmentDateTime.ToString("hh:mm tt") + @"</li>
            </ul>
            <p>Please make sure to be prepared and ready on time. This will be an <strong>online interview</strong>. Check your profile , the meeting link will be updated soon .</p>
            <p>If you have any questions or need to reschedule, feel free to contact us at <strong>chrouf.est@gmail.com</strong>.</p>
            <p>We look forward to meeting you and discussing how you can contribute to our team.</p>
            <p>Best regards,<br>The Recruitment Team</p>
        ",
        };


        bool result = await _mailService.SendMailAsync(mailData);
        if (result)
        {
            return Ok(new { message = "Email sent successfully." });
        }
        else
        {
            return StatusCode(500, new { message = "Failed to send email." });
        }
            return Ok("Entretien programmé avec succès.");
        }
        [HttpGet("get-interviews/{appUserId}/{offreId}")]
        public async Task<IActionResult> GetInterviewsByCandidat(string appUserId, int offreId)
        {
            var interviews = await _context.Interviews
            .FirstOrDefaultAsync(i => i.AppUserId == appUserId && i.OffreId == offreId);
            return Ok(interviews);
        }
        [HttpGet("get-interviews-byOffer/{offreId}")]
        public async Task<IActionResult> GetInterviewsByOffer([FromRoute] int offreId)
        {
            var interviews = await _context.Interviews
                .Include(i=> i.AppUser)
             .Include(i => i.Offre)
             .Where(i=> i.OffreId == offreId)
            .ToListAsync();
            Console.WriteLine(interviews);
            return Ok(interviews);
        }
        [HttpGet("get-interviews-byUser/{appUserId}")]
        public async Task<IActionResult> GetInterviewsByOffer([FromRoute] string appUserId)
        {
            var interviews = await _context.Interviews
                .Include(i => i.AppUser)
             .Include(i => i.Offre)
             .Where(i => i.AppUserId == appUserId)
            .ToListAsync();
            Console.WriteLine(interviews);
            return Ok(interviews);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateInterviewDto updateDto)
        {
            var InterviewModel = await _interviewRepository.UpdateAsync(id, updateDto);
            if (InterviewModel == null)
            {
                return NotFound();
            }
            return Ok(InterviewModel);
        }
        [HttpDelete("DeleteInterview/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var interviewModel = await _interviewRepository.DeleteAsync(id);
            if (interviewModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
