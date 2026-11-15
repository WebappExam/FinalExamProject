import { useState } from 'react'
import RoomCreatePage from './rooms/RoomCreatePage'
import RoomUpdatePage from './rooms/RoomUpdatePage'

//Henter som styrer navigasjon mellom sider i appen
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
//Henter bootstrap komponenter for styling
import Container from 'react-bootstrap/Container'
//Henter komponenter for de ulike sidene i appen
import HomePage from './home/HomePage'
import RoomListPage from './rooms/RoomListPage'
import NavMenu from './shared/NavMenu'
import './App.css'

function App() {

    // Midlertidige lokasjoner til testing før API-et er klart.
  const locations = [
    { LocationId: 1, Name: 'Pilestredet' },
    { LocationId: 2, Name: 'Kjeller' },
    { LocationId: 3, Name: 'Holmlia' }
  ];

  // Felles romliste for visning, oppretting og senere redigering/sletting.
  const [rooms, setRooms] = useState([
    {
      RoomId: 1,
      RoomName: 'Grupperom 1',
      Capacity: 6,
      Building: 'P35',
      RoomType: 'Grupperom',
      LocationId: 1
    },
    {
      RoomId: 2,
      RoomName: 'Grupperom 2',
      Capacity: 20,
      Building: 'P38',
      RoomType: 'Grupperom',
      LocationId: 1
    }
  ]);

  // Legger til rommet uten å endre den eksisterende listen direkte.
  const createRoom = (room) => {
    setRooms(currentRooms => {
      const nextId =
        Math.max(0, ...currentRooms.map(item => item.RoomId)) + 1;

      return [...currentRooms, { ...room, RoomId: nextId }];
    });
  };

  // Erstatter bare rommet som har samme ID som det redigerte rommet.
const updateRoom = (updatedRoom) => {
  setRooms(currentRooms =>
    currentRooms.map(room =>
      room.RoomId === updatedRoom.RoomId ? updatedRoom : room
    )
  );
};

// Fjerner rommet med valgt ID fra den midlertidige romlisten.
const deleteRoom = (roomId) => {
  setRooms(currentRooms =>
    currentRooms.filter(room => room.RoomId !== roomId)
  );
};




  return (
    <>
    <Container>
    <NavMenu /> {/*Viser navigasjonsmeny*/}
        <Router> {/*Router styrer navigasjon mellom sider i appen*/ }
            <Routes>
              <Route path="/" element={<HomePage />} /> {/*Viser HomePage*/ }
               {/*Viser Rooms*/ }<Route
               path="/rooms"
              element={
                <RoomListPage
                   rooms={rooms}
                    onRoomDeleted={deleteRoom}
                      />
                        }
                        />

<Route
  path="/roomcreate"
  element={
    <RoomCreatePage
      locations={locations}
      onRoomCreated={createRoom}
    />
  }
/>

<Route
  path="/roomupdate/:roomId"
  element={
    <RoomUpdatePage
      rooms={rooms}
      locations={locations}
      onRoomUpdated={updateRoom}
    />
  }
/>

              <Route path="*" element={<Navigate to="/" replace />} /> {/*Hvis ingen ruter matcher, naviger til HomePage*/ }
            </Routes>
        </Router>
    </Container>
    </>
  )
}

export default App