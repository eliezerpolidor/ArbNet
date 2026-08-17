import { useState, useEffect } from 'react';
import { BiHomeAlt, BiLogOut } from 'react-icons/bi';
import { Link } from 'react-router-dom';
import * as Sentry from '@sentry/react'; // <-- Integración de Sentry
import './Dashboard.css';

const Dashboard = () => {
  const [sidebarOpen, setSidebarOpen] = useState(true);
  const [userName, setUserName] = useState(() => {
    // Esto se ejecuta una sola vez al montar el componente
    const user = localStorage.getItem('user');
    if (user) {
      const userData = JSON.parse(user);
      return userData.fullName || userData.email || '';
    }
    return '';
  });

  const diasSemana = ['Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb', 'Dom'];
  const [orders, setOrders] = useState([]);
  const [summary, setSummary] = useState({
    totalBuyFiat: 0,
    totalSellFiat: 0,
    totalVolumeCrypto: 0,
    totalCommissionCrypto: 0,
    netProfitFiat: 0,
    profitMarginPercentage: 0,
    completedOrdersCount: 0
  });

  useEffect(() => {
    // Cargar datos de Binance al montar el componente
    const fetchData = async () => {
      try {
        // const response = await fetch('https://localhost:7039/api/BinanceP2P/historial-p2p');
        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/BinanceP2P/historial-p2p`); //add Produccion
        
        if (!response.ok) {
          throw new Error(`HTTP Error en historial-p2p: ${response.status}`);
        }

        const data = await response.json();
        console.log('Orders recibidos:', data); // Agregar esto
        setOrders(data);
        
        // También calcular el summary
        //const summaryResponse = await fetch('https://localhost:7039/api/BinanceP2P/summary');
        const summaryResponse = await fetch(`${import.meta.env.VITE_API_URL}/api/BinanceP2P/summary`); //add Produccion
        
        if (!summaryResponse.ok) {
          throw new Error(`HTTP Error en summary: ${summaryResponse.status}`);
        }

        const summaryData = await summaryResponse.json();
        setSummary(summaryData);
      } catch (error) {
        console.error('Error cargando datos:', error);

        // 🚨 CAPTURA Y ENVÍO AUTOMÁTICO DEL ERROR A SENTRY
        Sentry.captureException(error, {
          tags: {
            seccion: 'Dashboard',
            endpoint: 'BinanceP2P'
          },
          extra: {
            apiUrl: import.meta.env.VITE_API_URL
          }
        });
      }
    };
    
    fetchData();
  }, []);

  return (
    <div className="dashboard-container">
      {/* Barra Superior (Header) */}
      <header className="dashboard-header">
        <div className="header-left">
          <div className="header-logo-container">
            <img src="/images/logoArbNet.png" alt="ArbNet" className="header-logo" />
            <span className="header-logo-text">ArbNet</span>
          </div>
          <button className="menu-hamburger" onClick={() => setSidebarOpen(!sidebarOpen)}>
            ☰
          </button>
        </div>
        <div className="header-center">
          <h1>Dashboard / Inicio</h1>
        </div>
        <div className="header-right">
          <div className="user-profile">
            <span className="user-avatar">👤</span>
            <span className="user-name">{userName}</span>
          </div>
        </div>
      </header>

      <div className="dashboard-body">
        {/* Menú Lateral Izquierdo (Sidebar) */}
        <aside className={`sidebar ${sidebarOpen ? 'open' : 'closed'}`}>
          <ul className="sidebar-menu">
            <li className="active">
              <BiHomeAlt className="menu-icon" />
              <span className="menu-text">Dashboard</span>
            </li>
            {/* Opciones en construcción - con clase disabled */}
            <li className="disabled" title="En construcción">
              <span className="menu-icon">⇄</span>
              <span className="menu-text">Órdenes P2P</span>
              <span className="badge">🔧</span>
            </li>
            
            <li className="disabled" title="En construcción">
              <span className="menu-icon">⚙️</span>
              <span className="menu-text">Configuración</span>
              <span className="badge">🔧</span>
            </li>
            
            <li className="disabled" title="En construcción">
              <span className="menu-icon">👤</span>
              <span className="menu-text">Perfil/Suscripción</span>
              <span className="badge">🔧</span>
            </li>
            
            {/* Salir - va a la página anterior */}
            <li className="logout" onClick={() => window.history.back()}>
              <BiLogOut className="menu-icon" />
              <span className="menu-text">Salir</span>
            </li>
          </ul>
          {/* Pie de página */}
          <div className="sidebar-footer">
            <p>© 2026 ArbNet | V1.0.0</p>
            <p className="copyright-short">©</p>
          </div>
        </aside>

        {/* Área de Contenido Principal */}
        <main className="main-content">
          {/* Tarjetas de Métricas */}
          <div className="metrics-cards">
            <div className="metric-card">
              <span className="metric-icon">💰</span>
              <div className="metric-info">
                <span className="metric-label">Capital Total Activo</span>
                <span className="metric-value">${summary.totalBuyFiat.toFixed(2)}</span>
              </div>
            </div>
            <div className="metric-card">
              <span className="metric-icon">📈</span>
              <div className="metric-info">
                <span className="metric-label">Ganancias Netas (Mes)</span>
                <span className="metric-value positive">+${summary.netProfitFiat.toFixed(2)}</span>
              </div>
            </div>
            <div className="metric-card">
              <span className="metric-icon">📊</span>
              <div className="metric-info">
                <span className="metric-label">Rendimiento (ROI)</span>
                <span className="metric-value positive">+{summary.profitMarginPercentage.toFixed(2)}%</span>
              </div>
            </div>
          </div>

          {/* Gráfica Semanal */}
          <div className="chart-section">
            <h2>Rendimiento Semanal</h2>
            <div className="chart-container">
              <div className="chart-bars">
                {orders.slice(0, 7).map((order, index) => (
                  <div 
                    key={index}
                    className="bar" 
                    style={{height: `${Math.min((Math.abs(order.netProfit) / 50) * 100, 100)}%`}}
                  >
                    <span className="bar-label">
                      {diasSemana[index]}
                    </span>
                  </div>
                ))}
              </div>
            </div>
          </div>

          {/* Tabla de Transacciones Recientes */}
          <div className="transactions-section">
            <h2>Transacciones Recientes</h2>
            <table className="transactions-table">
              <thead>
                <tr>
                  <th>Fecha</th>
                  <th>Tipo</th>
                  <th>Par</th>
                  <th>Cantidad</th>
                  <th>Precio</th>
                  <th>Comisión</th>
                  <th>Método</th>
                  <th>Status</th>
                  <th>Resultado</th>
                </tr>
              </thead>
              <tbody>
                {orders.slice(0, 10).map((order, index) => (
                  <tr key={index}>
                    <td>
                      {new Date(order.createTime).toLocaleDateString('es-VE')} {' '}
                      {new Date(order.createTime).toLocaleTimeString('es-VE', {hour: '2-digit', minute: '2-digit'})}
                    </td>
                    <td>{order.tradeType === 'BUY' ? 'COMPRA' : 'VENTA'}</td>
                    <td>{order.asset}/{order.fiat}</td>
                    <td>{order.amount} {order.asset}</td>
                    <td>{order.fiatSymbol}{order.unitPrice}</td>
                    <td>{order.commission} {order.asset}</td>
                    <td>{order.paymentMethod}</td>
                    <td className={order.status === 'COMPLETED' ? 'positive' : 'negative'}>
                      {order.status === 'COMPLETED' ? 'COMPLETADO' : 'CANCELADO'}
                    </td>
                    <td className={order.netProfit > 0 ? 'positive' : order.netProfit < 0 ? 'negative' : ''}>
                      {order.netProfit >= 0 ? '+' : ''}{order.fiatSymbol}{order.netProfit.toFixed(2)}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </main>
      </div>
    </div>
  );
};

export default Dashboard;