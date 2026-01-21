const modalEl = document.getElementById('PublicacionModal');

modalEl.addEventListener('show.bs.modal', async (event) => {
    const btn = event.relatedTarget;
    const id = btn.getAttribute('data-post-id'); // <-- ahora sí existe

    const content = document.getElementById('PublicacionModalContent');
    content.innerHTML = `<div class="p-4 text-center">Cargando...</div>`;

    const res = await fetch(`/Publicacion/Details?id=${id}`);
    content.innerHTML = await res.text();
});

document.addEventListener("submit", async function (e) {
    const form = e.target;

    // Solo interceptamos el form de comentar (poné esa class en tu form)
    if (!form.classList.contains("js-comentario-form")) return;

    e.preventDefault(); // evita navegar a /Publicacion/Comentar

    const url = form.action;
    const formData = new FormData(form);

    const resp = await fetch(url, {
        method: "POST",
        body: formData,
        headers: { "X-Requested-With": "XMLHttpRequest" }
    });

    if (!resp.ok) {
        console.error("Error comentando:", await resp.text());
        return;
    }

    const html = await resp.text();

    // ✅ Esto es lo importante:
    // Como el server devuelve SOLO la lista de comentarios (_CommentsList),
    // reemplazamos el contenido del contenedor commentsList dentro del modal.
    const commentsList = document.getElementById("commentsList");
    if (commentsList) commentsList.innerHTML = html;

    // Limpia input
    const input = form.querySelector('[name="contenido"]');
    if (input) input.value = "";
});
