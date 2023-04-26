using System.ComponentModel.DataAnnotations;

namespace BusinesssSelenium.Models
{
    public class Model
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserCode { get; set; }
    }
}
