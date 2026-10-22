import { Table } from 'react-bootstrap';

const RoomListPage= () => {  //Lager komponent som viser romoversikt
  const rooms = [ //Eksempeldata for rommene -Erstattes med data fra API senere
    {
      RoomId: 1,
      RoomName: "Grupperom 1",
      Capacity: 6,
      Building : "P35",
     // ImageUrl: "/images/chickenleg.jpg"
    },
    {
      RoomId: 2,
      RoomName: "Grupperom 2",
      Capacity: 20,
      Building : "P38",
     //ImageUrl: "/images/fishandchips.jpg"
    }
  ];

  console.log(rooms) //feilsøking
  console.log('load rooms')
  // debugger;

  return (
    <div>
      <h1>Rooms</h1>     

      {/*stripede rader, borders gir rammer, hover fremhever rad når musen er over den*/} 
      <Table striped bordered hover>  {/*Tabellens kolonneoverskrifter*/}
      <thead>
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Capacity</th>
          <th>Building</th>
          {/*<th>Images</th>*/}
        </tr>
      </thead>
      <tbody>
        {rooms.map(room => (     //går gjennom romliste og lager en rad for hvert rom
          <tr key={room.RoomId}> {/*Unikt nøkkel for hver rad, her bruker vi RoomId*/}
            <td>{room.RoomId}</td>
            <td>{room.RoomName}</td>
            <td>{room.Capacity}</td>
            <td>{room.Building}</td>
            {/*<td><img src={room.ImageUrl} alt={room.RoomName} width="120" /></td>*/}
          </tr>
        ))}
      </tbody>
      </Table>
    </div>
  );
};
//Gjør komponenten tilgjengelig for import i andre filer
export default RoomListPage;