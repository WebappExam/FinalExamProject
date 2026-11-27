
import {
  createContext,
  useContext,
  useState
} from 'react';

const AuthContext = createContext(null);

const API_URL =
  import.meta.env.VITE_API_URL || 'https://localhost:7018';

export function AuthProvider({ children }) {
  const [token, setToken] = useState(
    () => sessionStorage.getItem('token')
  );

  const [email, setEmail] = useState(
    () => sessionStorage.getItem('email')
  );

  const login = async (email, password) => {
    const response = await fetch(`${API_URL}/api/auth/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ email, password })
    });

    const data = await response.json();

    if (!response.ok) {
      throw new Error(
        data.message || 'Login failed.'
      );
    }

    sessionStorage.setItem('token', data.token);
    sessionStorage.setItem('email', data.email);

    setToken(data.token);
    setEmail(data.email);

    return data;
  };

  const register = async (userData) => {
    const response = await fetch(
      `${API_URL}/api/auth/register`,
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
      }
    );

    const data = await response.json();

    if (!response.ok) {
      throw new Error(
        data.message ||
        data.errors?.join(', ') ||
        'Registration failed.'
      );
    }

    return data;
  };

  const logout = () => {
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('email');

    setToken(null);
    setEmail(null);
  };

  return (
    <AuthContext.Provider
      value={{
        token,
        email,
        isAuthenticated: Boolean(token),
        login,
        register,
        logout
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}
