using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebShop.Data;

namespace WebShop.Models
{
    public enum TaskStatus
    {
        Open,
        InProgress,
        Done,
        Cancelled
    }

    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }

        // Assignee (user): can be null (unassigned)
        public string AssigneeId { get; set; }
        [ForeignKey(nameof(AssigneeId))]
        public virtual ApplicationUser Assignee { get; set; }

        // Document reference (required)
        public int DocumentId { get; set; }
        [ForeignKey(nameof(DocumentId))]
        public virtual Document Document { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        [Required]
        public TaskStatus Status { get; set; } = TaskStatus.Open;

        // Who created or assigned the task
        public string CreatedByUserId { get; set; }
        [ForeignKey(nameof(CreatedByUserId))]
        public virtual ApplicationUser CreatedByUser { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}