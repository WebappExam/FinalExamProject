import { useState } from 'react';
import { Form, Button } from 'react-bootstrap';
import RoomTable from './RoomTable';
import { Link } from 'react-router-dom';

//mottar den felles romlisten fra APP

  const RoomListPage = ({rooms, onRoomDeleted}) => {
  // Lagrer søketeksten og oppdaterer siden når teksten endres.
  const [searchQuery, setSearchQuery] = useState('');

  // Lagrer obligatorisk minste kapasitet = 1
  const [minCapacity, setMinCapacity] = useState('1');
  // Lagrer byggnavnet brukeren ønsker å søke etter.
  const [building, setBuilding] = useState('');

  // Lagrer ønsket tidsperiode og om bare ledige rom skal vises.
  const [startTime, setStartTime] = useState('');
  const [endTime, setEndTime] = useState('');
  const [onlyAvailable, setOnlyAvailable] = useState(false);

  // Kapasitet er obligatorisk og må være et helt tall fra 1 til 1000.
  const isCapacityValid =
    minCapacity !== '' &&
    Number.isInteger(Number(minCapacity)) &&
    Number(minCapacity) >= 1 &&
    Number(minCapacity) <= 1000;

  // Lagrer filtrene fra siste søk, slik at resultatet endres ved innsending.
  const [appliedFilters, setAppliedFilters] = useState({
  searchQuery: '',
  building: '',
  minCapacity: '1'
});

  //Kontrollerer tidsperioden før søkeresultatet vises.
  let timeError = '';
  if (onlyAvailable && (!startTime || !endTime)) {
    timeError = 'Velg både start- og sluttidspunkt for å søke etter ledige rom.';
  } else if ((startTime && !endTime) || (!startTime && endTime)) {
    timeError = 'Fyll ut både start- og sluttidspunkt.';
  } else if (
    startTime &&
    endTime &&
    new Date(startTime) >= new Date(endTime)
  ) {
    timeError = 'Sluttidspunkt må være etter starttidspunkt.';
  }

  // Filtrerer rommene med verdiene fra siste søk.
const filteredRooms = rooms.filter(room =>
  room.RoomName.toLowerCase().includes(
    appliedFilters.searchQuery.trim().toLowerCase()
  ) &&
  room.Building.toLowerCase().includes(
    appliedFilters.building.trim().toLowerCase()
  ) &&
  room.Capacity >= Number(appliedFilters.minCapacity)
);

// Kontrollerer skjemaet og bruker filtrene når brukeren trykker Søk.
const handleSearch = (event) => {
  event.preventDefault();

  if (!isCapacityValid || timeError) {
    return;
  }

  setAppliedFilters({
    searchQuery,
    building,
    minCapacity
  });
};

  // Tilbakestiller søkefeltene til startverdiene.
  const resetFilters = () => {
    setSearchQuery('');
    setBuilding('');
    setMinCapacity('1');

    setStartTime('');
    setEndTime('');
    setOnlyAvailable(false);

    // Tilbakestiller også søkeresultatet.
  setAppliedFilters({
  searchQuery: '',
  building: '',
  minCapacity: '1'
});
  };

  // Ber om bekreftelse før rommet slettes.
const handleRoomDeleted = (roomId) => {
  const room = rooms.find(item => item.RoomId === roomId);

  if (!room) {
    return;
  }

  const confirmDelete = window.confirm(
    `Er du sikker på at du vil slette rommet «${room.RoomName}»?`
  );

  if (confirmDelete) {
    onRoomDeleted(roomId);
  }
};


  return (

    <div className="text-start">
      <h1>Rom</h1>
        {/* Åpner opprettingssiden uten å laste hele appen på nytt. */}
<Link to="/roomcreate" className="btn btn-primary mb-3">
  Opprett nytt rom
</Link>


    {/* Bruker våre egne valideringsmeldinger ved innsending. */}
    <Form onSubmit={handleSearch} noValidate>

      <Form.Group className="mb-3" controlId="roomSearch">
        <Form.Label>Søk etter rom</Form.Label>
        <Form.Control
          type="text"
          placeholder="Skriv inn romnavn"
          value={searchQuery}
          onChange={e => setSearchQuery(e.target.value)}
        />
      </Form.Group>

      {/* Filtrer på bygg, tomt felt viser rom fra alle bygg.*/}
      <Form.Group className="mb-3" controlId="buildingSearch">
        <Form.Label>Bygg</Form.Label>
        <Form.Control
          type="text"
          placeholder="Skriv bygg, for eksempel P35"
          value={building}
          onChange={e => setBuilding(e.target.value)}
        />
      </Form.Group>

      {/* Filtrerer på hvor mange personer rommet minst må ha plass til. */}
      <Form.Group className="mb-3" controlId="minCapacity">
        <Form.Label>Minste kapasitet (antall personer)</Form.Label>
        <Form.Control
          type="number"
          min="1"
          max="1000"
          step="1" required
          placeholder="Skriv inn antall personer"
          value={minCapacity}
          onChange={e => setMinCapacity(e.target.value)}
        />
      </Form.Group>

      {/* Velger starten på tidsperioden. */}
      <Form.Group className="mb-3" controlId="startTime">
        <Form.Label>Fra dato og klokkeslett</Form.Label>
        <Form.Control
          type="datetime-local"
          value={startTime}
          onChange={e => setStartTime(e.target.value)}
        />
      </Form.Group>

      {/* Velger slutten på tidsperioden. */}
      <Form.Group className="mb-3" controlId="endTime">
        <Form.Label>Til dato og klokkeslett</Form.Label>
        <Form.Control
          type="datetime-local"
          value={endTime}
          onChange={e => setEndTime(e.target.value)}
        />
      </Form.Group>

      {/* Lagrer om søket skal begrenses til ledige rom. */}
      <Form.Check
        type="checkbox"
        id="onlyAvailable"
        label="Vis kun ledige rom"
        className="mb-3"
        checked={onlyAvailable}
        onChange={e => setOnlyAvailable(e.target.checked)}
      />

      {timeError && (
        <p className="text-danger fw-bold">
          {timeError}
        </p>
      )}

      <p className="text-muted">
        Tidsfilter og ledighet blir tilgjengelig når API-et er koblet til.
      </p>

        <Button
        type="submit"
        variant="primary"
        className="mb-3 me-2"
        >
        Søk
      </Button>

      <Button
        type="button"
        variant="secondary"
        className="mb-3"
        onClick={resetFilters}
      >
        Nullstill filtre
      </Button>

      </Form>

      {/* Viser feilmelding ved tom eller ugyldig kapasitet. */}
      {!isCapacityValid && (
        <p className="text-danger fw-bold">
          Skriv inn et helt tall mellom 1 og 1000.
        </p>
      )}

      {/* Viser søkeresultatet bare når kapasiteten er gyldig. */}
      {isCapacityValid && !timeError && (
        <>
          {filteredRooms.length === 0 && (
            <p>Søket ditt gir ingen treff</p>
          )}
        <RoomTable
          rooms={filteredRooms}
            onRoomDeleted={handleRoomDeleted}
              />
        </>
      )}
    </div>
  );
};

export default RoomListPage;