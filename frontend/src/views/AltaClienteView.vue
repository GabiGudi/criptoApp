<script setup>
import { ref } from 'vue'
import api from '@/api.js'

let nombre = ref('')
let email  = ref('')

let enviando = ref(false)
let error    = ref('')
let exito    = ref(false)

async function enviarFormulario() {
  error.value = ''
  exito.value = false

  if (!nombre.value.trim()) {
    error.value = 'El nombre no puede estar vacío.'
    return
  }
  if (!email.value.trim() || !email.value.includes('@')) {
    error.value = 'El email no tiene un formato válido.'
    return
  }

  try {
    enviando.value = true
    const resultado = await api.crearCliente({
      nombre: nombre.value,
      email: email.value
    })

    if (resultado.id) {
      nombre.value = ''
      email.value  = ''
      exito.value  = true
    } else {
      error.value = 'Hubo un error al guardar el cliente.'
    }
  } catch (e) {
    error.value = 'No se pudo conectar con el servidor.'
  } finally {
    enviando.value = false
  }
}
</script>

<template>
  <div class="pagina">
    <h1>Nuevo cliente</h1>
    <p class="subtitulo">Registrá un cliente para asignarle transacciones</p>

    <div class="card">

      <div class="campo">
        <label>Nombre completo</label>
        <input
          type="text"
          v-model="nombre"
          placeholder="Ej: Juan Pérez"
        />
      </div>

      <div class="campo">
        <label>Email</label>
        <input
          type="email"
          v-model="email"
          placeholder="Ej: juan@mail.com"
        />
      </div>

      <p class="msg-error" v-if="error">⚠ {{ error }}</p>

      <div class="msg-exito" v-if="exito">
        <div class="exito-titulo">✓ Cliente registrado correctamente</div>
        <div class="exito-detalle">Podés asignarle transacciones desde Nueva operación</div>
      </div>

      <button
        class="btn-submit"
        :disabled="enviando"
        @click="enviarFormulario"
      >
        {{ enviando ? 'Guardando...' : 'Guardar cliente' }}
      </button>

    </div>
  </div>
</template>

<style scoped>
.pagina { padding-top: 0.5rem; }

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
  max-width: 480px;
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

input {
  background: #21262d;
  border: 1px solid #30363d;
  border-radius: 8px;
  padding: 0.7rem 0.9rem;
  color: #e6edf3;
  font-size: 1rem;
  transition: border-color 0.15s;
}

input:focus {
  outline: none;
  border-color: #58a6ff;
}

input::placeholder { color: #484f58; }

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
}

.btn-submit {
  background: #238636;
  color: #fff;
  border: none;
  border-radius: 8px;
  padding: 0.75rem;
  font-size: 0.95rem;
  font-weight: 600;
  cursor: pointer;
  transition: opacity 0.15s;
}

.btn-submit:hover:not(:disabled) { background: #2ea043; }
.btn-submit:disabled { opacity: 0.5; cursor: not-allowed; }
</style>