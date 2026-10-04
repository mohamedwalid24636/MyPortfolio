using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class Skill_Type
    {
        // Skill <Has> Skill_Type
        public Skill Skill { get; set; }
        [ForeignKey("Skill")]
        public int SkillId { get; set; }   //FK   //PK

        // Type <Has> Skill_Type
        public Type Type { get; set; }
        [ForeignKey("Type")]
        public int TypeId { get; set; }   //FK
    }
}
