
import { Nav, Navbar } from 'react-bootstrap';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthContext';

const NavMenu = () => {
  const {
    isAuthenticated,
    email,
    logout
  } = useAuth();

  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <Navbar expand="lg" className="px-3">
      <Navbar.Brand as={Link} to="/">
        Study Room Booking
      </Navbar.Brand>

      <Navbar.Toggle aria-controls="basic-navbar-nav" />

      <Navbar.Collapse id="basic-navbar-nav">
        <Nav className="me-auto">
          <Nav.Link as={Link} to="/">
            Home
          </Nav.Link>

          <Nav.Link as={Link} to="/rooms">
            Rooms
          </Nav.Link>

          <Nav.Link as={Link} to="/locations">
            Locations
          </Nav.Link>

          {isAuthenticated && (
            <>
              <Nav.Link as={Link} to="/bookings">
                My Bookings
              </Nav.Link>

              <Nav.Link as={Link} to="/bookingcreate">
                Create Booking
              </Nav.Link>
            </>
          )}
        </Nav>

        <Nav>
          {isAuthenticated ? (
            <>
              <Navbar.Text className="me-3">
                {email}
              </Navbar.Text>

              <button
                className="btn btn-outline-danger"
                onClick={handleLogout}
              >
                Logout
              </button>
            </>
          ) : (
            <>
              <Nav.Link as={Link} to="/login">
                Login
              </Nav.Link>

              <Nav.Link as={Link} to="/register">
                Register
              </Nav.Link>
            </>
          )}
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default NavMenu;
