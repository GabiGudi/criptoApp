const URL_BASE = 'http://localhost:5003'

export default {
    async obtenerTransacciones() {
        const respuesta = await fetch(`${URL_BASE}/transacciones`)
        return respuesta.json()
    },

    async crearTransaccion(datos) {
        const respuesta = await fetch(`${URL_BASE}/transacciones`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(datos)
        })
        return respuesta.json()
    }
}