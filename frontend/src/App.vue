<script setup>
</script>

<template>
  <nav>
    <RouterLink to="/">Nueva Compra</RouterLink>
    <RouterLink to="/historial">Historial</RouterLink>
  </nav>

  <main>
    <RouterView />
  </main>
</template>

<style scoped>
nav {
  background-color: #1e3a5f;
  padding: 1rem 2rem;
  display: flex;
  gap: 2rem;
}

nav a {
  color: #93c5fd;
  text-decoration: none;
  font-weight: 500;
  font-size: 1rem;
}

nav a:hover {
  color: white;
}

nav a.router-link-active {
  color: white;
  border-bottom: 2px solid #60a5fa;
  padding-bottom: 2px;
}

main {
  padding: 2rem;
}
</style>