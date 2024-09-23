function eliminar(id, button, string) {
    const confirmacion = confirm('¿Estás seguro de que quieres borrarlo?');
    if (confirmacion) {
      fetch(`/${string}/Delete/${id}`, {
        method: 'POST'
      })
        .then(response => {
          if (response.ok) {
            window.location.href = `/${string}`;
          } else {
            console.error('Error al borrar.');
          }
        })
        .catch(error => {
          console.error('Error al realizar la solicitud:', error);
        });
    }
  }