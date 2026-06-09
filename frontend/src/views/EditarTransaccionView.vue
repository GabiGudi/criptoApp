<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '@/api.js'

const route  = useRoute()
const router = useRouter()

// Campos editables
let crypto_code   = ref('')
let action        = ref('')
let crypto_amount = ref('')
let money         = ref('')

let cargando  = ref(true)
let enviando  = ref(false)
let error     = ref('')
let exito     = ref(false)

const criptos = [
    { code: 'bitcoin', nombre: 'Bitcoin',  simbolo: '₿', color: '#f7931a', fondo: '#2a1500' },
    { code: 'eth',     nombre: 'Ethereum', simbolo: 'Ξ', color: '#627eea', fondo: '#0f1135' },
    { code: 'usdc',    nombre: 'USDC',     simbolo: '$', color: '#2775ca', fondo: '#001428' },
    { code: 'sol',     nombre: 'Solana',   simbolo: '◎', color: '#9945ff', fondo: '#150a28' },
    { code: 'dai',     nombre: 'DAI',      simbolo: '◈', color: '#f5ac37', fondo: '#2a1800' },
]

onMounted(async () => {
    try {
        // Cargamos la transacción actual para precompletar los campos
        const t = await api.obtenerTransaccionPorId(route.params.id)
        crypto_code.value   = t.crypto_code
        action.value        = t.action
        crypto_amount.value = t.crypto_amount
        money.value         = t.money
    } catch (e) {
        error.value = 'No se pudo cargar la transacción.'
    } finally {
        cargando.value = false
    }
})

async function guardarCambios() {
    error.value = ''
    exito.value = false

    if (Number(crypto_amount.value) <= 0) {
        error.value = 'La cantidad debe ser mayor a 0.'
        return
    }

    try {
        enviando.value = true
        const resultado = await api.actualizarTransaccion(route.params.id, {
            crypto_code:   crypto_code.value,
            action:        action.value,
            crypto_amount: Number(crypto_amount.value),
            money:         Number(money.value)
        })

        if (resultado.id) {
            exito.value = true
        } else {
            error.value = 'Hubo un error al actualizar.'
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
        <div class="header">
            <button class="btn-volver" @click="router.push('/historial')">
                ← Volver al historial
            </button>
        </div>

        <h1>Editar transacción</h1>
        <p class="subtitulo">Modificá los datos de la transacción #{{ route.params.id }}</p>

        <p class="cargando" v-if="cargando">Cargando...</p>

        <div class="card" v-else>

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
                    <button class="toggle-btn"
                        :class="{ 'toggle-compra': action === 'purchase' }"
                        @click="action = 'purchase'" type="button">
                        ▲ Compra
                    </button>
                    <button class="toggle-btn"
                        :class="{ 'toggle-venta': action === 'sale' }"
                        @click="action = 'sale'" type="button">
                        ▼ Venta
                    </button>
                </div>
            </div>

            <div class="campo">
                <label>Cantidad</label>
                <input type="number" v-model="crypto_amount" step="any" min="0" />
            </div>

            <div class="campo">
                <label>Total en ARS</label>
                <input type="number" v-model="money" step="any" min="0" />
                <span class="nota">* Podés ajustar el monto manualmente si es necesario.</span>
            </div>

            <p class="msg-error" v-if="error">⚠ {{ error }}</p>
            <p class="msg-exito" v-if="exito">✓ Transacción actualizada correctamente</p>

            <div class="card-acciones">
                <button class="btn-cancelar" @click="router.push('/historial')">
                    Cancelar
                </button>
                <button class="btn-guardar" :disabled="enviando" @click="guardarCambios">
                    {{ enviando ? 'Guardando...' : 'Guardar cambios' }}
                </button>
            </div>

        </div>
    </div>
</template>

<style scoped>
.pagina { padding-top: 0.5rem; }

.header { margin-bottom: 1rem; }

.btn-volver {
    background: transparent;
    border: 1px solid #30363d;
    color: #8b949e;
    padding: 0.4rem 0.8rem;
    border-radius: 6px;
    cursor: pointer;
    font-size: 0.9rem;
    transition: all 0.15s;
}
.btn-volver:hover { color: #e6edf3; border-color: #8b949e; }

h1 { font-size: 1.4rem; font-weight: 600; color: #e6edf3; margin-bottom: 4px; }
.subtitulo { color: #8b949e; font-size: 0.9rem; margin-bottom: 1.5rem; }
.cargando  { color: #8b949e; padding: 2rem; }

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

.campo { display: flex; flex-direction: column; gap: 0.5rem; }

label {
    font-size: 0.8rem;
    color: #8b949e;
    text-transform: uppercase;
    letter-spacing: 0.05em;
}

.nota { font-size: 0.75rem; color: #8b949e; }

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
.cripto-btn:hover { border-color: #8b949e; background: #2d333b; }
.cripto-simbolo { font-size: 1.1rem; font-weight: 600; }
.cripto-nombre  { font-size: 0.65rem; color: #8b949e; }
.cripto-btn.activo .cripto-nombre { color: #e6edf3; }

.toggle-group { display: flex; gap: 8px; }
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
.toggle-compra { background: #0d2b1f; border-color: #3fb950; color: #3fb950; font-weight: 500; }
.toggle-venta  { background: #2d1117; border-color: #f85149; color: #f85149; font-weight: 500; }

input {
    background: #21262d;
    border: 1px solid #30363d;
    border-radius: 8px;
    padding: 0.7rem 0.9rem;
    color: #e6edf3;
    font-size: 1rem;
    font-family: 'SF Mono', 'Consolas', monospace;
    transition: border-color 0.15s;
}
input:focus { outline: none; border-color: #58a6ff; }

.msg-error {
    color: #f85149;
    background: #2d1117;
    border: 1px solid #f8514933;
    border-radius: 6px;
    padding: 0.5rem 0.75rem;
    font-size: 0.85rem;
}
.msg-exito {
    color: #3fb950;
    background: #0d2b1f;
    border: 1px solid #3fb95033;
    border-radius: 6px;
    padding: 0.5rem 0.75rem;
    font-size: 0.85rem;
}

.card-acciones { display: flex; gap: 8px; justify-content: flex-end; }

.btn-cancelar {
    background: #21262d;
    border: 1px solid #30363d;
    color: #8b949e;
    padding: 0.6rem 1.2rem;
    border-radius: 6px;
    cursor: pointer;
    font-size: 0.9rem;
}
.btn-cancelar:hover { color: #e6edf3; }

.btn-guardar {
    background: #1a56db;
    border: none;
    color: #fff;
    padding: 0.6rem 1.2rem;
    border-radius: 6px;
    cursor: pointer;
    font-size: 0.9rem;
    font-weight: 500;
    transition: background 0.15s;
}
.btn-guardar:hover:not(:disabled) { background: #1e40af; }
.btn-guardar:disabled { opacity: 0.5; cursor: not-allowed; }
</style>