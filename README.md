# ArbNet
“Versión demo del proyecto ArbNet para el GitHub Finish-Up-A-Thon Challenge.”

## Contenido Del README.
- <p><a href="C0">Introduccion.</a></p>
- <p><a href="#C1">Requerimientos</a> → breve lista del MVP y funcionalidades.</p>
- <p><a href="#C2">Arquitectura</a> → diagrama simple del flujo (API Binance → Backend → DB → Frontend).</p>
- <p><a href="#C3">Pantallas</a> → mockups o capturas del Dashboard.</p>  
- <p><a href="#C4">Tecnologías</a> → frameworks y librerías usadas.</p> 
- <p><a href="#C5">Impacto</a> → cómo resuelve el problema de errores y tiempo perdido. </p> 
- <p><a href="#C6">Instrucciones de despliegue de ArbNet</a> → requisitos, pasos de instalación, variables de entorno.</p>  
- <p><a href="#C7">Demo</a> → capturas o video corto mostrando el flujo.</p>

---
<h2 id="C0">📌 Introducción</h2>

Este diagrama resume la propuesta de **ArbNet**, una aplicación automatizada conectada a **Binance** que optimiza el arbitraje P2P mediante procesos inteligentes y precisos.  

- En el modelo tradicional, cada comerciante P2P debe **registrar manualmente en una hoja de calculo(generalmente Excel) cada transacción realizada**, calcular ganancias o pérdidas con fórmulas y generar gráficas para analizar resultados. Este proceso repetitivo y propenso a errores consume tiempo y energía, afectando la eficiencia del negocio.  

- ArbNet transforma este flujo manual en un sistema **automatizado y confiable**, eliminando la necesidad de realizar la carga de las transaciones manualmente, ofreciendo cálculos en tiempo real, registros automáticos y reportes claros.  


<p align="center">
    <img src="assets/images/problema-solucion-impacto.png" alt="diagrama problema-solucion-impacto" width="600">
</p>
  
Nuestra App **ArbNet** se conecta con Binance, registra automáticamente cada operación y muestra ganancias y pérdidas en tiempo real, eliminando errores y ahorrando tiempo. Evitando con esto que los traders P2P pierden horas en una hoja de calculo y cometan errores que les cuestan dinero.

---
<h2 id="C1">Requerimientos</h2>
Los requerimientos básicos que la App ArbNet prentende cubrir son:
- Conectar con la API de Binance.
- Registrar operaciones de arbitraje.
- Mostrar métricas en un Dashboard.
- Evitar errores manuales en cálculos.

<h3>🧩 Producto Minimos Viable MVP</h3>
El MVP de la App ArbNet incluye las siguientes pantallas y funcionalidades básicas:

<ul>
  <li><strong>Pantalla de entrada</strong>
    <ul>
      <li>Logo + frase de impacto</li>
      <li>Botones: Iniciar sesión y Registrarse</li>
      <li>Opción secundaria: Entrar como invitado</li>
    </ul>
  </li>

  <li><strong>Pantalla de autenticación</strong>
    <ul>
      <li>Campos de usuario/contraseña</li>
      <li>Botón “Entrar”</li>
      <li>Enlace “¿Olvidaste tu contraseña?”</li>
    </ul>
  </li>

  <li><strong>Pantalla de registro</strong>
    <ul>
      <li>Campos básicos: nombre, correo, contraseña</li>
      <li>Botón “Crear cuenta”</li>
    </ul>
  </li>

  <li><strong>Dashboard principal</strong>
    <ul>
      <li>Capital total activo</li>
      <li>Ganancias día/mes</li>
      <li>Estado del mercado P2P</li>
      <li>Ranking de monedas</li>
    </ul>
  </li>
</ul>

---
<h2 id="C2">📌 Arquitectura</h2>
<h2 id="C3">📌 Pantallas</h2>
<h2 id="C4">📌 Tecnologías</h2>
<h2 id="C5">📌 Impacto</h2>
<h2 id="C6">📌 Instrucciones de despliegue de ArbNet</h2>
<h2 id="C7">📌 Demo</h2>