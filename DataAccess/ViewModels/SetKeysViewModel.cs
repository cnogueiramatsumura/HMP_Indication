using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class SetKeysViewModel
    {
        [Required]
        public string binanceKey { get; set; }
        [Required]
        public string binanceSecret { get; set; }
    }
}