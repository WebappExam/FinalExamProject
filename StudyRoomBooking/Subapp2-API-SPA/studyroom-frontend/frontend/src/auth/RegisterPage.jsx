
import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from './AuthContext';

function RegisterPage() {
  const { register } = useAuth();
  const navigate = useNavigate();

  const [form, setForm] = useState({
    email: '',
    password: '',
    firstName: '',
    lastName: '',
    studyProgram: ''
  });

  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChange = (event) => {
    setForm({
      ...form,
      [event.target.name]: event.target.value
    });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError('');
    setLoading(true);

    try {
      await register(form);
      navigate('/login');
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mt-5">
      <div className="card mx-auto p-4"
           style={{ maxWidth: '500px' }}>

        <h2 className="mb-4">Create Account</h2>

        {error && (
          <div className="alert alert-danger">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit}>
          {[
            ['firstName', 'First Name'],
            ['lastName', 'Last Name'],
            ['email', 'Email'],
            ['studyProgram', 'Study Program'],
            ['password', 'Password']
          ].map(([name, label]) => (
            <div className="mb-3" key={name}>
              <label className="form-label">
                {label}
              </label>

              <input
                className="form-control"
                name={name}
                type={
                  name === 'password'
                    ? 'password'
                    : name === 'email'
                      ? 'email'
                      : 'text'
                }
                value={form[name]}
                onChange={handleChange}
                required={
                  name !== 'studyProgram'
                }
                minLength={
                  name === 'password' ? 8 : undefined
                }
              />
            </div>
          ))}

          <button
            className="btn btn-primary w-100"
            disabled={loading}
            type="submit"
          >
            {loading ? 'Registering...' : 'Register'}
          </button>
        </form>

        <p className="mt-3">
          Already registered?{' '}
          <Link to="/login">Login</Link>
        </p>
      </div>
    </div>
  );
}

export default RegisterPage;
