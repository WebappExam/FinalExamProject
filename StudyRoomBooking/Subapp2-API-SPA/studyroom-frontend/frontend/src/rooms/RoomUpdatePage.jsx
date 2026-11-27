import { useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import RoomForm from './RoomForm';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import * as LocationService from '../locations/LocationService';
import * as RoomService from '../RoomService';


const RoomUpdatePage = () => {
  // Henter rom-ID fra nettadressen.
  const { roomId } = useParams();
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState(false);

  const queryClient = useQueryClient();

  // Henter lokasjonene fra databasen.
  const {
    data: locations = [],
    isLoading: locationsLoading,
    error: locationsError
  } = useQuery({
    queryKey: ['locations'],
    queryFn: LocationService.fetchLocations
  });

  // Henter rommet som skal redigeres fra API-et.
  const {
    data: room,
    isLoading,
    error: fetchError
  } = useQuery({
    queryKey: ['rooms', 'detail', roomId],
    queryFn: () => RoomService.fetchRoomById(roomId),
    enabled: !!roomId,
    select: data => ({
      RoomId: data.roomId,
      RoomName: data.roomName,
      Capacity: data.capacity,
      Building: data.building,
      RoomType: data.roomType,
      LocationId: data.locationId,
      Available: data.available
    })
  });

  // Oppdaterer rommet og markerer romlisten for ny henting.
  const updateMutation = useMutation({
    mutationFn: RoomService.updateRoom,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['rooms'] });
    }
  });
  const handleUpdate = async (updatedRoom) => {
    setSubmitting(true);

    try {
      await updateMutation.mutateAsync({
        ...updatedRoom,
        Available: room.Available
      });
      navigate('/rooms');
    } finally {
      setSubmitting(false);
    }
  };

  if (isLoading || locationsLoading) {
    return <p>Laster rommet …</p>;
  }

  if (fetchError || locationsError) {
    return <p className="text-danger">Kunne ikke hente rommet.</p>;
  }

  // Håndterer en adresse som peker på et rom som ikke finnes.
  if (!room) {
    return (
      <div className="text-start">
        <p role="alert">Rommet finnes ikke.</p>
        <Link to="/rooms">Tilbake til romoversikten</Link>
      </div>
    );
  }

  return (
    <div className="text-start">
      <h1>Rediger rom</h1>

      <RoomForm
        key={room.RoomId}
        roomId={room.RoomId}
        initialValues={room}
        locations={locations.map(location => ({
          LocationId: location.locationId,
          Name: location.name
        }))}
        onRoomChanged={handleUpdate}
        onCancel={() => navigate('/rooms')}
        submitting={submitting}
      />
    </div>
  );
};

export default RoomUpdatePage;