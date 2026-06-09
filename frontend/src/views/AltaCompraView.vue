<script setup>
import { ref } from 'vue'
import api from '@/api.js'

const criptos = [
  { code: 'bitcoin', nombre: 'Bitcoin',  simbolo: '₿', color: '#f7931a', fondo: '#2a1500' },
  { code: 'eth',     nombre: 'Ethereum', simbolo: 'Ξ', color: '#627eea', fondo: '#0f1135' },
  { code: 'usdc',    nombre: 'USDC',     simbolo: '$', color: '#2775ca', fondo: '#001428' },
  { code: 'sol',     nombre: 'Solana',   simbolo: '◎', color: '#9945ff', fondo: '#150a28' },
  { code: 'dai',     nombre: 'DAI',      simbolo: '◈', color: '#f5ac37', fondo: '#2a1800' },
]

let crypto_code   = ref('')
let action        = ref('purchase')
let crypto_amount = ref('')
let datetime      = ref('')
let enviando      = ref(false)
let error         = ref('')
let exito         = ref(null)  

async function enviarFormulario() {
  error.value = ''
  exito.value = null

  if (!crypto_code.value) {
    error.value = 'Seleccioná una criptomoneda.'
    return
  }
  if (!crypto_amount.value || Number(crypto_amount.value) <= 0) {
    error.value = 'La cantidad debe ser mayor a 0.'
    return
  }
  if (!datetime.value) {
    error.value = 'Ingresá la fecha y hora.'
    return
  }

  enviando.value = true
  const resultado = await api.crearTransaccion({
    crypto_code:   crypto_code.value,
    action:        action.value,
    crypto_amount: Number(crypto_amount.value),
    datetime: datetime.value,
  })
  enviando.value = false

  if (resultado.id) {
    exito.value = resultado
    crypto_code.value   = ''
    crypto_amount.value = ''
    action.value        = 'purchase'
  } else {
    error.value = 'Hubo un error al guardar. Intentá de nuevo.'
  }
}

function criptoActual() {
  return criptos.find(c => c.code === crypto_code.value)
}
</script>

<template>
  <div class="pagina">
    <h1>Nueva operación</h1>
    <p class="subtitulo">Registrá una compra o venta de criptomonedas</p>

    <div class="card">
      <div class="campo">
        <label>Criptomoneda</label>
        <div class="cripto-grid">
          <button
            v-for="cripto in criptos"
            :key="cripto.code"
            class="cripto-btn"
            :class="{ activo: crypto_code === cripto.code }"
            :style="crypto_code === cripto.code
              ? { borderColor: cripto.color, background: cripto.fondo }
              : {}"
            @click="crypto_code = cripto.code"
            type="button"
          >
            <span class="cripto-simbolo" :style="{ color: cripto.color }">
              {{ cripto.simbolo }}
            </span>
            <span class="cripto-nombre">{{ cripto.nombre }}</span>
          </button>
        </div>
      </div>

      <div class="campo">
        <label>Tipo de operación</label>
        <div class="toggle-group">
          <button
            class="toggle-btn"
            :class="{ 'toggle-compra': action === 'purchase' }"
            @click="action = 'purchase'"
            type="button"
          >
            ▲ Compra
          </button>
          <button
            class="toggle-btn"
            :class="{ 'toggle-venta': action === 'sale' }"
            @click="action = 'sale'"
            type="button"
          >
            ▼ Venta
          </button>
        </div>
      </div>

      <div class="campo">
        <label>Cantidad</label>
        <div class="input-wrapper">
          <span v-if="criptoActual()" class="input-prefix"
            :style="{ color: criptoActual().color }">
            {{ criptoActual().simbolo }}
          </span>
          <input
            type="number"
            v-model="crypto_amount"
            placeholder="0.00000000"
            step="any"
            min="0"
            :class="{ 'con-prefix': criptoActual() }"
          />
        </div>
      </div>

      <p class="msg-error" v-if="error">⚠ {{ error }}</p>

      <div class="msg-exito" v-if="exito">
        <div class="exito-titulo">✓ Operación registrada</div>
        <div class="exito-detalle">
          <span>{{ exito.crypto_code.toUpperCase() }}</span>
          <span>·</span>
          <span>{{ exito.crypto_amount }} unidades</span>
          <span>·</span>
          <span>$ {{ exito.money.toLocaleString('es-AR') }} ARS</span>
        </div>
      </div>

      <div class="campo">
    <label>Fecha y hora</label>
    <input
        type="datetime-local"
        v-model="datetime"
    />
</div>

      <button
        class="btn-submit"
        :class="action === 'purchase' ? 'btn-compra' : 'btn-venta'"
        :disabled="enviando"
        @click="enviarFormulario"
      >
        {{ enviando ? 'Procesando...' : (action === 'purchase' ? '▲ Confirmar compra' : '▼ Confirmar venta') }}
      </button>

    </div>
  </div>
</template>

<style scoped>
.pagina {
  padding-top: 0.5rem;
}

h1 {
  font-size: 1.4rem;
  font-weight: 600;
  color: #e6edf3;
  margin-bottom: 4px;
}

.subtitulo {
  color: #8b949e;
  font-size: 0.9rem;
  margin-bottom: 1.5rem;
}

.card {
  background: #161b22;
  border: 1px solid #30363d;
  border-radius: 12px;
  padding: 1.5rem;
  max-width: 520px;
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

.campo {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

label {
  font-size: 0.8rem;
  color: #8b949e;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

/* Grid de criptos */
.cripto-grid {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 8px;
}

.cripto-btn {
  background: #21262d;
  border: 1px solid #30363d;
  border-radius: 8px;
  padding: 0.6rem 0.3rem;
  cursor: pointer;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 4px;
  transition: all 0.15s;
}

.cripto-btn:hover {
  border-color: #8b949e;
  background: #2d333b;
}

.cripto-simbolo {
  font-size: 1.1rem;
  font-weight: 600;
}

.cripto-nombre {
  font-size: 0.65rem;
  color: #8b949e;
}

.cripto-btn.activo .cripto-nombre {
  color: #e6edf3;
}

/* Toggle compra/venta */
.toggle-group {
  display: flex;
  gap: 8px;
}

.toggle-btn {
  flex: 1;
  background: #21262d;
  border: 1px solid #30363d;
  border-radius: 8px;
  padding: 0.6rem;
  cursor: pointer;
  color: #8b949e;
  font-size: 0.9rem;
  transition: all 0.15s;
}

.toggle-compra {
  background: #0d2b1f;
  border-color: #3fb950;
  color: #3fb950;
  font-weight: 500;
}

.toggle-venta {
  background: #2d1117;
  border-color: #f85149;
  color: #f85149;
  font-weight: 500;
}

/* Input cantidad */
.input-wrapper {
  position: relative;
  display: flex;
  align-items: center;
}

.input-prefix {
  position: absolute;
  left: 12px;
  font-size: 1rem;
  font-weight: 600;
}

input {
  width: 100%;
  background: #21262d;
  border: 1px solid #30363d;
  border-radius: 8px;
  padding: 0.7rem 0.9rem;
  color: #e6edf3;
  font-size: 1rem;
  font-family: 'SF Mono', 'Consolas', monospace;
  transition: border-color 0.15s;
}

input.con-prefix {
  padding-left: 2.2rem;
}

input:focus {
  outline: none;
  border-color: #58a6ff;
}

input::placeholder {
  color: #484f58;
}

/* Mensajes */
.msg-error {
  color: #f85149;
  font-size: 0.85rem;
  background: #2d1117;
  border: 1px solid #f8514933;
  border-radius: 6px;
  padding: 0.5rem 0.75rem;
}

.msg-exito {
  background: #0d2b1f;
  border: 1px solid #3fb95033;
  border-radius: 6px;
  padding: 0.75rem;
}

.exito-titulo {
  color: #3fb950;
  font-weight: 500;
  font-size: 0.9rem;
  margin-bottom: 4px;
}

.exito-detalle {
  color: #8b949e;
  font-size: 0.8rem;
  font-family: monospace;
  display: flex;
  gap: 8px;
}

/* Botón submit */
.btn-submit {
  border: none;
  border-radius: 8px;
  padding: 0.75rem;
  font-size: 0.95rem;
  font-weight: 600;
  cursor: pointer;
  transition: opacity 0.15s;
  letter-spacing: 0.02em;
}

.btn-compra {
  background: #238636;
  color: #fff;
}

.btn-compra:hover:not(:disabled) {
  background: #2ea043;
}

.btn-venta {
  background: #da3633;
  color: #fff;
}

.btn-venta:hover:not(:disabled) {
  background: #f85149;
}

.btn-submit:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>