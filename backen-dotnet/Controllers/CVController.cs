using backen_dotnet.Dtos.Resume;
using backen_dotnet.Interfaces;
using backen_dotnet.Models.Resume;
using backen_dotnet.Repository;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backen_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CVController : ControllerBase
    {
        private readonly ICVRepository _cvRepository;
        public CVController(ICVRepository cvRepository)
        {
            _cvRepository = cvRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCv([FromBody] CreateCvDto cvDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Utilisation du repository pour créer le CV
            var cv = await _cvRepository.CreateCvAsync(cvDto);

            // Générer le PDF après avoir sauvegardé les données
           // var pdfBytes = GeneratePdf(cv);

            // Retourner le PDF comme fichier téléchargeable
            return Ok(cv);
        }
        private string GenerateHtmlForCv(CV cv)
        {
            return $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    h1, h2, h3 {{ margin: 0; }}
                    p {{ margin: 0 0 10px; }}
                </style>
            </head>
            <body>
                <h1>{cv.Contact.FirstName} {cv.Contact.LastName}</h1>
                <h3>{cv.Contact.JobTitle}</h3>
                <p>Adresse : {cv.Contact.Adress}, {cv.Contact.Ville}, {cv.Contact.ZipCode}</p>
                <p>Email : {cv.Contact.Email} | Téléphone : {cv.Contact.Phone}</p>

                <h2>Expérience</h2>
                    <h3>{cv.WorkExperience.Position} chez {cv.WorkExperience.CompanyName}</h3>
                    <p>{cv.WorkExperience.StartMonth} {cv.WorkExperience.StartYear} - {cv.WorkExperience.EndMonth} {cv.WorkExperience.EndYear}</p>
              

                <h2>Éducation</h2>
                    <h3>{cv.Education.Degree} en {cv.Education.FieldofStudy}</h3>
                    <p>{cv.Education.SchoolName} ({cv.Education.SchoolLocation})</p>
                    <p>{cv.Education.StartMonth} {cv.Education.StartYear} - {cv.Education.EndMonth} {cv.Education.EndYear}</p>

                <h2>Compétences</h2>
                <ul>
                    {string.Join("", cv.SkillsList.Select(skill => $"<li>{skill}</li>"))}
                </ul>
            </body>
            </html>
        ";
        }
        //private byte[] GeneratePdf(CV cv)
        //{
        //    var htmlContent = GenerateHtmlForCv(cv);

        //    var pdfDocument = new HtmlToPdfDocument
        //    {
        //        GlobalSettings = new GlobalSettings
        //        {
        //            ColorMode = ColorMode.Color,
        //            Orientation = Orientation.Portrait,
        //            PaperSize = PaperKind.A4
        //        },
           
        //    };
        //    // Ajoute les ObjectSettings après la création du pdfDocument
        //    pdfDocument.Objects.Add(new ObjectSettings
        //    {
        //        PagesCount = true,
        //        HtmlContent = htmlContent,
        //        WebSettings = { DefaultEncoding = "utf-8" }
        //    });
        //    return _converter.Convert(pdfDocument);
        //}
    }
}
