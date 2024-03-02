﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.ManagementModels
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }

        [Key]
        public string ProjectUniqueId { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string ProjectDescription { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Required]
        public TypeOfDatabase TypeOfDatabase { get; set; }

        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string DatabaseName { get; set; }

        [Required]
        public TrueFalse MigrateDatabase { get; set; }

        [Required]
        public TrueFalse Status { get; set; }
    }
}
