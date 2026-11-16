import { Table, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';

// Mottar romlisten fra RoomListPage gjennom props.
const RoomTable = ({ rooms, onRoomDeleted }) => {
  return (
    <Table striped bordered hover>
      <thead>
        <tr>
          <th>ID</th>
          <th>Romnavn</th>
          <th>Kapasitet</th>
          <th>Bygg</th>
          <th>Handlinger</th>

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
    to={`/roomupdate/${room.RoomId}`}
    className="btn btn-primary btn-sm"
  >
    Rediger
  </Link>

  <Button
  type="button"
  variant="danger"
  size="sm"
  className="ms-2"
  onClick={() => onRoomDeleted(room.RoomId)}
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

export default RoomTable;