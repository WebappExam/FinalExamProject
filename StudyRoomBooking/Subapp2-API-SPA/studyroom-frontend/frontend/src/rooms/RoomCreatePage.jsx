import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import RoomForm from './RoomForm';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import * as LocationService from '../locations/LocationService';
import * as RoomService from '../RoomService';

// Mottar lokasjoner og lagringsfunksjonen fra foreldresiden.
// Vi kobler inn API-lagring når backend er klar.
const RoomCreatePage = () => {
  const navigate = useNavigate();
  const queryClient = useQueryClient();


  // Henter lokasjonene fra databasen.
  const { data: locations = [], isLoading, error } = useQuery({
    queryKey: ['locations'],
    queryFn: LocationService.fetchLocations
  });

  // Oppretter rom gjennom API-et og oppdaterer romlisten.
  const createMutation = useMutation({
    mutationFn: RoomService.createRoom,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['rooms'] });
    }
  });
  const [submitting, setSubmitting] = useState(false);

  // Sender rommet videre og åpner romoversikten etter vellykket lagring.
  const handleCreate = async (room) => {
    setSubmitting(true);

    try {
      await createMutation.mutateAsync(room);
      navigate('/rooms');
    } finally {
      setSubmitting(false);
    }
  };

  if (isLoading) return <p>Laster lokasjoner …</p>;

  if (error) return <p className="text-danger">Kunne ikke hente lokasjonene.</p>;

  return (
    <div className="text-start">
      <h1>Opprett nytt rom</h1>

      <RoomForm
        locations={locations.map(location => ({
          LocationId: location.locationId,
          Name: location.name
        }))}
        onRoomChanged={handleCreate}
        onCancel={() => navigate('/rooms')}
        submitting={submitting}
      />
    </div>
  );
};

export default RoomCreatePage;