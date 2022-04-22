using System;
using System.ComponentModel.DataAnnotations;

namespace Timelog.Core.Entities
{
    public class ActivityType : Entity
    {
        [Display(Name = "Название")]
        public string Name { get; set; } = String.Empty;
        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }
}  
