<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '@/api.js'

const route  = useRoute()
const router = useRouter()

let transaccion = ref(null)
let cargando    = ref(true)
let error       = ref('')

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

onMounted(async () => {
    try {
        transaccion.value = await api.obtenerTransaccionPorId(route.params.id)
    } catch (e) {
        error.value = 'No se pudo cargar la transacción.'
    } finally {
        cargando.value = false
    }
})
</script>

<template>
    <div class="pagina">
        <div class="header">
            <button class="btn-volver" @click="router.push('/historial')">
                ← Volver al historial
            </button>
        </div>

        <p class="cargando" v-if="cargando">Cargando...</p>
        <p class="msg-error" v-else-if="error">⚠ {{ error }}</p>

        <div v-else-if="transaccion" class="card">
            <div class="card-header">
                <div class="cripto-info">
                    <div class="cripto-icono"
                        :style="{ background: obtenerCripto(transaccion.crypto_code).fondo }">
                        <span :style="{ color: obtenerCripto(transaccion.crypto_code).color }">
                            {{ obtenerCripto(transaccion.crypto_code).simbolo }}
                        </span>
                    </div>
                    <div>
                        <div class="cripto-nombre">
                            {{ transaccion.crypto_code.toUpperCase() }}
                        </div>
                        <span class="tipo-badge"
                            :class="transaccion.action === 'purchase' ? 'compra' : 'venta'">
                            {{ transaccion.action === 'purchase' ? '▲ Compra' : '▼ Venta' }}
                        </span>
                    </div>
                </div>
                <div class="monto-total">
                    $ {{ transaccion.money.toLocaleString('es-AR', { minimumFractionDigits: 2 }) }}
                    <span class="monto-label">ARS</span>
                </div>
            </div>

            <div class="detalle-grid">
                <div class="detalle-item">
                    <div class="detalle-label">ID</div>
                    <div class="detalle-valor"># {{ transaccion.id }}</div>
                </div>
                <div class="detalle-item">
                    <div class="detalle-label">Cantidad</div>
                    <div class="detalle-valor mono"
                        :class="transaccion.action === 'purchase' ? 'pos' : 'neg'">
                        {{ transaccion.action === 'purchase' ? '+' : '-' }}{{ transaccion.crypto_amount }}
                        {{ transaccion.crypto_code.toUpperCase() }}
                    </div>
                </div>
                <div class="detalle-item">
                    <div class="detalle-label">Fecha</div>
                    <div class="detalle-valor">
                        {{ new Date(transaccion.datetime).toLocaleDateString('es-AR') }}
                    </div>
                </div>
                <div class="detalle-item">
                    <div class="detalle-label">Hora</div>
                    <div class="detalle-valor">
                        {{ new Date(transaccion.datetime).toLocaleTimeString('es-AR', { hour: '2-digit', minute: '2-digit' }) }}
                    </div>
                </div>
            </div>

            <div class="card-acciones">
                <button class="btn-editar"
                    @click="router.push(`/transacciones/${transaccion.id}/editar`)">
                    ✎ Editar transacción
                </button>
            </div>
        </div>
    </div>
</template>

<style scoped>
.pagina { padding-top: 0.5rem; max-width: 560px; }

.header { margin-bottom: 1.5rem; }

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

.cargando { color: #8b949e; padding: 2rem; text-align: center; }
.msg-error {
    color: #f85149;
    background: #2d1117;
    border: 1px solid #f8514933;
    border-radius: 6px;
    padding: 0.75rem;
}

.card {
    background: #161b22;
    border: 1px solid #30363d;
    border-radius: 12px;
    overflow: hidden;
}

.card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1.25rem 1.5rem;
    border-bottom: 1px solid #30363d;
}

.cripto-info { display: flex; align-items: center; gap: 12px; }

.cripto-icono {
    width: 44px;
    height: 44px;
    border-radius: 10px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.3rem;
    font-weight: 600;
}

.cripto-nombre {
    font-size: 1.1rem;
    font-weight: 600;
    color: #e6edf3;
    margin-bottom: 4px;
}

.tipo-badge {
    font-size: 0.78rem;
    font-weight: 500;
    padding: 2px 8px;
    border-radius: 12px;
}
.compra { background: #0d2b1f; color: #3fb950; }
.venta  { background: #2d1117; color: #f85149; }

.monto-total {
    font-size: 1.4rem;
    font-weight: 600;
    color: #e6edf3;
    font-family: 'SF Mono', 'Consolas', monospace;
    text-align: right;
}
.monto-label {
    font-size: 0.8rem;
    color: #8b949e;
    font-weight: 400;
    display: block;
}

.detalle-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 0;
}

.detalle-item {
    padding: 1rem 1.5rem;
    border-bottom: 1px solid #21262d;
    border-right: 1px solid #21262d;
}
.detalle-item:nth-child(2n) { border-right: none; }
.detalle-item:nth-last-child(-n+2) { border-bottom: none; }

.detalle-label {
    font-size: 0.75rem;
    color: #8b949e;
    text-transform: uppercase;
    letter-spacing: 0.05em;
    margin-bottom: 4px;
}

.detalle-valor {
    font-size: 0.95rem;
    color: #e6edf3;
}

.mono { font-family: 'SF Mono', 'Consolas', monospace; }
.pos  { color: #3fb950; }
.neg  { color: #f85149; }

.card-acciones {
    padding: 1rem 1.5rem;
    border-top: 1px solid #30363d;
    display: flex;
    gap: 8px;
}

.btn-editar {
    background: transparent;
    border: 1px solid #30363d;
    color: #8b949e;
    padding: 0.5rem 1rem;
    border-radius: 6px;
    cursor: pointer;
    font-size: 0.85rem;
    transition: all 0.15s;
}
.btn-editar:hover { border-color: #f0b429; color: #f0b429; }
</style>