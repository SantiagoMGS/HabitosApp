﻿using System.ComponentModel.DataAnnotations;

namespace HabitosApp.Web.Data.Entities
{
    public class HabitosAppRole
    {
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
