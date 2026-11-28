import LoginPage from './auth/LoginPage';
import RegisterPage from './auth/RegisterPage';
import ProtectedRoute from './auth/ProtectedRoute';
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
import Footer from './shared/Footer'
import './App.css'

// Oppretter én felles klient for håndtering av API-data.
const queryClient = new QueryClient();

function App() {

  return (
    <QueryClientProvider client={queryClient}>
      <Router>
        <NavMenu />

        <Container>
          <Routes>
            <Route path="/" element={<HomePage />} />

            <Route
              path="/rooms"
              element={<RoomListPage />}
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
              element={<RoomCreatePage />}
            />

            <Route
              path="/roomupdate/:roomId"
              element={<RoomUpdatePage />}
            />
            <Route
              path="/bookings"
              element={<BookingListPage />}
            />

            <Route
              path="/bookingcreate"
              element={<BookingCreatePage />}
            />

            <Route
              path="/bookingupdate/:bookingId"
              element={<BookingUpdatePage />}
            />

            <Route
              path="*"
              element={<Navigate to="/" replace />}
            />
            <Route path="/login" element={<LoginPage />} />

            <Route path="/register" element={<RegisterPage />} />
          </Routes>
        </Container>
        <Footer />
      </Router>
    </QueryClientProvider>
  );
}

export default App;
