namespace HabitosApp.Web.Data.Entities
{
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Medication
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre del medicamento es obligatorio.")]
		public string Name { get; set; }

		public int UnitId { get; set; }
		[ForeignKey("UnitId")]
		public Unit Unit { get; set; }


		public int ViaAdminId { get; set; }
		[ForeignKey("ViaAdminId")]
		public ViaAdmin ViaAdmin { get; set; }

		[Required(ErrorMessage = "La cantidad del medicamento es obligatoria.")]
		[Range(0.1, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
		public float Quantity { get; set; }
	}
}
