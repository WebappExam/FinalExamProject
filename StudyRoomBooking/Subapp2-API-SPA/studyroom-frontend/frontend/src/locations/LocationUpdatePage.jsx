import { useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import LocationForm from './LocationForm';

const LocationUpdatePage = ({ locations, onLocationUpdated }) => {
  // Henter lokasjons-ID fra nettadressen.
  const { locationId } = useParams();
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState(false);

  const location = locations.find(
    item => item.LocationId === Number(locationId)
  );

  // Sender endringene videre og åpner oversikten etter lagring.
  const handleUpdate = async (updatedLocation) => {
    setSubmitting(true);

    try {
      await onLocationUpdated(updatedLocation);
      navigate('/locations');
    } finally {
      setSubmitting(false);
    }
  };

  if (!location) {
    return (
      <div className="text-start">
        <p role="alert">Lokasjonen finnes ikke.</p>
        <Link to="/locations">Tilbake til lokasjonsoversikten</Link>
      </div>
    );
  }

  return (
    <div className="text-start">
      <h1>Rediger lokasjon</h1>

      <LocationForm
        key={location.LocationId}
        locationId={location.LocationId}
        initialValues={location}
        onLocationChanged={handleUpdate}
        onCancel={() => navigate('/locations')}
        submitting={submitting}
      />
    </div>
  );
};

export default LocationUpdatePage;
