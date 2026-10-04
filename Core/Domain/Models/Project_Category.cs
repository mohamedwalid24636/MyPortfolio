using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class Project_Category
    {
        // Category <Has> Project_Category
        public Category Category { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }        //FK


        // Project <Has> Project_Category
        public Project Project { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }          //FK     //PK




    }
}
