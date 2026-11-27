const API_URL =
  import.meta.env.VITE_API_URL || 'https://localhost:7018';

export async function apiRequest(endpoint, options = {}) {
  const token = sessionStorage.getItem('token');

  const response = await fetch(
    `${API_URL}${endpoint}`,
    {
      ...options,
      headers: {
        'Content-Type': 'application/json',
        ...options.headers,
        ...(token
          ? { Authorization: `Bearer ${token}` }
          : {})
      }
    }
  );

  if (response.status === 401) {
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('email');
  }

  if (!response.ok) {
    const error = await response.json().catch(() => ({}));

    throw new Error(
      error.message || `Request failed: ${response.status}`
    );
  }

  if (response.status === 204) {
    return null;
  }

  return response.json();
}
