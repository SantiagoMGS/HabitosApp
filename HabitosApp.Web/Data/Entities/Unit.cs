namespace HabitosApp.Web.Data.Entities
{
	using System.ComponentModel.DataAnnotations;

	public class Unit
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre de la unidad es obligatorio.")]
		public string Description { get; set; }

		// Otras propiedades relacionadas con la unidad, si es necesario.
	}
}
