.sidebar{
    position: fixed!important;
    top: 0!important;
    left: 0!important;
    width: 260px!important;
    height: 100vh!important;
    z - index: 1020!important;
    overflow - y: auto!important;
    border - bottom: 0!important;
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

footer.footer{
    margin - left: 260px!important;
}

@media(max - width: 576px) {
  .sidebar{
        position: static!important;
        width: 100 % !important;
        height: auto!important;
    }
    body > .container, footer.footer{
        margin - left: 0!important;
        max - width: 100 % !important;
    }
}
