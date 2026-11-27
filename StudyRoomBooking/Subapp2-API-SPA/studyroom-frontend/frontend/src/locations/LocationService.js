const API_URL = import.meta.env.VITE_API_URL;

// Kontrollerer svaret før JSON-dataene returneres.
async function handleResponse(response) {
  if (!response.ok) {
    throw new Error(`Forespørselen feilet (status ${response.status}).`);
  }

  return response.json();
}

// Henter alle lokasjoner fra API-et.
export async function fetchLocations() {
  const response = await fetch(`${API_URL}/api/locations`);
  return handleResponse(response);
}

// Oppretter en lokasjon i databasen gjennom API-et.
export async function createLocation(location) {
  const response = await fetch(`${API_URL}/api/locations`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      name: location.Name
    })
  });

  if (!response.ok) {
    const errorData = await response.json().catch(() => null);

    throw new Error(
      errorData?.message ??
      `Lokasjonen kunne ikke opprettes (status ${response.status}).`
    );
  }

  return response.json();
}

// Henter én lokasjon for redigering.
export async function fetchLocationById(locationId) {
  const response = await fetch(`${API_URL}/api/locations/${locationId}`);
  return handleResponse(response);
}

// Lagrer endringer i en eksisterende lokasjon.
export async function updateLocation(location) {
  const response = await fetch(
    `${API_URL}/api/locations/${location.LocationId}`,
    {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        name: location.Name
      })
    }
  );

  if (!response.ok) {
    const errorData = await response.json().catch(() => null);

    throw new Error(
      errorData?.message ??
      `Lokasjonen kunne ikke oppdateres (status ${response.status}).`
    );
  }
}

// Sletter en lokasjon dersom serveren tillater det.
export async function deleteLocation(locationId) {
  const response = await fetch(
    `${API_URL}/api/locations/${locationId}`,
    {
      method: 'DELETE'
    }
  );

  if (!response.ok) {
    const errorData = await response.json().catch(() => null);

    throw new Error(
      errorData?.message ??
      `Lokasjonen kunne ikke slettes (status ${response.status}).`
    );
  }
}