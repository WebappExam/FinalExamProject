
import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import BookingForm from './BookingForm';

// Bruker det felles skjemaet til å redigere en booking.
const BookingUpdatePage = ({ bookings, rooms, onBookingUpdated }) => {
  const navigate = useNavigate();
  const { bookingId } = useParams();
  const [submitting, setSubmitting] = useState(false);

  // Finner bookingen som skal redigeres.
  const booking = bookings.find(
    item => item.Id === Number(bookingId)
  );

  // Viser melding dersom bookingen ikke finnes.
  if (!booking) {
    return <p>Bookingen ble ikke funnet.</p>;
  }

  // Oppdaterer bookingen og går tilbake til oversikten.
  const handleUpdate = async (updatedBooking) => {
    setSubmitting(true);

    try {
      await onBookingUpdated(updatedBooking);
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
