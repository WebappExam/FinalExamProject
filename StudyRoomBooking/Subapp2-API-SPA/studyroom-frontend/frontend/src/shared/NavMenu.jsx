

import { Nav, Navbar } from 'react-bootstrap'; //Henter menykomponent fra react-bootstrap

const NavMenu = () => { //Lager komponent NavMenu 
  return (
    

    <Navbar expand="lg"> {/*Meny expander på større og mindre skjermer*/}
        <Navbar.Brand href="/">Study Room Booking</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" /> {/*Toggle for å vise menyen på mindre skjermer*/}
        <Navbar.Collapse id="basic-navbar-nav"> {/*Styrer knapp og lenker i menyen*/}
        <Nav className="me-auto">
          <Nav.Link href="/">Home</Nav.Link>
            <Nav.Link href="/rooms">Rooms</Nav.Link> {/*Lenke til romoversikt*/}
        </Nav>
      </Navbar.Collapse>
  </Navbar>
  );
};

export default NavMenu;