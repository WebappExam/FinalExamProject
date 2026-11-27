
import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import BookingForm from './BookingForm';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import * as BookingService from './BookingService';
import * as RoomService from '../RoomService';

// Bruker det felles skjemaet til å redigere en booking.
const BookingUpdatePage = () => {
  const navigate = useNavigate();
  const { bookingId } = useParams();
  const [submitting, setSubmitting] = useState(false);

  const queryClient = useQueryClient();

  // Henter alle rom fra databasen.
  const {
    data: rooms = [],
    isLoading: roomsLoading,
    error: roomsError
  } = useQuery({
    queryKey: ['rooms', 'all'],
    queryFn: RoomService.fetchRooms,
    select: data => data.map(room => ({
      RoomId: room.roomId,
      RoomName: room.roomName
    }))
  });

  // Henter bookingen som skal redigeres fra API-et.
  const {
    data: booking,
    isLoading,
    error: fetchError
  } = useQuery({
    queryKey: ['bookings', bookingId],
    queryFn: () => BookingService.fetchBookingById(bookingId),
    enabled: !!bookingId,
    select: data => ({
      Id: data.id,
      RoomId: data.roomId,
      StartTime: data.startTime,
      EndTime: data.endTime,
      Course: data.course,
      Description: data.description
    })
  });

  // Oppdaterer bookingen gjennom API-et.
  const updateMutation = useMutation({
    mutationFn: BookingService.updateBooking,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['bookings'] });
    }
  });

  if (isLoading || roomsLoading) {
    return <p>Laster bookinginformasjon …</p>;
  }

  if (fetchError || roomsError) {
    return <p className="text-danger">Kunne ikke hente bookinginformasjonen.</p>;
  }

  // Viser melding dersom bookingen ikke finnes.
  if (!booking) {
    return <p>Bookingen ble ikke funnet.</p>;
  }

  // Oppdaterer bookingen og går tilbake til oversikten.
  const handleUpdate = async (updatedBooking) => {
    setSubmitting(true);

    try {
      await updateMutation.mutateAsync(updatedBooking);
      navigate('/bookings');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="text-start">
      <h1>Rediger booking</h1>

      <BookingForm
        rooms={rooms}
        initialValues={booking}
        bookingId={booking.Id}
        onBookingChanged={handleUpdate}
        onCancel={() => navigate('/bookings')}
        submitting={submitting}
      />
    </div>
  );
};

export default BookingUpdatePage;
