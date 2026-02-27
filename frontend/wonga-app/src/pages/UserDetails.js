import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './Auth.css';

function UserDetails() {
  const [user, setUser] = useState(null);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUser = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        navigate('/login');
        return;
      }
      try {
        const res = await axios.get('http://localhost:8080/api/Auth/me', {
          headers: { Authorization: `Bearer ${token}` }
        });
        setUser(res.data);
      } catch (err) {
        setError('Failed to load user details');
      }
    };
    fetchUser();
  }, [navigate]);

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    navigate('/login');
  };

  return (
    <div className="auth-container">
      <div className="auth-card user-card">
        <h2>User Details</h2>
        <p className="subtitle">Your account information</p>
        {error && <div className="error">{error}</div>}
        {user ? (
          <div className="user-info">
            <div className="user-avatar">
              {user.firstName.charAt(0)}{user.lastName.charAt(0)}
            </div>
            <div className="info-group">
              <label>First Name</label>
              <p>{user.firstName}</p>
            </div>
            <div className="info-group">
              <label>Last Name</label>
              <p>{user.lastName}</p>
            </div>
            <div className="info-group">
              <label>Email</label>
              <p>{user.email}</p>
            </div>
            <button onClick={handleLogout} className="btn-logout">Logout</button>
          </div>
        ) : (
          <p>Loading...</p>
        )}
      </div>
    </div>
  );
}

export default UserDetails;