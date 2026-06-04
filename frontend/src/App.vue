<script setup>
</script>

<template>
  <nav>
    <div class="nav-logo">
      <svg width="40" height="36" viewBox="0 4 108 92" xmlns="http://www.w3.org/2000/svg">
        <path d="M 62 27 A 32 32 0 1 0 62 73"
          fill="none" stroke="#f0b429" stroke-width="15" stroke-linecap="butt"/>
        <polyline points="53,90 71,10 89,90"
          fill="none" stroke="#f0b429" stroke-width="13" stroke-linejoin="miter"/>
        <line x1="60" y1="59" x2="82" y2="59"
          stroke="#f0b429" stroke-width="13" stroke-linecap="butt"/>
      </svg>
      CriptoApp
    </div>

    <div class="nav-links">
      <RouterLink to="/">Nueva operación</RouterLink>
      <RouterLink to="/historial">Historial</RouterLink>
    </div>
  </nav>

  <main>
    <RouterView />
  </main>
</template>

<!-- Sin scoped: aplica a toda la app -->
<style>
*, *::before, *::after {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

body {
  background-color: #0d1117;
  color: #e6edf3;
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
  min-height: 100vh;
}
</style>

<style scoped>
nav {
  background-color: #161b22;
  border-bottom: 1px solid #30363d;
  padding: 0 2rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 56px;
}

.nav-logo {
  color: #e6edf3;
  font-size: 1.1rem;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 10px;
}

.logo-badge {
  width: 32px;
  height: 32px;
  background: linear-gradient(135deg, #1a56db, #0ea5e9);
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.75rem;
  font-weight: 700;
  color: #fff;
  letter-spacing: 0.03em;
}

.nav-links {
  display: flex;
  gap: 0.5rem;
}

.nav-links a {
  color: #8b949e;
  text-decoration: none;
  font-size: 0.9rem;
  padding: 0.4rem 0.8rem;
  border-radius: 6px;
  transition: all 0.15s;
}

.nav-links a:hover {
  color: #e6edf3;
  background-color: #21262d;
}

.nav-links a.router-link-active {
  color: #58a6ff;
  background-color: #161b22;
  border: 1px solid #30363d;
}

main {
  padding: 2rem;
  max-width: 900px;
  margin: 0 auto;
}
</style>