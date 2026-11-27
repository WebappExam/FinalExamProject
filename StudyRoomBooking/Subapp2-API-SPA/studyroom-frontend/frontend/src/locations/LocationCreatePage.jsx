import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import LocationForm from './LocationForm';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import * as LocationService from './LocationService';

// Bruker det felles skjemaet til å opprette en lokasjon.
const LocationCreatePage = () => {
  const navigate = useNavigate();

  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: LocationService.createLocation,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['locations'] });
    }
  });

  const [submitting, setSubmitting] = useState(false);

  // Sender lokasjonen videre og åpner oversikten etter lagring.
  const handleCreate = async (location) => {
    setSubmitting(true);

    try {
      await createMutation.mutateAsync(location);
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