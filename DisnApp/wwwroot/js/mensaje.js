// ==============================
// DisnApp Chat Module (AJAX + Modal + Polling)
// ==============================

(() => {
    // -------- Config --------
    const POLL_MS = 2500;

    // -------- State --------
    let pollTimer = null;
    let activeConvId = null;

    // -------- DOM helpers --------
    const $ = (id) => document.getElementById(id);

    function scrollChatToBottom(force = true) {
        const scroll = $("chatScroll");
        if (!scroll) return;

        // Si no forzamos, sólo bajamos si el usuario está cerca del final
        if (!force && !isNearBottom(scroll)) return;

        // Dejalo en el último frame para que mida bien el scrollHeight
        requestAnimationFrame(() => {
            scroll.scrollTop = scroll.scrollHeight;
        });
    }

    function isNearBottom(el, threshold = 120) {
        return el.scrollHeight - el.scrollTop - el.clientHeight < threshold;
    }

    // -------- Polling control --------
    function stopPolling() {
        if (pollTimer) clearInterval(pollTimer);
        pollTimer = null;
        activeConvId = null;
    }

    function startPolling(chatId) {
        stopPolling();
        activeConvId = chatId;
        pollTimer = setInterval(refreshMessagesOnly, POLL_MS);
    }

    // -------- Bandeja refresh --------
    async function refreshBandeja() {
        const cont = $("bandejaContainer");
        if (!cont) return;

        const res = await fetch(`/Usuario/Conversacion?t=${Date.now()}`, {
            headers: { "X-Requested-With": "XMLHttpRequest" }
        });

        if (!res.ok) {
            console.error("refreshBandeja fallo:", res.status, await res.text());
            return;
        }

        cont.innerHTML = await res.text();
    }

    // -------- Load chat partial into modal --------
    async function loadChatPartial(chatId) {
        const body = $("chatModalBody");
        if (!body) return false;

        body.innerHTML = `<div class="p-4 text-center">Cargando...</div>`;

        const res = await fetch(`/Mensaje/Chat?id=${encodeURIComponent(chatId)}&t=${Date.now()}`, {
            headers: { "X-Requested-With": "XMLHttpRequest" }
        });

        if (!res.ok) {
            body.innerHTML = `<div class="p-4 text-danger">Error cargando el chat.</div>`;
            console.error("Error cargando chat:", res.status, await res.text());
            return false;
        }

        body.innerHTML = await res.text();
        return true;
    }

    // -------- Refresh ONLY messages (keep input, keep modal) --------
    async function refreshMessagesOnly() {
        if (!activeConvId) return;

        const scroll = $("chatScroll");
        const wasNear = scroll ? isNearBottom(scroll) : true;

        const res = await fetch(`/Mensaje/Chat?id=${encodeURIComponent(activeConvId)}&t=${Date.now()}`, {
            headers: { "X-Requested-With": "XMLHttpRequest" }
        });

        if (!res.ok) return;

        const html = await res.text();

        // Parsear HTML y extraer SOLO #chatMessages
        const doc = new DOMParser().parseFromString(html, "text/html");
        const newMessages = doc.getElementById("chatMessages");
        const currentMessages = $("chatMessages");

        if (!newMessages || !currentMessages) return;

        currentMessages.innerHTML = newMessages.innerHTML;

        // Si el usuario estaba cerca del final, bajamos automático.
        scrollChatToBottom(wasNear);
    }

    // -------- Open chat by id (used by "CreateOrGet") --------
    async function openChatById(chatId) {
        // 1) cargar partial
        const ok = await loadChatPartial(chatId);
        if (!ok) return;

        // 2) abrir modal
        const modalEl = $("chatModal");
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();

        // 3) scroll y bandeja
        scrollChatToBottom(true);
        await refreshBandeja();

        // 4) iniciar polling
        startPolling(chatId);
    }

    // ==============================
    // Events
    // ==============================

    // A) Abrir chat EXISTENTE desde bandeja:
    // requiere botón con data-chat-id y bootstrap modal trigger
    const chatModalEl = $("chatModal");
    if (chatModalEl) {
        chatModalEl.addEventListener("show.bs.modal", async (event) => {
            const btn = event.relatedTarget;
            const chatId = btn?.getAttribute("data-chat-id");
            if (!chatId) return;

            // cargar partial
            const ok = await loadChatPartial(chatId);
            if (!ok) return;

            // bandeja + polling
            await refreshBandeja();
            startPolling(chatId);

            // el scroll final lo hacemos cuando el modal ya se mostró del todo
            // (ver listener "shown.bs.modal")
        });

        // Cuando el modal terminó de abrir, scrollear sí o sí al final
        chatModalEl.addEventListener("shown.bs.modal", () => {
            scrollChatToBottom(true);

            // re-enfocar input si existe
            const input = document.querySelector("#sendMessageForm input[name='texto']");
            if (input) input.focus();
        });

        // Cerrar modal => parar polling
        chatModalEl.addEventListener("hidden.bs.modal", () => {
            stopPolling();
        });
    }

    // B) Enviar mensaje (AJAX)
    document.addEventListener("submit", async (e) => {
        const form = e.target;
        if (!form.matches("#sendMessageForm")) return;

        e.preventDefault();

        const res = await fetch(form.action, {
            method: "POST",
            body: new FormData(form),
            headers: { "X-Requested-With": "XMLHttpRequest" }
        });

        if (!res.ok) {
            console.error("Error enviando mensaje:", res.status, await res.text());
            return;
        }

        // Reemplazamos el body del modal por el partial actualizado
        const html = await res.text();
        $("chatModalBody").innerHTML = html;

        // Mantener el chat activo (tomamos el id del hidden)
        const chatId = form.querySelector("input[name='ConversacionId']")?.value;
        if (chatId) startPolling(chatId);

        // Scroll abajo y bandeja
        scrollChatToBottom(true);
        await refreshBandeja();

        // limpiar input (opcional) + focus
        const input = document.querySelector("#sendMessageForm input[name='texto']");
        if (input) {
            input.value = "";
            input.focus();
        }
    });

    // C) Filtro de chats (AJAX) -> actualiza bandejaContainer
    (() => {
        const form = $("filtroChatsForm");
        const container = $("bandejaContainer");
        if (!form || !container) return;

        form.addEventListener("submit", async (e) => {
            e.preventDefault();

            const url = form.action;
            const params = new URLSearchParams(new FormData(form)).toString();
            const fullUrl = url + (url.includes("?") ? "&" : "?") + params;

            const res = await fetch(fullUrl, { method: "GET" });
            const html = await res.text();

            if (!res.ok) {
                container.innerHTML = `<div class="p-3 text-danger small">Error ${res.status}<br>${html}</div>`;
                return;
            }

            container.innerHTML = html;
        });
    })();

    // D) Crear conversación desde filtro (AJAX) + abrir chat en modal
    document.addEventListener("click", async (e) => {
        const btn = e.target.closest(".js-create-chat");
        if (!btn) return;

        const receptorId = btn.getAttribute("data-user-id");
        if (!receptorId) return;

        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

        const fd = new FormData();
        fd.append("receptorId", receptorId);
        if (token) fd.append("__RequestVerificationToken", token);

        const res = await fetch("/Mensaje/CreateOrGet", {
            method: "POST",
            body: fd,
            headers: { "X-Requested-With": "XMLHttpRequest" }
        });

        if (!res.ok) {
            console.error("CreateOrGet error:", res.status, await res.text());
            return;
        }

        const data = await res.json();
        if (!data?.conversacionId) {
            console.error("CreateOrGet no devolvió conversacionId:", data);
            return;
        }

        // Abrir modal con el chat recién creado
        openChatById(data.conversacionId);
    });

    // Exponer openChatById por si lo querés llamar desde otro lado
    window.openChatById = openChatById;
})();


document.addEventListener("click", async (e) => {
    const btn = e.target.closest(".js-delete-chat");
    if (!btn) return;

    const chatId = btn.getAttribute("data-chat-id");
    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

    const fd = new FormData();
    fd.append("conversacionId", chatId);
    if (token) fd.append("__RequestVerificationToken", token);

    const res = await fetch("/Mensaje/EliminarConversacion", {
        method: "POST",
        body: fd,
        headers: { "X-Requested-With": "XMLHttpRequest" }
    });

    if (!res.ok) {
        console.error("EliminarConversacion error:", res.status, await res.text());
        return;
    }

    // Si estaba abierto ese chat, cerrás modal y frenás polling
    if (window.activeConvId && String(window.activeConvId) === String(chatId)) {
        const modalEl = document.getElementById("chatModal");
        bootstrap.Modal.getOrCreateInstance(modalEl).hide();
    }

    // refrescar bandeja
    if (window.refreshBandeja) await window.refreshBandeja();
});
