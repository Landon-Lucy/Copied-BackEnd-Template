using System;
using System.Collections.Generic;

namespace ClassLibrary.DTOs
{
    public class EmployeeRoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<RoleDTO> Roles { get; set; } = new();
    }

    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<PermissionDTO> Permissions { get; set; } = new();
    }

    public class PermissionDTO
    {
        public int Id { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string? PermissionDescription { get; set; }
    }
}