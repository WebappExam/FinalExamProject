import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import RoomForm from './RoomForm';

// Mottar lokasjoner og lagringsfunksjonen fra foreldresiden.
// Vi kobler inn API-lagring når backend er klar.
const RoomCreatePage = ({ locations, onRoomCreated }) => {
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState(false);

  // Sender rommet videre og åpner romoversikten etter vellykket lagring.
  const handleCreate = async (room) => {
    setSubmitting(true);

    try {
      await onRoomCreated(room);
      navigate('/rooms');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="text-start">
      <h1>Opprett nytt rom</h1>

      <RoomForm
        locations={locations}
        onRoomChanged={handleCreate}
        onCancel={() => navigate('/rooms')}
        submitting={submitting}
      />
    </div>
  );
};

export default RoomCreatePage;