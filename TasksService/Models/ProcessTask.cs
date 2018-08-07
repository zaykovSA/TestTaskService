using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasksService.Models
{
    public class ProcessTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaskId { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime StatusChangeDate { get; set; }
    }
}