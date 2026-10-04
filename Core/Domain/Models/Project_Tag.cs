using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    [PrimaryKey("ProjectId","TagId")]              //Install EfCore
    public class Project_Tag
    {



        // Project <Has> Project_Tag 
        public Project Project { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }       //FK  //pk





        // Tag <Has> Project_Tag 
        public Tag Tag { get; set; }

        [ForeignKey("Tag")]
        public int TagId { get; set; }         //FK    //pk

    }
}
