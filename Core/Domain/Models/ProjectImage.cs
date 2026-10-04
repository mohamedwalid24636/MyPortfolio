using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class ProjectImage : BaseEntity<int>
    {
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
        public string DisplayOrder { get; set; }

       

        // Project <Has> ProjectImage
        public Project Project { get; set; }
        
        [ForeignKey("Project")]
        public int ProjectId { get; set; }    //FK
    }
}
