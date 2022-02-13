using System.ComponentModel.DataAnnotations;

namespace PollApp.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}