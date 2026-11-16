import { useState } from 'react';
import { Form, Button, Alert } from 'react-bootstrap';

// Felles skjema for oppretting og redigering av lokasjoner.
const LocationForm = ({
  onLocationChanged,
  onCancel,
  initialValues = {},
  locationId,
  error = null,
  submitting = false
}) => {
  const [name, setName] = useState(initialValues.Name ?? '');
  const [validationError, setValidationError] = useState('');

  const isEditing = locationId !== undefined && locationId !== null;

  // Kontrollerer navnet før lokasjonen sendes til foreldresiden.
  const handleSubmit = async (event) => {
    event.preventDefault();
    setValidationError('');

    const trimmedName = name.trim();

    if (trimmedName.length < 2 || trimmedName.length > 50) {
      setValidationError('Lokasjonsnavn må ha mellom 2 og 50 tegn.');
      return;
    }

    const location = {
      LocationId: locationId,
      Name: trimmedName
    };

    try {
      await onLocationChanged(location);
    } catch (err) {
      setValidationError(
        err instanceof Error
          ? err.message
          : 'Lokasjonen kunne ikke lagres. Prøv igjen.'
      );
    }
  };

  return (
    <Form onSubmit={handleSubmit}>
      <Form.Group className="mb-3" controlId="locationName">
        <Form.Label>Lokasjonsnavn</Form.Label>
        <Form.Control
          type="text"
          placeholder="Skriv lokasjonsnavn"
          value={name}
          onChange={e => setName(e.target.value)}
          required
          minLength={2}
          maxLength={50}
          onInvalid={e => {
            if (e.target.validity.valueMissing) {
              e.target.setCustomValidity('Vennligst skriv inn lokasjonsnavn.');
            } else {
              e.target.setCustomValidity('Lokasjonsnavn må ha mellom 2 og 50 tegn.');
            }
          }}
          onInput={e => e.target.setCustomValidity('')}
        />
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
            : 'Opprett lokasjon'}
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

export default LocationForm;