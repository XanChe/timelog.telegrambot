using System;
using System.ComponentModel.DataAnnotations;

namespace Timelog.Core.Entities
{
    public enum ActivityStatus
    {
        Draft,
        Started,
        Complite
    }
    public class UserActivity : Entity
    {
        public UserActivity() { }
        public UserActivity(UserActivity other, Project project, ActivityType activityType)
        {
            this.Title = other.Title;
            this.StartTime = other.StartTime;
            this.EndTime = other.EndTime;
            this.Comment = other.Comment;
            this.Id = other.Id;
           // this.UniqId = other.UniqId;
            this.UserUniqId = other.UserUniqId;
            this.Status = other.Status;
            this.Project = project;
            this.ActivityType = activityType;
        }        

        [Display(Name = "Начало")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Окончание")]
        public DateTime EndTime { get; set; }
        [Display(Name = "Статус")]
        public ActivityStatus Status { get; set; } = ActivityStatus.Draft;
        [Display(Name = "Заголовок")]
        public string Title { get; set; } = String.Empty;
        [Display(Name = "Комментарий")]
        public string Comment { get; set; } = String.Empty;
        public Guid ProjectId { get; set; }
#nullable enable
        [Display(Name = "Проект")] 
        public Project? Project { get; set; }
        [Display(Name = "Тип деятельности")]
        public ActivityType? ActivityType { get; set; }
#nullable restore
        public Guid ActivityTypeId { get; set; }

    }
}

