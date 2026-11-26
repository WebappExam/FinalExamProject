import { useState } from 'react'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
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


// Oppretter én felles klient for håndtering av API-data.
const queryClient = new QueryClient();

function App() {

  // Midlertidige lokasjoner til testing før API-et er klart.
  const [locations] = useState([
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

    return (
    <QueryClientProvider client={queryClient}>
      <Router>
        <NavMenu />

        <Container>
          <Routes>
            <Route path="/" element={<HomePage />} />

            <Route
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
              element={<LocationUpdatePage />}
            />

            <Route
              path="/locations"
              element={<LocationListPage />}
            />

            <Route
              path="/locationcreate"
              element={<LocationCreatePage />}
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
                  bookings={bookings}
                  rooms={rooms}
                  onBookingDeleted={deleteBooking}
                />
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
            <Route
              path="*"
              element={<Navigate to="/" replace />}
            />
          </Routes>
        </Container>
      </Router>
    </QueryClientProvider>
  );
}

export default App;
