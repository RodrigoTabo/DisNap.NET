# DisnApp — Version 1 (MVP)

DisnApp es una app tipo red social construida en ASP.NET Core (MVC) con foco en back-end, arquitectura en capas y persistencia con Entity Framework.

## ✅ Estado actual (V1)

V1 es una versión MVP: la aplicación es usable y tiene funcionalidades core implementadas, pero todavía hay features incompletas y bugs conocidos.

## Funcionalidades implementadas

### Publicaciones

- ✅ Crear y subir publicaciones (imagen/contenido)
- ✅ Feed de publicaciones
- ✅ Comentarios en publicaciones (funcional)
- ✅ Likes en publicaciones (funcional)

### Perfil

- ✅ Pantalla de perfil
- ✅ Cantidad de publicaciones
- ✅ Cantidad de seguidores y seguidos
- ✅ Carga/listado de publicaciones del usuario

### Historias

- ✅ Visualización/listado de historias activas (casi terminado)
- ❌ Agregar historia (pendiente)
- ❌ Eliminar historia (pendiente)

### Mensajes

- ✅ Bandeja/lista de conversaciones
- ✅ Apertura del chat en modal lateral (Bootstrap)
- ✅ Carga del historial de mensajes por conversación (partial)
- ✅ Enviar mensaje (pendiente)
- ❌ Borrar mensajes / borrar conversación (pendiente)

## Arquitectura / stack

- ASP.NET Core MVC
- Entity Framework Core
- Identity (autenticación/usuarios)
- Bootstrap (UI)

## 🐞 Known issues (V1)

- Historias: falta completar alta/baja
- Mensajes: falta envío y eliminación
- Algunos flujos pueden requerir mejoras de validación y manejo de errores (UI/UX)

## Roadmap (V2)

- [ ] Historias: agregar y eliminar
- [ ] Mensajes: POST enviar (ideal: AJAX para refrescar el modal sin recargar)
- [ ] Mensajes: borrar mensaje / conversación
- [ ] Validaciones extra (servidor + UI)
- [ ] Refactor y limpieza de código (nombres, capas, duplicados)
- [ ] Mejoras de performance (consultas, includes, paginación si aplica)

## Notas

Esta versión se publica como “MVP funcional”. La versión 2 enfocará en completar features, corregir bugs y mejorar calidad general.
