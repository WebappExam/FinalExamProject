import { Nav, Navbar } from 'react-bootstrap'; // Henter menykomponenter fra react-bootstrap
import { Link, NavLink } from 'react-router-dom';

const NavMenu = () => { // Lager komponenten NavMenu
  return (
<Navbar expand="md" className="site-navbar" data-bs-theme="dark"> {/* Menyen tilpasser seg skjermstørrelsen */}
      <Navbar.Brand as={Link} to="/">Study Room Booking</Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" />

      <Navbar.Collapse id="basic-navbar-nav">
        <Nav className="me-auto">
          <Nav.Link as={NavLink} to="/">Hjem</Nav.Link>
          <Nav.Link as={NavLink} to="/rooms">Rom</Nav.Link>
          <Nav.Link as={NavLink} to="/bookings">Mine bookinger</Nav.Link>
          <Nav.Link as={NavLink} to="/bookingcreate">Opprett booking</Nav.Link>
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default NavMenu;
