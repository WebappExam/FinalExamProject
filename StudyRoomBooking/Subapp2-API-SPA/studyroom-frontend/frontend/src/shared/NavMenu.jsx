
import { Nav, Navbar } from 'react-bootstrap';
import { Link, NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthContext';
import osloMetLogo from '../assets/Logo_of_Oslo_Metropolitan_University.svg';

const NavMenu = () => {
  const {
    isAuthenticated,
    //email,
    logout
  } = useAuth();

  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <Navbar expand="md" className="site-navbar" data-bs-theme="dark">
      <Navbar.Brand as={Link} to="/" className ="d-flex align-items-center gap-2 me-md-4">
      <img src={osloMetLogo} alt="OsloMet" height="36" className="me-3" />
        Study Room Booking
      </Navbar.Brand>

      <Navbar.Toggle aria-controls="basic-navbar-nav" />

      <Navbar.Collapse id="basic-navbar-nav">
        <Nav className="me-auto gap-md-3">
          <Nav.Link as={NavLink} to="/">
            Hjem
          </Nav.Link>

          <Nav.Link as={NavLink} to="/rooms">
            Rom
          </Nav.Link>

          <Nav.Link as={NavLink} to="/locations">
            Lokasjoner
          </Nav.Link>

          {isAuthenticated && (
            <>
              <Nav.Link as={NavLink} to="/bookings">
                Mine bookinger
              </Nav.Link>

              <Nav.Link as={NavLink} to="/bookingcreate">
                Opprett booking
              </Nav.Link>
            </>
          )}
        </Nav>

        <Nav>
          {isAuthenticated ? (
            <>
              {/*<Navbar.Text className="me-3">
                {email}
              </Navbar.Text>
                */}
              <button
                className="btn btn-outline-light"
                onClick={handleLogout}
              >
                Logg ut
              </button>
            </>
          ) : (
            <>
              <Nav.Link as={NavLink} to="/login">
                Logg inn
              </Nav.Link>

              <Nav.Link as={NavLink} to="/register">
                Registrer
              </Nav.Link>
            </>
          )}
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default NavMenu;
