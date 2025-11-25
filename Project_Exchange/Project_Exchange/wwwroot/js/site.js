document.addEventListener('DOMContentLoaded', () => {
    const appointmentInput = document.querySelector('input[name="Form.AppointmentDateTime"]');
    if (!appointmentInput) {
        return;
    }

    const now = new Date();
    const minutes = now.getMinutes();
    const remainder = minutes % 15;
    if (remainder > 0) {
        now.setMinutes(minutes + (15 - remainder));
    }

    now.setMinutes(now.getMinutes() + 60);
    appointmentInput.min = formatForDateTimeLocal(now);
    appointmentInput.value = appointmentInput.value || appointmentInput.min;
});

function formatForDateTimeLocal(date) {
    const pad = (value) => value.toString().padStart(2, '0');
    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}`;
}
