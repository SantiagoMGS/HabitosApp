namespace HabitosApp.Web.Data.Entities
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public HabitosAppRole Role { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
