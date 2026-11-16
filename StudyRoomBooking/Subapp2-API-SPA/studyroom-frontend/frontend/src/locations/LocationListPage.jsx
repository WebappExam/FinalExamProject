import { Table, Button, Alert } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { useState } from 'react';

// Viser lokasjonene fra den felles listen i App.
const LocationListPage = ({ locations, onLocationDeleted }) => {
  const [error, setError] = useState('');

  // Ber om bekreftelse og viser en melding hvis sletting ikke tillates.
  const handleDelete = async (location) => {
    setError('');

    const confirmed = window.confirm(
      `Er du sikker på at du vil slette lokasjonen «${location.Name}»?`
    );

    if (!confirmed) {
      return;
    }

    try {
      await onLocationDeleted(location.LocationId);
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : 'Lokasjonen kunne ikke slettes. Prøv igjen.'
      );
    }
  };
  return (
    <div className="text-start">
      <h1>Lokasjoner</h1>

      {/* Viser hvorfor lokasjonen ikke kunne slettes. */}
      {error && (
        <Alert variant="danger" className="fw-bold">
          {error}
        </Alert>
      )}

      <Link to="/locationcreate" className="btn btn-primary mb-3">
        Opprett ny lokasjon
      </Link>

      {/* Går tilbake til romoversikten uten å laste appen på nytt. */}
      <Link to="/rooms" className="btn btn-secondary mb-3 ms-2">
        Tilbake til romoversikten
      </Link>

      {locations.length === 0 ? (
        <p>Ingen lokasjoner er opprettet ennå.</p>
      ) : (
        <Table striped bordered hover responsive>
          <thead>
            <tr>
              <th>ID</th>
              <th>Lokasjonsnavn</th>
              <th>Handlinger</th>

            </tr>
          </thead>

          <tbody>
            {locations.map(location => (
              <tr key={location.LocationId}>
                <td>{location.LocationId}</td>
                <td>{location.Name}</td>
                <td>
                  <Link
                    to={`/locationupdate/${location.LocationId}`}
                    className="btn btn-primary btn-sm"
                  >
                    Rediger
                  </Link>

                  <Button
                    type="button"
                    variant="danger"
                    size="sm"
                    className="ms-2"
                    onClick={() => handleDelete(location)}
                  >
                    Slett
                  </Button>

                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      )}
    </div>
  );
};

export default LocationListPage;