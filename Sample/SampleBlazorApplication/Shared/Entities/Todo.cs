using System;
using System.ComponentModel.DataAnnotations;

namespace SampleBlazorApplication.Shared.Entities
{
    public class Todo
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
