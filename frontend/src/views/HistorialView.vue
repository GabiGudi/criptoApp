<script setup>
import { ref } from 'vue'
import api from '@/api.js'

let transacciones = ref([])
let cargando      = ref(true)

async function cargarTransacciones() {
  cargando.value = true
  transacciones.value = await api.obtenerTransacciones()
  cargando.value = false
}

cargarTransacciones()

// Colores y símbolos por criptomoneda
const infoCripto = {
  bitcoin: { simbolo: '₿', color: '#f7931a', fondo: '#2a1500' },
  eth:     { simbolo: 'Ξ', color: '#627eea', fondo: '#0f1135' },
  usdc:    { simbolo: '$', color: '#2775ca', fondo: '#001428' },
  sol:     { simbolo: '◎', color: '#9945ff', fondo: '#150a28' },
  dai:     { simbolo: '◈', color: '#f5ac37', fondo: '#2a1800' },
}

function obtenerCripto(code) {
  return infoCripto[code] || { simbolo: '?', color: '#8b949e', fondo: '#21262d' }
}
</script>

<template>
  <div class="pagina">
    <div class="header">
      <div>
        <h1>Historial de movimientos</h1>
        <p class="subtitulo" v-if="!cargando">
          {{ transacciones.length }} operaciones registradas
        </p>
      </div>
      <button class="btn-actualizar" @click="cargarTransacciones" type="button">
        ↻ Actualizar
      </button>
    </div>

    <p class="cargando" v-if="cargando">Cargando historial...</p>

    <p class="vacio" v-else-if="transacciones.length === 0">
      No hay operaciones registradas todavía.
    </p>

    <div class="tabla-wrapper" v-else>
      <table>
        <thead>
          <tr>
            <th>Fecha</th>
            <th>Activo</th>
            <th>Tipo</th>
            <th class="derecha">Cantidad</th>
            <th class="derecha">Total ARS</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="t in transacciones" :key="t.id">

            <td class="fecha">
              {{ new Date(t.datetime).toLocaleDateString('es-AR') }}
              <span class="hora">
                {{ new Date(t.datetime).toLocaleTimeString('es-AR', { hour: '2-digit', minute: '2-digit' }) }}
              </span>
            </td>

            <td>
              <div class="cripto-badge"
                :style="{ background: obtenerCripto(t.crypto_code).fondo }">
                <span :style="{ color: obtenerCripto(t.crypto_code).color }">
                  {{ obtenerCripto(t.crypto_code).simbolo }}
                </span>
                <span class="cripto-code"
                  :style="{ color: obtenerCripto(t.crypto_code).color }">
                  {{ t.crypto_code.toUpperCase() }}
                </span>
              </div>
            </td>

            <td>
              <span class="tipo-badge" :class="t.action === 'purchase' ? 'compra' : 'venta'">
                {{ t.action === 'purchase' ? '▲ Compra' : '▼ Venta' }}
              </span>
            </td>

            <td class="derecha mono">
              <span :class="t.action === 'purchase' ? 'pos' : 'neg'">
                {{ t.action === 'purchase' ? '+' : '-' }}{{ t.crypto_amount }}
              </span>
            </td>

            <td class="derecha mono monto">
              $ {{ t.money.toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) }}
            </td>

          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<style scoped>
.pagina {
  padding-top: 0.5rem;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 1.5rem;
}

h1 {
  font-size: 1.4rem;
  font-weight: 600;
  color: #e6edf3;
  margin-bottom: 4px;
}

.subtitulo {
  color: #8b949e;
  font-size: 0.85rem;
}

.btn-actualizar {
  background: #21262d;
  border: 1px solid #30363d;
  color: #8b949e;
  border-radius: 6px;
  padding: 0.4rem 0.8rem;
  font-size: 0.85rem;
  cursor: pointer;
  transition: all 0.15s;
}

.btn-actualizar:hover {
  color: #e6edf3;
  border-color: #8b949e;
}

.cargando, .vacio {
  color: #8b949e;
  text-align: center;
  padding: 3rem;
}

.tabla-wrapper {
  background: #161b22;
  border: 1px solid #30363d;
  border-radius: 12px;
  overflow: hidden;
}

table {
  width: 100%;
  border-collapse: collapse;
}

thead {
  background: #21262d;
}

th {
  padding: 0.75rem 1rem;
  font-size: 0.75rem;
  font-weight: 500;
  color: #8b949e;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  text-align: left;
  border-bottom: 1px solid #30363d;
}

td {
  padding: 0.85rem 1rem;
  border-bottom: 1px solid #21262d;
  font-size: 0.9rem;
  color: #e6edf3;
}

tr:last-child td {
  border-bottom: none;
}

tr:hover td {
  background: #1c2128;
}

/* Fecha */
.fecha {
  color: #e6edf3;
  white-space: nowrap;
}

.hora {
  display: block;
  font-size: 0.75rem;
  color: #8b949e;
}

/* Badge cripto */
.cripto-badge {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 3px 8px;
  border-radius: 6px;
}

.cripto-code {
  font-size: 0.8rem;
  font-weight: 600;
}

/* Badge tipo */
.tipo-badge {
  font-size: 0.78rem;
  font-weight: 500;
  padding: 3px 8px;
  border-radius: 12px;
}

.compra {
  background: #0d2b1f;
  color: #3fb950;
}

.venta {
  background: #2d1117;
  color: #f85149;
}

/* Números */
.mono {
  font-family: 'SF Mono', 'Consolas', monospace;
  font-size: 0.85rem;
}

.derecha {
  text-align: right;
}

.pos { color: #3fb950; }
.neg { color: #f85149; }

.monto {
  color: #e6edf3;
  font-weight: 500;
}
</style>