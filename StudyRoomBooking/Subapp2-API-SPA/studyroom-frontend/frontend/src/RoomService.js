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