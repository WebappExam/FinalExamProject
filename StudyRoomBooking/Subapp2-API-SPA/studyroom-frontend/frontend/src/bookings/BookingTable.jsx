import { Table, Button } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

// Viser alle bookinger i en tabell.
const BookingTable = ({ bookings, onBookingDeleted }) => {
  const navigate = useNavigate();

  return (
    <Table striped bordered hover>
      <thead>
        <tr>
          <th>ID</th>
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
            <td>{booking.Id}</td>
            <td>{booking.RoomId}</td>
            <td>{booking.StartTime}</td>
            <td>{booking.EndTime}</td>
            <td>{booking.Course}</td>
            <td>{booking.Description}</td>
            <td>
              <Button
                variant="warning"
                size="sm"
                onClick={() => navigate(`/bookingupdate/${booking.Id}`)}
              >
                Rediger
              </Button>
              {/* Sender valgt booking videre til sletting. */}
              <Button
                type="button"
                variant="danger"
                size="sm"
                className="ms-2"
                onClick={() => onBookingDeleted(booking.Id)}
              >
                Slett
              </Button>

            </td>
          </tr>
        ))}
      </tbody>
    </Table>
  );
};

export default BookingTable;
