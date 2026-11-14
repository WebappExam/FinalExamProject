
import { useState } from 'react';
import { Table, Form } from 'react-bootstrap';

const RoomListPage = () => {
  // Lagrer det brukeren skriver i søkefeltet
  const [searchQuery, setSearchQuery] = useState('');

  // Eksempeldata - erstattes med data fra API senere
  const rooms = [
    {
      RoomId: 1,
      RoomName: "Grupperom 1",
      Capacity: 6,
      Building: "P35"
    },
    {
      RoomId: 2,
      RoomName: "Grupperom 2",
      Capacity: 20,
      Building: "P38"
    }
  ];

  // Filtrerer rommene basert på søketeksten
  const filteredRooms = rooms.filter(room =>
    room.RoomName.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <div>
      <h1>Rooms</h1>

      {/* Søkefelt for romnavn */}
      <Form.Group className="mb-3" controlId="roomSearch">
        <Form.Label>Search by room name</Form.Label>
        <Form.Control
          type="text"
          placeholder="Enter room name"
          value={searchQuery}
          onChange={e => setSearchQuery(e.target.value)}
        />
      </Form.Group>

      {/* Tabell som viser romoversikten */}
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
          {/* Lager en rad for hvert rom som matcher søket */}
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

// Gjør komponenten tilgjengelig for andre filer
export default RoomListPage;
