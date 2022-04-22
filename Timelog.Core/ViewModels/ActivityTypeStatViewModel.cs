using System.ComponentModel.DataAnnotations;

namespace Timelog.Core.ViewModels
{
    public class ActivityTypeStatViewModel: TotalStatisticsVewModel
    {
        [Display(Name = "Тип деятельности")]
        public string ActivityTypeName { get; set; } = String.Empty;
        public long ActivityTypeId { get; set; }
    }
}
