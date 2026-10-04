using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text;

namespace Domain.Models
{
    [PrimaryKey(nameof(ProjectId), nameof(TechnologyId))]
    public class Project_Technology
    {
        // Project <Has> Project_Technology
        public Project Project { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }          //FK     //PK


        // Technology <Has> Project_Technology
        public Technology Technology { get; set; }
        [ForeignKey("Technology")]
        public int TechnologyId { get; set; }       //FK   //PK

    }
}
