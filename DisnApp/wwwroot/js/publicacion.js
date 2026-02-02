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

document.addEventListener("click", async (e) => {
  const btn = e.target.closest(".btn-like");
    if (!btn) return;

    const id = btn.dataset.id;
    const form = btn.closest("form.like-form");
    const token = form.querySelector('input[name="__RequestVerificationToken"]').value;

    try {
    const res = await fetch(form.action, {
        method: "POST",
    headers: {
        "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8",
    "RequestVerificationToken": token
      },
    body: new URLSearchParams({id})
    });

    if (!res.ok) throw new Error("Error Like");

    const data = await res.json();

    // contador
    const countEl = document.querySelector(`#like-count-${id}`);
    if (countEl) countEl.textContent = data.likeCount;

    // estado visual
    btn.classList.toggle("liked", data.liked);

  } catch (err) {
        console.error(err);
    alert("No se pudo procesar el like.");
  }
});
