
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import BookingForm from './BookingForm';

// Bruker det felles skjemaet til å opprette en booking.
const BookingCreatePage = ({ rooms, onBookingCreated }) => {
  const navigate = useNavigate();
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
        onBookingChanged={handleCreate}
        onCancel={() => navigate('/bookings')}
        submitting={submitting}
      />
    </div>
  );
};

export default BookingCreatePage;
