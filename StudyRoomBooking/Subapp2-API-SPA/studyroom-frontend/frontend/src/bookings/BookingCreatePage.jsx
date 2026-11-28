import { useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import BookingForm from './BookingForm';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import * as BookingService from './BookingService';
import * as RoomService from '../RoomService';


// Bruker det felles skjemaet til å opprette en booking.
const BookingCreatePage = () => {
  const navigate = useNavigate();

  const queryClient = useQueryClient();

  // Henter alle rom fra databasen.
  const { data: rooms = [], isLoading, error } = useQuery({
    queryKey: ['rooms', 'all'],
    queryFn: RoomService.fetchRooms,
    select: data => data.map(room => ({
      RoomId: room.roomId,
      RoomName: room.roomName
    }))
  });

  // Oppretter booking gjennom API-et og oppdaterer bookinglisten.
  const createMutation = useMutation({
    mutationFn: BookingService.createBooking,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['bookings'] });
    }
  });

  // Leser valgt rom fra adressen og kontrollerer at det finnes.
  const [searchParams] = useSearchParams();
  const selectedRoom = rooms.find( //søker først
    room => Number(room.RoomId) === Number(searchParams.get('roomId'))
  );
  const [submitting, setSubmitting] = useState(false); //Hold oversikt over pågående lagring

  // Sender bookingen videre og åpner oversikten etter lagring.
  const handleCreate = async (booking) => {
    setSubmitting(true); //Det lagres

    try {
      await createMutation.mutateAsync(booking); //Send til API
      navigate('/bookings');
    } finally { //Kjører uavhengig fail/pass
      setSubmitting(false); //nullstill
    }
  };

  if (isLoading) return <p>Laster rom …</p>;

  if (error) return <p className="text-danger">Kunne ikke hente rommene.</p>;

  return (
    <div className="text-start">
      <h1>Opprett ny booking</h1>

      <BookingForm
        rooms={rooms}
        initialValues={{ RoomId: selectedRoom?.RoomId ?? '' }} //Hvis ikke ID finnes, bruk tom verdi
        onBookingChanged={handleCreate}
        onCancel={() => navigate('/bookings')}
        submitting={submitting} //Sender til BookingForm via props
      />
    </div>
  );
};

export default BookingCreatePage;
