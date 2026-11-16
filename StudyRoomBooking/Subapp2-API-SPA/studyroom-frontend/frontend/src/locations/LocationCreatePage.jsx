import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import LocationForm from './LocationForm';

// Bruker det felles skjemaet til å opprette en lokasjon.
  const LocationCreatePage = ({ onLocationCreated }) => {
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState(false);

  // Sender lokasjonen videre og åpner oversikten etter lagring.
  const handleCreate = async (location) => {
    setSubmitting(true);

    try {
      await onLocationCreated(location);
      navigate('/locations');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="text-start">
      <h1>Opprett ny lokasjon</h1>

      <LocationForm
        onLocationChanged={handleCreate}
        onCancel={() => navigate('/locations')}
        submitting={submitting}
      />
    </div>
  );
};

export default LocationCreatePage;