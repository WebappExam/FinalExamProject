import { useState } from 'react'
import RoomCreatePage from './rooms/RoomCreatePage'
import RoomUpdatePage from './rooms/RoomUpdatePage'
import RoomListPage from './rooms/RoomListPage'
import LocationCreatePage from './locations/LocationCreatePage'
import LocationListPage from './locations/LocationListPage'
import LocationUpdatePage from './locations/LocationUpdatePage'
import BookingListPage from './bookings/BookingListPage'
import BookingCreatePage from './bookings/BookingCreatePage'
import BookingUpdatePage from './bookings/BookingUpdatePage'
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
import Container from 'react-bootstrap/Container'
import HomePage from './home/HomePage'
import NavMenu from './shared/NavMenu'
import './App.css'


function App() {

  // Midlertidige lokasjoner til testing før API-et er klart.
  const [locations, setLocations] = useState([
    { LocationId: 1, Name: 'Pilestredet' },
    { LocationId: 2, Name: 'Kjeller' },
    { LocationId: 3, Name: 'Holmlia' }
  ]);

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

  // Midlertidige bookinger til testing før API-et er klart.
  const [bookings, setBookings] = useState([
    {
      Id: 1,
      RoomId: 1,
      StartTime: '2026-11-17T10:00',
      EndTime: '2026-11-17T12:00',
      Course: '',
      Description: 'Gruppearbeid'
    }
  ]);


  // Kontrollerer om en booking overlapper med en eksisterende booking.
  const hasOverlappingBooking = (newBooking) => {
    return bookings.some(booking =>
      Number(booking.RoomId) === Number(newBooking.RoomId) &&
      Number(booking.Id) !== Number(newBooking.Id) &&
      new Date(newBooking.StartTime) < new Date(booking.EndTime) &&
      new Date(newBooking.EndTime) > new Date(booking.StartTime)
    );
  };

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

    // Hindrer sletting av rom som har bookinger.
    const hasBookings = bookings.some(
      booking => Number(booking.RoomId) === Number(roomId)
    );

    if (hasBookings) {
      throw new Error('Rommet kan ikke slettes fordi det har bookinger.');
    }

    setRooms(currentRooms =>
      currentRooms.filter(room => room.RoomId !== roomId)
    );
  };

  // Hindrer like lokasjonsnavn og legger til en ny lokasjon.
  const createLocation = (location) => {
    const nameExists = locations.some(item =>
      item.Name.toLowerCase() === location.Name.toLowerCase()
    );

    if (nameExists) {
      throw new Error('En lokasjon med dette navnet finnes allerede.');
    }

    setLocations(currentLocations => {
      const nextId =
        Math.max(0, ...currentLocations.map(item => item.LocationId)) + 1;

      return [
        ...currentLocations,
        { ...location, LocationId: nextId }
      ];
    });
  };

  // Legger til en ny booking i den midlertidige bookinglisten.
  const createBooking = (booking) => {

    // Hindrer at samme rom bookes på overlappende tidspunkter.
    if (hasOverlappingBooking(booking)) {
      throw new Error('Rommet er allerede booket i dette tidsrommet.');
    }

    setBookings(currentBookings => {
      const nextId =
        Math.max(0, ...currentBookings.map(item => item.Id)) + 1;

      return [...currentBookings, { ...booking, Id: nextId }];
    });
  };

  // Oppdaterer en eksisterende booking med samme ID.
  const updateBooking = (updatedBooking) => {

    // Hindrer overlappende bookinger ved redigering.
    if (hasOverlappingBooking(updatedBooking)) {
      throw new Error('Rommet er allerede booket i dette tidsrommet.');
    }

    setBookings(currentBookings =>
      currentBookings.map(booking =>
        Number(booking.Id) === Number(updatedBooking.Id)
          ? updatedBooking
          : booking
      )
    );

  };
  // Fjerner valgt booking fra den midlertidige bookinglisten.
  const deleteBooking = (bookingId) => {
    setBookings(currentBookings =>
      currentBookings.filter(
        booking => Number(booking.Id) !== Number(bookingId)
      )
    );
  };

  // Oppdaterer valgt lokasjon og hindrer dupliserte navn.
  const updateLocation = (updatedLocation) => {
    const nameExists = locations.some(location =>
      location.LocationId !== updatedLocation.LocationId &&
      location.Name.toLowerCase() === updatedLocation.Name.toLowerCase()
    );

    if (nameExists) {
      throw new Error('En lokasjon med dette navnet finnes allerede.');
    }

    setLocations(currentLocations =>
      currentLocations.map(location =>
        location.LocationId === updatedLocation.LocationId
          ? updatedLocation
          : location
      )
    );
  };

  // Hindrer sletting av lokasjoner som fortsatt har rom.
  const deleteLocation = (locationId) => {
    const hasRooms = rooms.some(room => room.LocationId === locationId);

    if (hasRooms) {
      throw new Error('Lokasjonen kan ikke slettes fordi den har rom.');
    }

    setLocations(currentLocations =>
      currentLocations.filter(location =>
        location.LocationId !== locationId
      )
    );
  };

  return (

    <>
      <Container>
        <Router> {/*Router styrer navigasjon mellom sider i appen*/}
          <NavMenu /> {/*Viser navigasjonsmeny*/}
          <Routes>
            <Route path="/" element={<HomePage />} /> {/*Viser HomePage*/}
            {/*Viser Rooms*/}<Route
              path="/rooms"
              element={
                <RoomListPage
                  rooms={rooms}
                  bookings={bookings}
                  onRoomDeleted={deleteRoom}
                />
              }
            />
            <Route
              path="/locationupdate/:locationId"
              element={
                <LocationUpdatePage
                  locations={locations}
                  onLocationUpdated={updateLocation}
                />
              }
            />

            <Route
              path="/locations"
              element={
                <LocationListPage
                  locations={locations}
                  onLocationDeleted={deleteLocation}
                />
              }
            />

            <Route
              path="/locationcreate"
              element={
                <LocationCreatePage onLocationCreated={createLocation} />
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

            <Route
              path="/bookings"
              element={
                <BookingListPage
                 bookings={bookings} onBookingDeleted={deleteBooking}/>
              }
            />

            <Route
              path="/bookingcreate"
              element={
                <BookingCreatePage
                  rooms={rooms}
                  onBookingCreated={createBooking}
                />
              }
            />

            <Route
              path="/bookingupdate/:bookingId"
              element={
                <BookingUpdatePage
                  bookings={bookings}
                  rooms={rooms}
                  onBookingUpdated={updateBooking}
                />
              }
            />

            <Route path="*" element={<Navigate to="/" replace />} /> {/*Hvis ingen ruter matcher, naviger til HomePage*/}
          </Routes>
        </Router>
      </Container>
    </>
  )
}

export default App