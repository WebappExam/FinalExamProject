import { Table, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';

// Mottar romlisten fra RoomListPage gjennom props.
const RoomTable = ({ rooms, onRoomDeleted, isAdmin = false }) => {
  return (
    <Table striped hover responsive className="room-results-table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Romnavn</th>
          <th>Kapasitet</th>
          <th>Bygg</th>
          <th>Booking</th>
          {isAdmin && <th>Handlinger</th>}

        </tr>
      </thead>

      <tbody>
        {/* Lager en tabellrad for hvert rom i listen. */}
        {rooms.map(room => (
          <tr key={room.RoomId}>
            <td>{room.RoomId}</td>
            <td>{room.RoomName}</td>
            <td>{room.Capacity}</td>
            <td>{room.Building}</td>
            <td>
              <Link
                to={`/bookingcreate?roomId=${room.RoomId}`}
                className="btn btn-outline-dark btn-sm"
              >
                Book rom
              </Link>
            </td>


            {isAdmin && (
              <td>
                <div className="d-flex flex-wrap gap-2">
                  <Link
                    to={`/roomupdate/${room.RoomId}`}
                    className="btn btn-outline-dark btn-sm"
                  >
                    Rediger
                  </Link>

                  <Button
                    type="button"
                    variant="outline-danger"
                    size="sm"
                    onClick={() => onRoomDeleted(room.RoomId)}
                  >
                    Slett
                  </Button>
                </div>
              </td>
            )}

          </tr>
        ))}
      </tbody>
    </Table>
  );
};

export default RoomTable;