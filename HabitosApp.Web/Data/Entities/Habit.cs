namespace HabitosApp.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Habit
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public int HabitTypeId { get; set; }

    [ForeignKey("HabitTypeId")]
    public HabitType HabitType { get; set; }

    // Luego le ponemos frecuencia o algo as�, por ahora as�.
    // Otras propiedades comunes
}