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


async function main() {
    document.getElementById("search").addEventListener("keyup", search);
}

function matchAllTitles(newsList, text) {
    
    var matching = new Boolean(false);
    for (var newsTopics in newsList) {
        for (var news in newsList[newsTopics]) {
            matching = match(newsList[newsTopics][news].Title, text);
            if (text == "")
                show(newsList[newsTopics][news].Title)
            else if (matching)
                show(newsList[newsTopics][news].Title)
            else
                hide(newsList[newsTopics][news].Title)
        }
    }
    return matching;
}

function matchList(list, word) {
    var matching = new Boolean(false);
    for (var element in list) {
        matching = match(list[element], word);
        if (matching)
            return matching;
    }
    return matching;
}

function match(word, substring) {
    return word.toLowerCase().includes(substring.toLowerCase());
}

function hide(elementId) {
    //console.log("Hidding ", elementId);
    document.getElementById(elementId).style.display = "none";
}

function show(elementId) {
    document.getElementById(elementId).style.display = "block";
}

main();