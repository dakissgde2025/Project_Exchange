using System.ComponentModel.DataAnnotations;

namespace Project_Exchange.Models
{
    public class AppointmentFormViewModel
    {
        [Required(ErrorMessage = "Kérjük válasszon időpontot.")]
        [Display(Name = "Kívánt időpont")]
        public DateTime? AppointmentDateTime { get; set; }

        [Required(ErrorMessage = "Kérjük írja le röviden a tárgyat.")]
        [StringLength(255, ErrorMessage = "A leírás legfeljebb 255 karakter lehet.")]
        [Display(Name = "Tárgyleírás")]
        public string Note { get; set; } = string.Empty;
    }

    public class AppointmentListItemViewModel
    {
        public DateTime AppointmentDateTime { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime Created { get; set; }
    }

    public class AppointmentPageViewModel
    {
        public IReadOnlyCollection<AppointmentListItemViewModel> UpcomingAppointments { get; set; } = Array.Empty<AppointmentListItemViewModel>();
        public AppointmentFormViewModel Form { get; set; } = new();
    }
}

