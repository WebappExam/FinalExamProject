import { Table } from 'react-bootstrap';

// Mottar romlisten fra RoomListPage gjennom props.
const RoomTable = ({ rooms }) => {
  return (
    <Table striped bordered hover>
      <thead>
        <tr>
          <th>ID</th>
          <th>Romnavn</th>
          <th>Kapasitet</th>
          <th>Bygg</th>
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
          </tr>
        ))}
      </tbody>
    </Table>
  );
};

export default RoomTable;