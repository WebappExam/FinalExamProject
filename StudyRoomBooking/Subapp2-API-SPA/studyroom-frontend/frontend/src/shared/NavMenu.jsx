import { Nav, Navbar } from 'react-bootstrap'; // Henter menykomponenter fra react-bootstrap

const NavMenu = () => { // Lager komponenten NavMenu
  return (
    <Navbar expand="lg"> {/* Menyen tilpasser seg skjermstørrelsen */}
      <Navbar.Brand href="/">Study Room Booking</Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" />

      <Navbar.Collapse id="basic-navbar-nav">
        <Nav className="me-auto">
          <Nav.Link href="/">Hjem</Nav.Link>
          <Nav.Link href="/rooms">Rom</Nav.Link>
          <Nav.Link href="/bookings"> Mine bookinger</Nav.Link>
          <Nav.Link href="/bookingcreate">Opprett booking</Nav.Link>
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default NavMenu;
