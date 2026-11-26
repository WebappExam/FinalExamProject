const API_URL = import.meta.env.VITE_API_URL;

// Kontrollerer svaret før JSON-dataene returneres.
async function handleResponse(response) {
    if (!response.ok) {
        throw new Error(`Forespørselen feilet (status ${response.status}).`);
    }

    return response.json();
}

// Henter en lokasjon med rommene som tilhører den.
export async function fetchLocationRooms(locationId) {
    const response = await fetch(
        `${API_URL}/api/locations/${locationId}/rooms`
    );

    return handleResponse(response);
}

// Oppretter et nytt rom i databasen gjennom API-et.
export async function createRoom(room) {
    const response = await fetch(`${API_URL}/api/rooms`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            roomName: room.RoomName,
            capacity: room.Capacity,
            building: room.Building,
            roomType: room.RoomType,
            locationId: room.LocationId
        })
    });

    return handleResponse(response);
}
// Henter ett bestemt rom fra API-et for redigering.
export async function fetchRoomById(roomId) {
    const response = await fetch(`${API_URL}/api/rooms/${roomId}`);
    return handleResponse(response);
}

// Lagrer endringer i et eksisterende rom gjennom API-et.
export async function updateRoom(room) {
  const response = await fetch(`${API_URL}/api/rooms/${room.RoomId}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      roomName: room.RoomName,
      capacity: room.Capacity,
      building: room.Building,
      roomType: room.RoomType,
      locationId: room.LocationId,
      available: room.Available
    })
  });

  if (!response.ok) {
    throw new Error(`Rommet kunne ikke oppdateres (status ${response.status}).`);
  }
}
// Sletter et rom dersom serveren tillater det.
export async function deleteRoom(roomId) {
  const response = await fetch(`${API_URL}/api/rooms/${roomId}`, {
    method: 'DELETE'
  });

  if (!response.ok) {
    const errorData = await response.json().catch(() => null);

    throw new Error(
      errorData?.message ??
      `Rommet kunne ikke slettes (status ${response.status}).`
    );
  }
}