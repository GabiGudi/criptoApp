<script setup>
import { ref } from 'vue'
import api from '@/api.js'

let transacciones = ref([])
let cargando = ref(true)

async function cargarTransacciones() {
  cargando.value = true
  transacciones.value = await api.obtenerTransacciones()
  cargando.value = false
}

cargarTransacciones()
</script>

<template>
  <h1>Historial de movimientos</h1>

  <p v-if="cargando">Cargando...</p>

  <p v-else-if="transacciones.length === 0">No hay transacciones registradas.</p>

  <table v-else>
    <thead>
      <tr>
        <th>Fecha</th>
        <th>Criptomoneda</th>
        <th>Tipo</th>
        <th>Cantidad</th>
        <th>Total (ARS)</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="transaccion in transacciones" :key="transaccion.id">
        <td>{{ new Date(transaccion.datetime).toLocaleString('es-AR') }}</td>
        <td>{{ transaccion.crypto_code.toUpperCase() }}</td>
        <td>{{ transaccion.action === 'purchase' ? 'Compra' : 'Venta' }}</td>
        <td>{{ transaccion.crypto_amount }}</td>
        <td>$ {{ transaccion.money.toLocaleString('es-AR') }}</td>
      </tr>
    </tbody>
  </table>
</template>

<style scoped>
h1 {
  color: #1e3a5f;
  margin-bottom: 1.5rem;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th {
  background-color: #1e3a5f;
  color: white;
  padding: 0.75rem 1rem;
  text-align: left;
}

td {
  padding: 0.75rem 1rem;
  border-bottom: 1px solid #e2e8f0;
}

tr:hover {
  background-color: #f1f5f9;
}
</style>