using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Entities
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public DateTime AppointmentDateTime { get; set; }

        [Required]
        [MaxLength(255)]
        public string Note { get; set; } = string.Empty;

        [Required]
        public int AppUser_ID { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime? LastModified { get; set; }

        // Navigation property for the user
        [ForeignKey("AppUser_ID")]
        public virtual AppUser AppUser { get; set; } = null!;
    }
}
