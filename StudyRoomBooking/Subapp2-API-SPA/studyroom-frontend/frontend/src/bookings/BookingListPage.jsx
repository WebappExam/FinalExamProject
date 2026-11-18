
import BookingTable from './BookingTable';

// Viser oversikten over alle bookinger.
const BookingListPage = ({ bookings = [], rooms = [], onBookingDeleted }) => {

    // Ber om bekreftelse før bookingen slettes.
    const handleDelete = (bookingId) => {
        const confirmed = window.confirm(
            'Er du sikker på at du vil slette denne bookingen?'
        );

        if (!confirmed) {
            return;
        }

        onBookingDeleted(bookingId);
    };

    return (
        <div className="text-start">
            <h1>Oversikt over dine bookinger</h1>

            {/* Viser melding dersom det ikke finnes bookinger. */}
            {bookings.length === 0 ? (
                <p>Det finnes ingen bookinger ennå.</p>
            ) : (
                <BookingTable bookings={bookings}
                 rooms={rooms}
                 onBookingDeleted={handleDelete}
                />
            )}
        </div>
    );
};

export default BookingListPage;
