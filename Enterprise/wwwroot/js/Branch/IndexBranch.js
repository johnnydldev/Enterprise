document.addEventListener('DOMContentLoaded', function () {
    showBranch();
});

//show all branches function
function showBranch() {
    fetch('/branch/showBranches')
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error: ${response.statusText}`)
            }
            return response.json
        })
        .then(responseJson => {
            console.log(responseJson);
            const tbody = document.querySelector('#branch-table tbody');
            tbody.innerHTML = '';

            if (responseJson.length > 0) {
                responseJson.forEach(branch => {
                    const row = document.createElement('tr');
                    row.innerHTML = `
                        <td>${branch.id}</td>
                        <td>${branch.description}</td>
                        <td class="">
                            <button class="rounded-component btn-borderless bg-green-lime" data-branch='${JSON.stringify(branch)}'>Editar</button>
                            <button class="rounded-component btn-borderless bg-green-lime" data-branch='${JSON.stringify(branch)}'>Detalles</button>
                            <button class="rounded-component btn-borderless bg-green-lime" data-branch='${JSON.stringify(branch)}'>Eliminar</button>
                        </td>
                    `;
                    tbody.appendChild(row)
                })

                Swal.fire({
                    icon: 'success',
                    title: 'Sucursales cargadas',
                    text: 'Las sucursales se han cargado correctamente',
                    timer: 2000,
                    showConnfirmButton: false
                });

            } else {
                tbody.innerHTML = `<tr><td colspan='6' class="text-center">No se encontraron productos.</td></tr>`;
            }
        })
        .catch(error => {
            console.error('Error:', error);

            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Hubo un problema al cargar las sucursales'
            });
        });

}//End show all branches