﻿namespace Portfolio.API.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProjectImage
    {
        [Key]
        public int Id { get; set; }

        public string ImageUrl { get; set; }


        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }

        public virtual Project Project{ get; set; }
    }
}