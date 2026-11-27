import { useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import LocationForm from './LocationForm';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import * as LocationService from './LocationService';

const LocationUpdatePage = () => {
  // Henter lokasjons-ID fra nettadressen.
  const { locationId } = useParams();
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState(false);

  const queryClient = useQueryClient();

  // Henter valgt lokasjon fra API-et.
  const {
    data: location,
    isLoading,
    error: fetchError
  } = useQuery({
    queryKey: ['locations', locationId],
    queryFn: () => LocationService.fetchLocationById(locationId),
    select: data => ({
      LocationId: data.locationId,
      Name: data.name
    })
  });

  // Oppdaterer lokasjonen og markerer lagrede data for ny henting.
  const updateMutation = useMutation({
    mutationFn: LocationService.updateLocation,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['locations'] });
    }
  });

  // Sender endringene videre og åpner oversikten etter lagring.
  const handleUpdate = async (updatedLocation) => {
    setSubmitting(true);

    try {
      await updateMutation.mutateAsync(updatedLocation);
      navigate('/locations');
    } finally {
      setSubmitting(false);
    }
  };

  if (isLoading) {
    return <p role="status">Laster lokasjonen …</p>;
  }

  if (fetchError) {
    return (
      <div className="text-start">
        <p role="alert">{fetchError.message}</p>
        <Link to="/locations">Tilbake til lokasjonsoversikten</Link>
      </div>
    );
  }

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
