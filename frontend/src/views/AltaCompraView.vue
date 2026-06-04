<script setup>
import { ref } from 'vue'
import api from '@/api.js'

// Datos del formulario
let crypto_code = ref('')
let action = ref('purchase')
let crypto_amount = ref('')

// Estado de la pantalla
let enviando = ref(false)
let error = ref('')
let exito = ref(false)

async function enviarFormulario() {
  // Limpiar mensajes anteriores
  error.value = ''
  exito.value = false

  // Validaciones
  if (!crypto_code.value) {
    error.value = 'Seleccioná una criptomoneda.'
    return
  }
  if (!crypto_amount.value || Number(crypto_amount.value) <= 0) {
    error.value = 'La cantidad debe ser mayor a 0.'
    return
  }
  

  enviando.value = true

  const resultado = await api.crearTransaccion({
    crypto_code: crypto_code.value,
    action: action.value,
    crypto_amount: Number(crypto_amount.value),
    datetime: new Date().toISOString()
  })

  enviando.value = false

  if (resultado.id) {
    // Limpiar formulario
    crypto_code.value = ''
    action.value = 'purchase'
    crypto_amount.value = ''
    datetime.value = ''
    exito.value = true
  } else {
    error.value = 'Hubo un error al guardar la transacción.'
  }
}
</script>

<template>
  <h1>Nueva transacción</h1>

  <form @submit.prevent="enviarFormulario">

    <div class="campo">
      <label>Criptomoneda</label>
      <select v-model="crypto_code">
        <option value="">-- Seleccioná --</option>
        <option value="bitcoin">Bitcoin (BTC)</option>
        <option value="eth">Ethereum (ETH)</option>
        <option value="usdc">USDC</option>
        <option value="sol">Solana (SOL)</option>
        <option value="dai">DAI</option>
      </select>
    </div>

    <div class="campo">
      <label>Tipo de operación</label>
      <select v-model="action">
        <option value="purchase">Compra</option>
        <option value="sale">Venta</option>
      </select>
    </div>

    <div class="campo">
      <label>Cantidad</label>
      <input
        type="number"
        v-model="crypto_amount"
        placeholder="Ej: 0.00070"
        step="any"
        min="0"
      />
    </div>

    <p class="error" v-if="error">{{ error }}</p>
    <p class="exito" v-if="exito">¡Transacción registrada correctamente!</p>

    <button type="submit" :disabled="enviando">
      {{ enviando ? 'Guardando...' : 'Guardar transacción' }}
    </button>

  </form>
</template>

<style scoped>
h1 {
  color: #1e3a5f;
  margin-bottom: 1.5rem;
}

form {
  max-width: 480px;
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

.campo {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

label {
  font-weight: 500;
  color: #1e3a5f;
  font-size: 0.95rem;
}

input, select {
  padding: 0.6rem 0.8rem;
  border: 1px solid #cbd5e1;
  border-radius: 6px;
  font-size: 1rem;
}

input:focus, select:focus {
  outline: none;
  border-color: #2563eb;
}

button {
  background-color: #2563eb;
  color: white;
  border: none;
  padding: 0.75rem;
  border-radius: 6px;
  font-size: 1rem;
  cursor: pointer;
  font-weight: 500;
}

button:hover:not(:disabled) {
  background-color: #1d4ed8;
}

button:disabled {
  background-color: #93c5fd;
  cursor: not-allowed;
}

.error {
  color: #dc2626;
  font-size: 0.9rem;
  margin: 0;
}

.exito {
  color: #16a34a;
  font-size: 0.9rem;
  margin: 0;
}
</style>