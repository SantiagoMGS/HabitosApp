using System.ComponentModel.DataAnnotations;

namespace HabitosApp.Web.DTOs
{
    public class HabitTypeDto
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

    }
}
