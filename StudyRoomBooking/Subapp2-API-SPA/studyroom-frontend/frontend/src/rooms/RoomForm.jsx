import { useState } from 'react';
import { Form, Button, Alert } from 'react-bootstrap';

// Felles skjema for oppretting og redigering av rom.
// Foreldresiden bestemmer hva som skjer ved lagring og avbryt.
const RoomForm = ({
  onRoomChanged,
  onCancel,
  initialValues = {},
  roomId,
  locations = [],
  error = null,
  submitting = false
}) => {
  const [roomName, setRoomName] = useState(initialValues.RoomName ?? '');
  const [capacity, setCapacity] = useState(
    String(initialValues.Capacity ?? 1)
  );
  const [building, setBuilding] = useState(initialValues.Building ?? '');
  const [roomType, setRoomType] = useState(
    initialValues.RoomType ?? 'Grupperom'
  );
  const [locationId, setLocationId] = useState(
    String(initialValues.LocationId ?? '')
  );
  const [validationError, setValidationError] = useState('');

  const isEditing = roomId !== undefined && roomId !== null;

  // Kontrollerer verdiene og sender rommet til foreldresiden.
  const handleSubmit = async (event) => {
    event.preventDefault();
    setValidationError('');

    if (
      roomName.trim().length < 2 ||
      building.trim().length < 2 ||
      roomType.trim().length < 2
    ) {
      setValidationError(
        'Romnavn, bygg og romtype må inneholde minst to tegn utenom mellomrom.'
      );
      return;
    }

    if (
      !Number.isInteger(Number(capacity)) ||
      Number(capacity) < 1 ||
      Number(capacity) > 1000
    ) {
      setValidationError('Kapasitet må være et helt tall mellom 1 og 1000.');
      return;
    }

    if (!locations.some(location =>
      String(location.LocationId) === locationId
    )) {
      setValidationError('Velg en gyldig lokasjon.');
      return;
    }

    const room = {
      RoomId: roomId,
      RoomName: roomName.trim(),
      Capacity: Number(capacity),
      Building: building.trim(),
      RoomType: roomType.trim(),
      LocationId: Number(locationId)
    };

    try {
      await onRoomChanged(room);
    } catch {
      setValidationError('Rommet kunne ikke lagres. Prøv igjen.');
    }
  };

  return (
    <Form onSubmit={handleSubmit}>
      <Form.Group className="mb-3" controlId="roomName">
        <Form.Label>Romnavn</Form.Label>
        <Form.Control
          type="text"
          value={roomName}
          onChange={e => setRoomName(e.target.value)}
          required
          minLength={2}
          maxLength={50}
        />
      </Form.Group>

      <Form.Group className="mb-3" controlId="roomCapacity">
        <Form.Label>Kapasitet</Form.Label>
        <Form.Control
          type="number"
          value={capacity}
          onChange={e => setCapacity(e.target.value)}
          required
          min={1}
          max={1000}
          step={1}
        />
      </Form.Group>

      <Form.Group className="mb-3" controlId="roomBuilding">
        <Form.Label>Bygg</Form.Label>
        <Form.Control
          type="text"
          value={building}
          onChange={e => setBuilding(e.target.value)}
          required
          minLength={2}
          maxLength={50}
        />
      </Form.Group>

      <Form.Group className="mb-3" controlId="roomType">
        <Form.Label>Romtype</Form.Label>
        <Form.Control
          type="text"
          value={roomType}
          onChange={e => setRoomType(e.target.value)}
          required
          minLength={2}
          maxLength={50}
        />
      </Form.Group>

      <Form.Group className="mb-3" controlId="roomLocation">
        <Form.Label>Lokasjon</Form.Label>
        <Form.Select
          value={locationId}
          onChange={e => setLocationId(e.target.value)}
          required
        >
          <option value="">Velg lokasjon</option>
          {locations.map(location => (
            <option
              key={location.LocationId}
              value={location.LocationId}
            >
              {location.Name}
            </option>
          ))}
        </Form.Select>
      </Form.Group>

      {/* Viser valideringsfeil eller feil fra foreldresiden. */}
      {(validationError || error) && (
        <Alert variant="danger" className="fw-bold">
          {validationError || error}
        </Alert>
      )}

      <Button type="submit" variant="primary" disabled={submitting}>
        {submitting
          ? 'Lagrer …'
          : isEditing
            ? 'Lagre endringer'
            : 'Opprett rom'}
      </Button>

      <Button
        type="button"
        variant="secondary"
        className="ms-2"
        onClick={onCancel}
        disabled={submitting}
      >
        Avbryt
      </Button>
    </Form>
  );
};

export default RoomForm;