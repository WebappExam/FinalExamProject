import { useState } from 'react';
import { Table, Form } from 'react-bootstrap';

const RoomListPage = () => {
  // Husker teksten brukeren skriver i søkefeltet
  const [searchQuery, setSearchQuery] = useState('');

  // Eksempeldata – erstattes med data fra API senere
  const rooms = [
    {
      RoomId: 1,
      RoomName: 'Grupperom 1',
      Capacity: 6,
      Building: 'P35'
    },
    {
      RoomId: 2,
      RoomName: 'Grupperom 2',
      Capacity: 10,
      Building: 'P36'
    },
    {
      RoomId: 3,
      RoomName: 'Grupperom 3',
      Capacity: 8,
      Building: 'P37'
    }
  ];

  // Velger rom som inneholder søketeksten i navnet
  const filteredRooms = rooms.filter(room =>
    room.RoomName.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <div>
      <h1>Rooms</h1>

      {/* Oppdaterer søketeksten når brukeren skriver */}
      <Form.Group className="mb-3" controlId="roomSearch">
        <Form.Label>Search by room name</Form.Label>
        <Form.Control
          type="text"
          placeholder="Enter room name"
          value={searchQuery}
          onChange={e => setSearchQuery(e.target.value)}
        />
      </Form.Group>

      <Table striped bordered hover>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Capacity</th>
            <th>Building</th>
          </tr>
        </thead>

        <tbody>
          {/* Lager en rad for hvert rom som matcher søketeksten */}
          {filteredRooms.map(room => (
            <tr key={room.RoomId}>
              <td>{room.RoomId}</td>
              <td>{room.RoomName}</td>
              <td>{room.Capacity}</td>
              <td>{room.Building}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

// Gjør komponenten tilgjengelig for import i andre filer
export default RoomListPage;