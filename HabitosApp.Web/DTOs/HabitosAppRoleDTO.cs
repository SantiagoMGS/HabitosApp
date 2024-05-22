﻿using System.ComponentModel.DataAnnotations;

namespace HabitosApp.Web.DTOs
{
    public class HabitosAppRoleDTO
    {
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public List<PermissionForDTO>? Permissions { get; set; }

        public string? PermissionIds { get; set; }
    }
}
