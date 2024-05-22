using System.ComponentModel.DataAnnotations;

namespace HabitosApp.Web.Data.Entities
{
    public class HabitType
    {
        public int Id { get; set; }

        [Display(Name="Tipo Habito")]
        [Required( ErrorMessage = "El campo '{0}' es requerido.")]
        public string Description { get; set; }
    }
}

