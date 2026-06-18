import { useState, useRef } from 'react';
import { Link } from 'react-router-dom';
import './Welcome.css';


const Welcome = () => {
  // States de los modales
const [showModal, setShowModal] = useState(false);
const [showContactModal, setShowContactModal] = useState(false);
const [showLoginModal, setShowLoginModal] = useState(false);
const [showRegisterModal, setShowRegisterModal] = useState(false);

// States del formulario
const [nombre, setNombre] = useState('');
const [email, setEmail] = useState('');
const [password, setPassword] = useState('');
const [confirmPassword, setConfirmPassword] = useState('');
const [errors, setErrors] = useState({});
const [showPassword, setShowPassword] = useState(false);
const [showConfirmPassword, setShowConfirmPassword] = useState(false);
const [username, setUsername] = useState('');
const [rememberMe, setRememberMe] = useState(false);
const [country, setCountry] = useState('Venezuela');
const [subscriptionType, setSubscriptionType] = useState('Free');

// Estado de usuarios registrados
const [registeredEmails, setRegisteredEmails] = useState([]);

// Referencia
const emailInputRef = useRef(null);

 // === FUNCIONES ===
  
  // Función para limpiar el formulario
  const cleanForm = () => {
    setNombre('');
    setUsername(''); 
    setEmail('');
    setPassword('');
    setConfirmPassword('');
    setShowPassword(false);
    setShowConfirmPassword(false);
    setErrors({});
  };

  // Función para cerrar registro
  const closeRegisterModal = () => {
    setShowRegisterModal(false);
    cleanForm();
  };

  // Función para cerrar login
  const closeLoginModal = () => {
    setShowLoginModal(false);
    cleanForm();
  };

// Función para abrir registro limpio
const openRegisterModal = () => {
  cleanForm();
  setShowRegisterModal(true);
};

// Función para abrir login limpio
const openLoginModal = () => {
  cleanForm();
  setShowLoginModal(true);
};

const validateForm = () => {
  const newErrors = {};
  
  // 1. Nombre vacío
  if (!nombre.trim()) {
    newErrors.nombre = 'El nombre es requerido';
  }
  
  // 2. Correo vacío
  if (!email) {
    newErrors.email = 'El correo es requerido';
  }
  
  // 3. Contraseña vacía
  if (!password) {
    newErrors.password = 'La contraseña es requerida';
  }
  
  // 4. Contraseña mínima 6 caracteres
  if (password && password.length < 6) {
    newErrors.password = 'Mínimo 6 caracteres';
  }
  
  setErrors(newErrors);
  return Object.keys(newErrors).length === 0;
};

/*-----------handleRegister--------------*/
  const handleRegister = async (e) => {
    e.preventDefault();
    
    // Validar campos vacíos
    if (!nombre.trim()) {
      setErrors({...errors, nombre: 'El nombre es requerido'});
      return;
    }
    
    if (!email) {
      setErrors({...errors, email: 'El correo es requerido'});
      return;
    }
    
    if (!password) {
      setErrors({...errors, password: 'La contraseña es requerida'});
      return;
    }

    if (password !== confirmPassword) {
      setErrors({...errors, confirmPassword: 'Las contraseñas no coinciden'});
      return;
    }
    
    // Llamar al backend
    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/api/users/register`,  {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          fullName: nombre,
          email: email,
          password: password,
          country: country  // ← AGREGAR AQUÍ
        })
      });
      
      const data = await response.json();
      
      if (data.success) {
        alert('Cuenta creada con éxito!');
        closeRegisterModal();
      } else {
        setErrors({...errors, email: data.message});
      }
    } catch (error) {
      setErrors({...errors, email: 'Error al conectar con el servidor'});
    }
  };

/*-----------handleLogin----------------*/
const handleLogin = async (e) => {
    e.preventDefault();

    if (!username) {
      setErrors({...errors, password: 'Ingresa tu usuario o correo'});
      return;
    }
    
    if (!password) {
      setErrors({...errors, password: 'Ingresa tu contraseña'});
      return;
    }
    
    // Llamar al backend
    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/api/users/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          email: username,
          password: password
        })
      });
      
      const data = await response.json();
      
      if (data.success) {
        // Guardar usuario en localStorage
        localStorage.setItem('user', JSON.stringify(data));
        closeLoginModal();
        window.location.href = '/dashboard';
      } else {
        setErrors({...errors, password: data.message});
      }
    } catch (error) {
      setErrors({...errors, password: 'Error al conectar con el servidor'});
    }
  };

  return (
    <div className="welcome-container">
      {/* Fondo con partículas bokeh */}
      <div className="bokeh-bg">
        <div className="particle bitcoin"></div>
        <div className="particle ethereum"></div>
        <div className="particle tether"></div>
      </div>

      {/* Navbar */}
      <nav className="navbar">
        <div className="logo">
          <img src="/images/logoArbNet.png" alt="ArbNet" className="logo-img" />
          <span className="logo-text">ArbNet</span>
        </div>
        <div className="nav-links">
          <button onClick={() => setShowModal(true)} className="nav-link">¿Qué es ArbNet?</button>
          <Link to="#" onClick={(e) => { e.preventDefault(); setShowContactModal(true); }}>Contáctanos</Link>
          {/*<button onClick={openLoginModal} className="nav-login">Iniciar Sesión</button>*/}
          <Link to="/dashboard" onClick={(e) => { e.preventDefault(); openLoginModal(); }}className="nav-login">
          Iniciar Sesión</Link>
          <Link to="#" onClick={(e) => { e.preventDefault(); setShowRegisterModal(true); }} className="nav-register">Registrarse</Link>
        </div>
      </nav>

      {/* Resto del contenido... */}
      
      {/* MODAL */}
      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <button className="modal-close" onClick={() => setShowModal(false)}>✕</button>
            
            <div className="modal-header">
              <img src="/images/logoArbNet.png" alt="ArbNet" className="modal-logo" />
              <h2>¿Qué es ArbNet?</h2>
            </div>

            <div className="modal-body">
              <div className="modal-step">
                <div className="step-icon">⚡</div>
                <div className="step-line"></div>
                <div className="step-text">
                  <h3>1. Conexión API</h3>
                  <p>Integración directa con Binance</p>
                </div>
              </div>

              <div className="modal-step">
                <div className="step-icon">🤖</div>
                <div className="step-line"></div>
                <div className="step-text">
                  <h3>2. Automatización</h3>
                  <p>Carga automáticamente tus operaciones P2P de Binance</p>
                </div>
              </div>

              <div className="modal-step">
                <div className="step-icon">📊</div>
                <div className="step-text">
                  <h3>3. Ganancias/Perdidas en Tiempo Real</h3>
                  <p>Métricas financieras instantáneas</p>
                </div>
              </div>
            </div>

            <div className="modal-footer">
              <p>🤖 <strong>Automatización inteligente</strong> para tus inversiones</p>
            </div>
          </div>
        </div>
      )}

      {/* Modal Contáctanos */}
      {showContactModal && (
        <div className="modal-overlay" onClick={() => setShowContactModal(false)}>
          <div className="modal-content contact-modal" onClick={(e) => e.stopPropagation()}>
            <button className="modal-close" onClick={() => setShowContactModal(false)}>✕</button>
            
            <div className="modal-header">
              <img src="/images/logoArbNet.png" alt="ArbNet" className="modal-logo" />
              <h2>Contáctanos</h2>
            </div>

            <div className="modal-body contact-body">
              <p className="contact-text">
                ¿Tienes alguna pregunta o necesitas más información?
              </p>
              
              <div className="contact-option">
                <span className="contact-icon">📧</span>
                <div className="contact-details">
                  <h3>Correo electrónico</h3>
                  <p>arbnet.devdemo@gmail.com</p>
                </div>
              </div>

              <div className="contact-option">
                <span className="contact-icon">💬</span>
                <div className="contact-details">
                  <h3>WhatsApp</h3>
                  <p className="contact-note">(Próximamente)</p>
                </div>
              </div>
            </div>

            <div className="modal-footer">
              <p>✉️ <strong>Escríbenos</strong> y te responderemos pronto</p>
            </div>
          </div>
        </div>
      )}

      /*----------Modal Login-----------*/
      {/* Modal Login */}
      {showLoginModal && (
        <div className="modal-overlay" onClick={closeLoginModal}>
          <div className="modal-content login-modal" onClick={(e) => e.stopPropagation()}>
            <button className="modal-close" onClick={closeLoginModal}>✕</button>
            
            <div className="modal-header">
              <img src="/images/logoArbNet.png" alt="ArbNet" className="modal-logo" />
              <h2>Iniciar Sesión</h2>
            </div>

            <form className="login-form" onSubmit={handleLogin}>
              
              {/* Campo Usuario */}
              <div className="form-group input-with-icon">
                <span className="input-icon">👤</span>
                <input 
                  type="text" 
                  placeholder="Usuario o Correo Electrónico" 
                  className="form-input user-input"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                />
              </div>

              {/* Campo Contraseña */}
              <div className="form-group password-group">
                <span className="input-icon lock">🔒</span>
                <input 
                  type={showPassword ? "text" : "password"}
                  placeholder="Contraseña" 
                  className="form-input password-input"
                  value={password}
                  onChange={(e) => {
                    setPassword(e.target.value);
                    setErrors({...errors, password: ''});
                  }}
                />
                <button 
                  type="button" 
                  className="toggle-password2"
                  onClick={() => setShowPassword(!showPassword)}
                >
                  {showPassword ? '👁️' : '🙈'}
                </button>
              </div>
              {errors.password && <p className="field-error">{errors.password}</p>}

              {/* === ERROR CON BOTÓN === */}
              {errors.password && (
                <div className="error-with-action">
                  <p className="field-error">{errors.password}</p>
                  {!registeredEmails.includes(username) && (
                    <button 
                      type="button" 
                      className="btn-use-other"
                      onClick={() => {
                        closeLoginModal();
                        openRegisterModal();
                      }}
                    >
                      Crear cuenta
                    </button>
                  )}
                </div>
              )}
              {/* ================================ */}    

              {/* Recordar sesión */}
              <label className="checkbox-remember">
                <input 
                  type="checkbox" 
                  checked={rememberMe}
                  onChange={(e) => setRememberMe(e.target.checked)}
                />
                <span className="checkmark">✓</span>
                <span className="remember-text">Recordar sesión</span>
              </label>

              {/* Botón Entrar */}
              <button type="submit" className="btn-login-submit">
                Entrar
              </button>

              {/* Enlaces */}
              <div className="login-links">
                <Link to="#" className="forgot-link">¿Olvidaste tu contraseña?</Link>
                <Link to="#" onClick={(e) => { e.preventDefault(); closeLoginModal(); openRegisterModal(); }} className="register-link">Crear una cuenta nueva</Link>
              </div>
            </form>

            <div className="modal-footer">
              <p>🔒 <strong>Tus datos están seguros</strong></p>
            </div>
          </div>
        </div>
      )}

      /*-----------Modal Registro -------------*/
      {/* Modal Registro */}
      {showRegisterModal && (
        <div className="modal-overlay" onClick={() => setShowRegisterModal(false)}>
          <div className="modal-content register-modal" onClick={(e) => e.stopPropagation()}>
            <button className="modal-close" onClick={closeRegisterModal}>✕</button>
            
            <div className="modal-header">
              <img src="/images/logoArbNet.png" alt="ArbNet" className="modal-logo" />
              <h2>Crear Cuenta</h2>
            </div>

            <form className="register-form" onSubmit={handleRegister}>
              <div className="form-group">
                <input 
                  type="text" 
                  placeholder="Nombre completo" 
                  className="form-input"
                  value={nombre}
                  onChange={(e) => {
                  setNombre(e.target.value);
                  setErrors({...errors, nombre: ''});
                }}
                />
                {errors.nombre && <p className="field-error">{errors.nombre}</p>}
              </div>
              
              <div className="form-group">
                <input 
                  type="email" 
                  placeholder="Correo electrónico" 
                  className="form-input"
                  ref={emailInputRef}
                  value={email}
                  onChange={(e) => {
                  setEmail(e.target.value);
                  setErrors({...errors, email: ''});
                }}
                />
                {errors.email && (
                  <div className="error-with-action">
                    <p className="field-error">{errors.email}</p>
                    <button 
                      type="button" 
                      className="btn-use-other"
                      onClick={() => {
                        setEmail('');
                        setConfirmPassword('');
                        const passwordInput = document.querySelectorAll('.register-form input[type="password"]');
                        if (passwordInput[0]) passwordInput[0].value = '';
                        if (passwordInput[1]) passwordInput[1].value = '';
                        if (emailInputRef.current) {
                          emailInputRef.current.focus();
                        }
                      }}
                    >
                      Usar otro correo
                    </button>
                  </div>
                )}
              </div>

              <div className="form-group">
                <input 
                    type={showPassword ? "text" : "password"}
                    placeholder="Contraseña" 
                    className="form-input"
                    value={password}
                    onChange={(e) => {
                      setPassword(e.target.value);
                      setErrors({...errors, password: ''});
                  }}
                />
                <button 
                  type="button" 
                  className="toggle-password"
                  onClick={() => setShowPassword(!showPassword)}
                >
                  {showPassword ? '👁️' : '🙈'}
                </button>          
              </div>
              {errors.password && <p className="field-error">{errors.password}</p>}  
              <div className="form-group">
                <input 
                    type={showConfirmPassword ? "text" : "password"}
                    placeholder="Confirmar contraseña" 
                    className="form-input"
                    value={confirmPassword}
                    onChange={(e) => {
                      setConfirmPassword(e.target.value);
                      setErrors({...errors, confirmPassword: ''});
                    }}
                  />
                  <button 
                    type="button" 
                    className="toggle-password1"
                    onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                  >
                    {showConfirmPassword ? '👁️' : '🙈'}
                  </button>
                </div>
                {errors.confirmPassword && <p className="field-error">{errors.confirmPassword}</p>}
              {/*=====add new file======== */}
              {/* Campo País */}
              <div className="form-group">
                <input 
                  type="text" 
                  placeholder="País" 
                  className="form-input"
                  value={country}
                  onChange={(e) => setCountry(e.target.value)}
                />
              </div>

              {/* Tipo de Suscripción */}
              <div className="form-group subscription-options">
                <label className="subscription-label">Tipo de Suscripción:</label>
                <div className="radio-group">
                  <label className="radio-option">
                    <input 
                      type="radio" 
                      name="subscription" 
                      value="Free" 
                      checked={subscriptionType === 'Free'}
                      onChange={() => setSubscriptionType('Free')}
                    />
                    <span>Free</span>
                  </label>
                  <label className="radio-option disabled">
                    <input 
                      type="radio" 
                      name="subscription" 
                      value="Pago" 
                      disabled
                    />
                    <span>Pago $5 (Próximamente)</span>
                  </label>
                </div>
              </div> 
              {/*==============theare file new============ */}     
              <button type="submit" className="btn-register-submit">
                Crear Cuenta
              </button>
            </form>

            <p className="login-link">
              ¿Ya tienes cuenta? <Link to="#" onClick={(e) => { e.preventDefault(); setShowRegisterModal(false); setShowLoginModal(true); }}>Inicia sesión</Link>
            </p>

            <div className="modal-footer">
              <p>🔒 <strong>Tus datos están seguros</strong></p>
            </div>
          </div>
          </div>
        )}


            {/* Sección Central */}
            <main className="hero-section">
            <div className="hero-content">
              <h1 className="hero-title">
                Más eficiencia y precisión en el arbitraje P2P
              </h1>
              <p className="hero-subtitle">
                Automatiza tus operaciones y maximiza tu rentabilidad con ArbNet.
              </p>

              {/* Nueva sección de iconos e información */}
              <div className="features-section">
                <div className="feature">
                  <span className="feature-icon">⚡</span>
                  <h3>Automatización</h3>
                  <p>Ejecuta operaciones automáticamente entre exchanges</p>
                </div>
                <div className="feature">
                  <span className="feature-icon">📈</span>
                  <h3>Ganancias</h3>
                  <p>Maximiza tus beneficios con arbitraje inteligente</p>
                </div>
                <div className="feature">
                  <span className="feature-icon">🔒</span>
                  <h3>Seguridad</h3>
                  <p>Solo lectura, tus fondos siempre protegidos</p>
                </div>
              </div>

              {/* Botones de acción */}
              <div className="cta-buttons">
                <Link 
                  to="/dashboard" 
                  onClick={(e) => { e.preventDefault(); openLoginModal(); }}
                  className="btn-login">
                  🔐 Iniciar Sesión
                </Link>
                <Link 
                  to="/dashboard" 
                  onClick={(e) => { e.preventDefault(); openRegisterModal(); }} 
                  className="btn-register">
                  ✏️ Registrarse
                </Link>
              </div>

              <p className="login-link">
                ¿Ya tienes una cuenta? 
                <Link 
                  to="/dashboard" 
                  onClick={(e) => { e.preventDefault(); openRegisterModal(); }} 
                  className="btn-guest"
                >
                  Entrar como invitado
                </Link>
              </p>
            </div>
      </main>
          </div>
        );
      };

export default Welcome;