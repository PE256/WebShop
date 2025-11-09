using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    public enum DocumentStatus
    {
        Draft,
        InReview,
        Approved,
        Rejected
    }

    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        // Content can be large
        [Column(TypeName = "text")]
        public string Content { get; set; }

        [Required]
        public DocumentStatus Status { get; set; } = DocumentStatus.Draft;

        // Navigation: tasks that reference this document
        public virtual ICollection<TaskItem> Tasks { get; set; }
    }
}