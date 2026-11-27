const API_URL = import.meta.env.VITE_API_URL;
const headers = {
    'Content-Type': 'application/json',
};

// Kontrollerer svaret før JSON-dataene returneres.
async function handleResponse(response) {
    if (!response.ok) {
        throw new Error(`Forespørselen feilet (status ${response.status}).`);
    }

    return response.json();
}

// Henter alle bookinger fra API-et.
export async function fetchBookings() {
    const response = await fetch(`${API_URL}/api/bookings`);
    return handleResponse(response);
}
// Oppretter en ny booking gjennom API-et.
export async function createBooking(booking) {
    const response = await fetch(`${API_URL}/api/bookings`, {
        method: 'POST',
        headers,
        body: JSON.stringify({
            roomId: booking.RoomId,
            organizerId: booking.OrganizerId,
            startTime: booking.StartTime,
            endTime: booking.EndTime,
            course: booking.Course,
            description: booking.Description
        }),
    });

    return handleResponse(response);
}
// Henter en bestemt booking fra API-et.
export async function fetchBookingById(bookingId) {
    const response = await fetch(`${API_URL}/api/bookings/${bookingId}`);
    return handleResponse(response);
}

// Oppdaterer en eksisterende booking gjennom API-et.
export async function updateBooking(booking) {
    const response = await fetch(`${API_URL}/api/bookings/${booking.Id}`, {
        method: 'PUT',
        headers,
        body: JSON.stringify({
            roomId: booking.RoomId,
            startTime: booking.StartTime,
            endTime: booking.EndTime,
            course: booking.Course,
            description: booking.Description
        }),
    });

    return handleResponse(response);
}

// Sletter en eksisterende booking gjennom API-et.
export async function deleteBooking(bookingId) {
    const response = await fetch(`${API_URL}/api/bookings/${bookingId}`, {
        method: 'DELETE',
    });

    if (!response.ok) {
        throw new Error(`Bookingen kunne ikke slettes (status ${response.status}).`);
    }
}
