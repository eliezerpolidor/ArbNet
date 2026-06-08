import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import Welcome from './pages/auth/Welcome';
import Login from './pages/auth/Login';
import Register from './pages/auth/Register';
import Dashboard from './pages/auth/Dashboard';
import About from './pages/info/About';
import Contact from './pages/info/Contact';
import Home from './pages/app/Home';
import Historial from './pages/app/Historial';
import Sumary from './pages/app/Sumary';

function App() {
  return (
    <Router>
      <Routes>
        {/* Pantalla principal - primera en mostrar */}
        <Route path="/" element={<Welcome />} />
        
        {/* Auth */}
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/dashboard" element={<Dashboard />} />
        
        {/* Info */}
        <Route path="/about" element={<About />} />
        <Route path="/contact" element={<Contact />} />
        
        {/* App interna */}
        <Route path="/home" element={<Home />} />
        <Route path="/historial" element={<Historial />} />
        <Route path="/summary" element={<Sumary />} />
      </Routes>
    </Router>
  );
}

export default App;