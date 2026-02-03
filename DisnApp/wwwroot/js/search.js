(() => {
    const input = document.getElementById("userSearchInput");
    const results = document.getElementById("userSearchResults");
    if (!input || !results) return;

    let timer = null;
    let abortController = null;

    const runSearch = async () => {
        const q = input.value.trim();

        if (q.length < 2) {
            results.innerHTML = `<div class="text-muted small">Escribí al menos 2 letras…</div>`;
            return;
        }

        // cancela request anterior si el user sigue tipeando
        if (abortController) abortController.abort();
        abortController = new AbortController();

        try {
            const res = await fetch(`/Usuario/Search?busqueda=${encodeURIComponent(q)}`, {
                headers: { "X-Requested-With": "XMLHttpRequest" },
                signal: abortController.signal
            });

            if (!res.ok) throw new Error("Error buscando usuarios");
            results.innerHTML = await res.text();
        } catch (e) {
            if (e.name === "AbortError") return;
            results.innerHTML = `<div class="text-danger small">Error al buscar</div>`;
        }
    };

    input.addEventListener("input", () => {
        clearTimeout(timer);
        timer = setTimeout(runSearch, 250);
    });
})();
