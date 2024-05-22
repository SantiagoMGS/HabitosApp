namespace HabitosApp.Web.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class ViaAdmin
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Se debe especificar la vía de administracion")]
        public string Description { get; set; }

        // Otras propiedades relacionadas con la unidad, si es necesario.
    }
}
