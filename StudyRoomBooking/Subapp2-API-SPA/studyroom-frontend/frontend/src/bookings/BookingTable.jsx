import { Table, Button } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

// Viser alle bookinger i en tabell.
const BookingTable = ({ bookings, rooms = [], onBookingDeleted }) => {
  const navigate = useNavigate();


  // Viser måned.dag.år og klokkeslett i 24-timersformat.
  const formatDateTime = (value) => {
    const date = new Date(value);

    if (Number.isNaN(date.getTime())) {
      return 'Ugyldig tidspunkt';
    }

    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    return `${month}.${day}.${year} kl ${hours}:${minutes}`;
  };

  return (
    <Table striped hover responsive className="room-results-table">
      <thead>
        <tr>
          <th>Rom</th>
          <th>Starttid</th>
          <th>Sluttid</th>
          <th>Kurs</th>
          <th>Beskrivelse</th>
          <th>Handlinger</th>

        </tr>
      </thead>

      <tbody>
        {/* Lager en rad for hver booking. */}
        {bookings.map(booking => (
          <tr key={booking.Id}>
            <td>
              {rooms.find(room =>
                Number(room.RoomId) === Number(booking.RoomId)
              )?.RoomName ?? 'Ukjent rom'}
            </td>
            <td>{formatDateTime(booking.StartTime)}</td>
            <td>{formatDateTime(booking.EndTime)}</td>
            <td>{booking.Course}</td>
            <td>{booking.Description}</td>
              <td>
                <div className="booking-actions">
                  <Button
                    type="button"
                    variant="outline-dark"
                    size="sm"
                    onClick={() => navigate(`/bookingupdate/${booking.Id}`)}
                  >
                    Rediger
                  </Button>

                  <Button
                    type="button"
                    variant="outline-danger"
                    size="sm"
                    onClick={() => onBookingDeleted(booking.Id)}
                  >
                    Slett
                  </Button>
                </div>
              </td>
          </tr>
        ))}
      </tbody>
    </Table>
  );
};

export default BookingTable;
