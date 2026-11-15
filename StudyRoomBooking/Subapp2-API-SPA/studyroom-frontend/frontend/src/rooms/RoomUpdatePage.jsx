import { useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import RoomForm from './RoomForm';

const RoomUpdatePage = ({ rooms, locations, onRoomUpdated }) => {
  // Henter rom-ID fra nettadressen.
  const { roomId } = useParams();
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState(false);

  // Finner rommet som skal redigeres.
  const room = rooms.find(item => item.RoomId === Number(roomId));

  const handleUpdate = async (updatedRoom) => {
    setSubmitting(true);

    try {
      await onRoomUpdated(updatedRoom);
      navigate('/rooms');
    } finally {
      setSubmitting(false);
    }
  };

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
        locations={locations}
        onRoomChanged={handleUpdate}
        onCancel={() => navigate('/rooms')}
        submitting={submitting}
      />
    </div>
  );
};

export default RoomUpdatePage;