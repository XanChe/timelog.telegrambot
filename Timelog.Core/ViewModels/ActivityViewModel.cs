using System.ComponentModel.DataAnnotations;
using Timelog.Core.Entities;

namespace Timelog.Core.ViewModels
{
    public class ActivityViewModel
    {
        public ActivityStatus Status { get; set; } 
        public Guid Id { get; set; }
        [Display(Name = "Начало")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Окончание")]
        public DateTime EndTime { get; set; }
        [Display(Name = "Длительность")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan Duration { get; set; }
        [Display(Name = "Статус")]
        public string StatusAsString { get; set; } = String.Empty;
        [Display(Name = "Заголовок")]
        public string Title { get; set; } = String.Empty;
        [Display(Name = "Комментарий")]
        public string Comment { get; set; } = String.Empty;
        public Guid ProjectId { get; set; }
        [Display(Name = "Проект")]
        public string ProjectName { get; set; } = String.Empty;
        [Display(Name = "Тип деятельности")]
        public string ActivityTypeName { get; set; } = String.Empty;
        public Guid ActivityTypeId { get; set; }

        public static implicit operator ActivityViewModel(UserActivity activity)
        {
            return ActivityViewModel.MapFromUserActivity(activity); 
        }
        public static ActivityViewModel MapFromUserActivity(UserActivity activity)
        {
            //if (activity != null)
            //{
                var endTime = activity.Status == ActivityStatus.Started ? DateTime.Now : activity.EndTime;
                var duration = endTime - activity.StartTime;
                return new ActivityViewModel()
                {
                    Title = activity.Title,
                    StartTime = activity.StartTime,
                    EndTime = activity.EndTime,
                    Duration = duration,
                    Comment = activity.Comment,
                    Id = activity.Id,
                    Status = activity.Status,
                    StatusAsString = activity.Status.ToString(),
                    ProjectId = activity.ProjectId,
                    ProjectName = activity?.Project?.Name ?? "",
#nullable disable
                    ActivityTypeId = activity.ActivityTypeId,
#nullable enable
                    ActivityTypeName = activity?.ActivityType?.Name ?? ""
                };
            //}            
        }

        public UserActivity MapToUserActivity()
        {
            return new UserActivity()
            {
                Title = this.Title,
                StartTime = this.StartTime,
                EndTime = this.EndTime,
                Comment = this.Comment,
                Id = this.Id,
                Status = this.Status,
                ProjectId = this.ProjectId,
                ActivityTypeId = this.ActivityTypeId
                
            };
        }


    }
}
