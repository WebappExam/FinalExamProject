
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

  return (
    <>
    <Container>
    <NavMenu /> {/*Viser navigasjonsmeny*/}
        <Router> {/*Router styrer navigasjon mellom sider i appen*/ }
            <Routes>
              <Route path="/" element={<HomePage />} /> {/*Viser HomePage*/ }
              <Route path="/rooms" element={<RoomListPage />} /> {/*Viser Rooms*/ }
              <Route path="*" element={<Navigate to="/" replace />} /> {/*Hvis ingen ruter matcher, naviger til HomePage*/ }
            </Routes>
        </Router>
    </Container>
    </>
  )
}

export default App