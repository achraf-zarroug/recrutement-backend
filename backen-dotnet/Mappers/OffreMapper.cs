using backen_dotnet.Dtos.Offre;
using backen_dotnet.Models;
using System.Runtime.CompilerServices;

namespace backen_dotnet.Mappers
{
    public static class OffreMapper
    {
        public static OffreDto ToOfferDto(this Offre offerModel)
        {
            return new OffreDto
            {
                Id = offerModel.Id,
                Titre = offerModel.Titre,
                Description = offerModel.Description,
                Qualifications = offerModel.Qualifications,
                Responsibilities = offerModel.Responsibilities,
                DateDebut = offerModel.DateDebut,
                PostedDate = offerModel.PostedDate,
                RequiredSkills = offerModel.RequiredSkills,
                RequiredLanguages = offerModel.RequiredLanguages,
                Benefits = offerModel.Benefits,
                EstStage = offerModel.EstStage,
                Departement = offerModel.Departement,
                Lieu = offerModel.Lieu,
                ContractType = offerModel.ContractType,
                ContractDuration = offerModel.ContractDuration,
                Gender = offerModel.Gender,
                NiveauEtudesRequis = offerModel.NiveauEtudesRequis,
                EstActive = offerModel.EstActive,
            };
        }
        public static Offre ToOfferFromCreateDto(this CreateOffreDto offreDto)
        {
            return new Offre
            {
                Titre = offreDto.Titre,
                Description = offreDto.Description,
                Qualifications = offreDto.Qualifications,
                Responsibilities = offreDto.Responsibilities,
                DateDebut = offreDto.DateDebut,
                PostedDate = offreDto.PostedDate,
                RequiredSkills = offreDto.RequiredSkills,
                RequiredLanguages = offreDto.RequiredLanguages,
                Benefits = offreDto.Benefits,
                EstStage = offreDto.EstStage,
                Departement= offreDto.Departement,
                Lieu= offreDto.Lieu,
                ContractType = offreDto.ContractType,
                ContractDuration = offreDto.ContractDuration,

                Gender = offreDto.Gender,
                NiveauEtudesRequis = offreDto.NiveauEtudesRequis,
                EstActive= offreDto.EstActive,

            };
        }
    }
}
