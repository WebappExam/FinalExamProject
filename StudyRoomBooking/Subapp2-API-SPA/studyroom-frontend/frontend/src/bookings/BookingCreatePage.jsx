import { useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import BookingForm from './BookingForm';

// Bruker det felles skjemaet til å opprette en booking.
const BookingCreatePage = ({ rooms, onBookingCreated }) => {
  const navigate = useNavigate();

  // Leser valgt rom fra adressen og kontrollerer at det finnes.
  const [searchParams] = useSearchParams();
  const selectedRoom = rooms.find(
    room => Number(room.RoomId) === Number(searchParams.get('roomId'))
  );
  const [submitting, setSubmitting] = useState(false);

  // Sender bookingen videre og åpner oversikten etter lagring.
  const handleCreate = async (booking) => {
    setSubmitting(true);

    try {
      await onBookingCreated(booking);
      navigate('/bookings');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="text-start">
      <h1>Opprett ny booking</h1>

      <BookingForm
        rooms={rooms}
        initialValues={{ RoomId: selectedRoom?.RoomId ?? '' }}
        onBookingChanged={handleCreate}
        onCancel={() => navigate('/bookings')}
        submitting={submitting}
      />
    </div>
  );
};

export default BookingCreatePage;
