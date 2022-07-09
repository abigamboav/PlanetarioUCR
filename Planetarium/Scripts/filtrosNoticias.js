const fuentesDeNoticias = ["fuente-0", "fuente-1", "fuente-2"];
function filtrarPorFuente(fuenteSeleccionada) {
    fuentesDeNoticias.forEach(fuente => {
        const contenedorNoticias = document.getElementById(fuente);
        if (fuente === fuenteSeleccionada.value || fuenteSeleccionada.value === "default") {
            contenedorNoticias.style.display = 'block';
        } else {
            contenedorNoticias.style.display = 'none';
        }
    });
}