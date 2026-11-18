import { useState } from 'react';
import { Form, Button } from 'react-bootstrap';
import RoomTable from './RoomTable';
//mottar den felles romlisten fra APP

const RoomListPage = ({ rooms, bookings = [], onRoomDeleted }) => {
  // Lagrer søketeksten og oppdaterer siden når teksten endres.
  const [searchQuery, setSearchQuery] = useState('');
  // Lagrer feilmeldingen dersom rommet ikke kan slettes.
  const [deleteError, setDeleteError] = useState('');

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
    minCapacity: '1',
    startTime: '',
    endTime: '',
    onlyAvailable: false
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

  // Filtrerer på romnavn, bygg, kapasitet og eventuelt ledighet.
  const filteredRooms = rooms.filter(room => {
    const matchesFilters =
      room.RoomName.toLowerCase().includes(
        appliedFilters.searchQuery.trim().toLowerCase()
      ) &&
      room.Building.toLowerCase().includes(
        appliedFilters.building.trim().toLowerCase()
      ) &&
      room.Capacity >= Number(appliedFilters.minCapacity);

    // Vanlig søk viser også rom som har bookinger.
    if (!appliedFilters.onlyAvailable) {
      return matchesFilters;
    }

    // Kontrollerer om rommet er booket i den valgte perioden.
    const hasOverlap = bookings.some(booking =>
      Number(booking.RoomId) === Number(room.RoomId) &&
      new Date(appliedFilters.startTime) < new Date(booking.EndTime) &&
      new Date(appliedFilters.endTime) > new Date(booking.StartTime)
    );

    return matchesFilters && !hasOverlap;
  });

  // Kontrollerer skjemaet og bruker filtrene når brukeren trykker Søk.
  const handleSearch = (event) => {
    event.preventDefault();

    if (!isCapacityValid || timeError) {
      return;
    }

    setAppliedFilters({
      searchQuery,
      building,
      minCapacity,
      startTime,
      endTime,
      onlyAvailable
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

  // Ber om bekreftelse og viser hvorfor sletting eventuelt avvises.
  const handleRoomDeleted = async (roomId) => {
    setDeleteError('');

    const room = rooms.find(item => item.RoomId === roomId);

    if (!room) {
      return;
    }

    const confirmDelete = window.confirm(
      `Er du sikker på at du vil slette rommet «${room.RoomName}»?`
    );

    if (!confirmDelete) {
      return;
    }

    try {
      await onRoomDeleted(roomId);
    } catch (error) {
      setDeleteError(
        error instanceof Error
          ? error.message
          : 'Rommet kunne ikke slettes. Prøv igjen.'
      );
    }
  };

  return (

    <div className="text-start room-page">
      <h1 className="room-page-title">Finn et grupperom</h1>
      <p className="text-muted">
        Søk etter romnavn, bygg og kapasitet, eller finn ledige rom i ønsket tidsrom.
      </p>

{/*---------------------Skjule oprett rom og administrer lokasjoner fra vanlig brukere --------------------------*/}
      {/* Åpner opprettingssiden uten å laste hele appen på nytt. *
      <Link to="/roomcreate" className="btn btn-primary mb-3">
        Opprett nytt rom
      </Link>

      <Link to="/locations" className="btn btn-secondary mb-3 ms-2">
        Administrer lokasjoner
      </Link>
      */}

      {/* Bruker våre egne valideringsmeldinger ved innsending. */}
      <Form className="room-search-panel" onSubmit={handleSearch} noValidate>
        <div className="room-search-fields">

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

        </div>

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

       <p className="text-muted room-search-help">
          Velg en tidsperiode og kryss av «Vis kun ledige rom» for å søke etter ledige rom.
        </p>

        <div className="room-search-actions">
          <Button
            type="submit"
            variant="primary"
            className="room-search-button"
          >
            Søk
          </Button>

          <Button
            type="button"
            variant="outline-dark"
            onClick={resetFilters}
          >
            Nullstill filtre
          </Button>
        </div>

      </Form>
      {/* Viser feilmelding når rommet ikke kan slettes. */}
      {deleteError && (
        <p className="text-danger fw-bold" role="alert">
          {deleteError}
        </p>
      )}

      {/* Viser feilmelding ved tom eller ugyldig kapasitet. */}
      {!isCapacityValid && (
        <p className="text-danger fw-bold">
          Skriv inn et helt tall mellom 1 og 1000.
        </p>
      )}

      {/*Condition rendering med &&: Viser resultat når kapasitet er gyldig og det ikke er tidfeil*/}
      {isCapacityValid && !timeError && (
        <>
          {/*Condition rendering med ternary (? :): Viser enten ingen treff eller romtabell*/}
          {filteredRooms.length === 0 ? (
            <p>Søket ditt gir ingen treff</p>
          ) : (
            <RoomTable
              rooms={filteredRooms}
              onRoomDeleted={handleRoomDeleted}
            />
          )}
        </>
      )}
    </div>
  );
};

export default RoomListPage;