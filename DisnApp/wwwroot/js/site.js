/* ===== DESKTOP: SIDEBAR FIJA ===== */
.sidebar{
    position: fixed!important;
    top: 0!important;
    left: 0!important;
    width: 260px!important;
    height: 100vh!important;
    z - index: 1020!important;
    overflow - y: auto!important;
    border - bottom: 0!important;
    background: #fff!important;
}

/* anula navbar-expand-sm que lo pone horizontal */
.sidebar.navbar - expand - sm.navbar - nav{
    flex - direction: column!important;
    width: 100 % !important;
}

/* empuja el contenido (tu layout usa .container) */
body > .container{
    margin - left: 260px!important;
    max - width: calc(100 % - 260px)!important;
}

/* footer alineado con contenido */
footer.footer{
    margin - left: 260px!important;
    max - width: calc(100 % - 260px)!important;
}

/* ===== MOBILE: esconder sidebar + bottom nav ===== */
@media(max - width: 767.98px) {

  /* ocultamos sidebar en celular */
  .sidebar{
        display: none!important;
    }

    /* el contenido vuelve full width */
    body > .container,
        footer.footer{
        margin - left: 0!important;
        max - width: 100 % !important;
    }

  /* dejamos espacio para la bottom bar */
  body{
        padding - bottom: 56px;
    }

  /* en móvil, oculto textos (por si se muestra algo) */
  .nav - text,
  .navbar - brand{
        display: none!important;
    }
}

/* ===== bottom nav estilo IG ===== */
.ig - bottomnav{
    height: 56px;
    z - index: 1030;
}

.ig - bottomnav.material - symbols - outlined{
    font - size: 28px;
}

.ig - iconbtn{
    padding: .25rem .5rem;
    border: 0;
    background: transparent;
}

