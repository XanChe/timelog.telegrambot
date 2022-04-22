using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timelog.Core.ViewModels
{
    public class TotalStatisticsVewModel
    {
        [Display(Name = "Количство подходов")]
        public long ActivityCount { get; set; } = 0;
        [Display(Name = "Суммарная длительность")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan DurationTotal { get; set; } 
        [Display(Name = "Средняя длительность")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan DurationAvarage { get; set; } 
        [Display(Name = "Минимальная длительность")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan DurationMin { get; set; } 
        [Display(Name = "Максимальная длительность")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan DurationMax { get; set; } 
        [Display(Name = "Самый ранний старт")]
        public DateTime FirstActivity { get; set; } = DateTime.MinValue;
        [Display(Name = "Самый поздний финиш")]
        public DateTime LastActivity { get; set; } = DateTime.MinValue;
    }
}
