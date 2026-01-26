// DEBUG: loguea todos los fetch
if (!window.__fetchLogged) {
    window.__fetchLogged = true;
    const origFetch = window.fetch;

    window.fetch = function (...args) {
        console.log("FETCH ->", args[0]);
        try { throw new Error("stack"); } catch (e) { console.log(e.stack); }
        return origFetch.apply(this, args);
    };

    console.log("✅ fetch logger activo");
}


console.log("publicacion.js cargado", new Date().toISOString());


const modalEl = document.getElementById('PublicacionModal');

modalEl.addEventListener('show.bs.modal', async (event) => {
    const btn = event.relatedTarget;

    console.log("TRIGGER TAG:", btn?.tagName);
    console.log("TRIGGER OUTER:", btn?.outerHTML);
    console.log("TRIGGER post-id:", btn?.getAttribute('data-post-id'));

    const id = btn?.getAttribute('data-post-id');

    const content = document.getElementById('PublicacionModalContent');
    content.innerHTML = `<div class="p-4 text-center">Cargando...</div>`;

    const res = await fetch(`/Publicacion/Details?id=${encodeURIComponent(id ?? "")}`);
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

const chatModalEl = document.getElementById('chatModal');

chatModalEl.addEventListener('show.bs.modal', async (event) => {
    const trigger = event.relatedTarget;          // el botón clickeado
    const chatId = trigger.getAttribute('data-chat-id');

    const body = document.getElementById('chatModalBody');
    body.innerHTML = `<div class="p-4 text-center">Cargando...</div>`;

    const res = await fetch(`/Mensaje/Chat?id=${encodeURIComponent(chatId)}`, {
        headers: { "X-Requested-With": "XMLHttpRequest" }
    });

    if (!res.ok) {
        body.innerHTML = `<div class="p-4 text-danger">Error cargando el chat.</div>`;
        return;
    }

    const html = await res.text();
    body.innerHTML = html;
});
