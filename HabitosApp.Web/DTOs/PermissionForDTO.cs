using HabitosApp.Web.Data.Entities;

namespace HabitosApp.Web.DTOs
{
    public class PermissionForDTO : Permission
    {
        public bool Selected { get; set; } = false;
    }
}
