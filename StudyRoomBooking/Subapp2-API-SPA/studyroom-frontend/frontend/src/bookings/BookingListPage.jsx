
import BookingTable from './BookingTable';

// Viser oversikten over alle bookinger.
const BookingListPage = ({ bookings = [] }) => {
  return (
    <div className="text-start">
      <h1>Administrer bookinger</h1>

      {/* Viser melding dersom det ikke finnes bookinger. */}
      {bookings.length === 0 ? (
        <p>Det finnes ingen bookinger ennå.</p>
      ) : (
        <BookingTable bookings={bookings} />
      )}
    </div>
  );
};

export default BookingListPage;
