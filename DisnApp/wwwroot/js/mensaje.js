
async function refreshBandeja() {
    const cont = document.getElementById("bandejaContainer");
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

// --- state ---
let pollTimer = null;
let activeConvId = null;

// --- helpers ---
function isNearBottom(el, threshold = 120) {
    return el.scrollHeight - el.scrollTop - el.clientHeight < threshold;
}

function stopPolling() {
    if (pollTimer) clearInterval(pollTimer);
    pollTimer = null;
    activeConvId = null;
}

async function refreshMessagesOnly() {
    if (!activeConvId) return;

    const scroll = document.getElementById("chatScroll");
    const wasNear = scroll ? isNearBottom(scroll) : true;

    const res = await fetch(`/Mensaje/Chat?id=${encodeURIComponent(activeConvId)}&t=${Date.now()}`, {
        headers: { "X-Requested-With": "XMLHttpRequest" }
    });

    if (!res.ok) return;

    const html = await res.text();

    // 
    const doc = new DOMParser().parseFromString(html, "text/html");
    const newMessages = doc.getElementById("chatMessages");
    const currentMessages = document.getElementById("chatMessages");

    if (!newMessages || !currentMessages) return;

    currentMessages.innerHTML = newMessages.innerHTML;

    const newScroll = document.getElementById("chatScroll");
    if (newScroll && wasNear) newScroll.scrollTop = newScroll.scrollHeight;
}

// --- modal open: load chat + start polling ---
const chatModalEl = document.getElementById("chatModal");

chatModalEl.addEventListener("show.bs.modal", async (event) => {
    const btn = event.relatedTarget;
    const chatId = btn?.getAttribute("data-chat-id");
    if (!chatId) return;

    activeConvId = chatId;

    const body = document.getElementById("chatModalBody");
    body.innerHTML = `<div class="p-4 text-center">Cargando...</div>`;

    const res = await fetch(`/Mensaje/Chat?id=${encodeURIComponent(activeConvId)}&t=${Date.now()}`, {
        headers: { "X-Requested-With": "XMLHttpRequest" }
    });

    if (!res.ok) {
        body.innerHTML = `<div class="p-4 text-danger">Error cargando el chat.</div>`;
        return;
    }

    body.innerHTML = await res.text();
    scrollChatToBottom();
    await refreshBandeja();



    // scroll al final al abrir
    const scroll = document.getElementById("chatScroll");
    if (scroll) scroll.scrollTop = scroll.scrollHeight;

    // polling
    stopPolling();             // limpia timer anterior
    activeConvId = chatId;     // lo restauramos
    pollTimer = setInterval(refreshMessagesOnly, 2500);
});

chatModalEl.addEventListener("hidden.bs.modal", () => {
    stopPolling();
});

// --- submit ajax: post message and refresh whole partial ---
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

    const html = await res.text();
    document.getElementById("chatModalBody").innerHTML = html;
    scrollChatToBottom();
    await refreshBandeja();


    // scroll al final
    const scroll = document.getElementById("chatScroll");
    if (scroll) scroll.scrollTop = scroll.scrollHeight;

    // re-enfocar input
    const input = document.querySelector("#sendMessageForm input[name='texto']");
    if (input) input.focus();
});


function scrollChatToBottom() {
    const scroll = document.getElementById("chatScroll");
    if (scroll) scroll.scrollTop = scroll.scrollHeight;
}

// cuando el modal YA se mostró, bajá al final
chatModalEl.addEventListener("shown.bs.modal", () => {
    scrollChatToBottom();
    setTimeout(scrollChatToBottom, 50); // por si hay layout/imagenes
});