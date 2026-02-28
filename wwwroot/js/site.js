// mobile side navbar function
document.addEventListener("DOMContentLoaded", function () {
    const openBtn = document.querySelector('.openIcon').parentElement;
    const closeBtn = document.querySelector('.closeIcon').parentElement;
    const sideBar = document.getElementById('sideBar');
    const allLinks = document.querySelectorAll('#sideBar a');

    // Hide the close button at the beginning
    closeBtn.style.display = 'none';

    // Logic for opening the menu
    openBtn.addEventListener('click', function () {
        sideBar.classList.add('sidebar-active');
        openBtn.style.display = 'none';
        closeBtn.style.display = 'block';
    });

    // Menu close function
    function closeMenu() {
        sideBar.classList.remove('sidebar-active');
        openBtn.style.display = 'block';
        closeBtn.style.display = 'none';
    }

    closeBtn.addEventListener('click', closeMenu);

    // Clicking on any link will close the sidebar
    allLinks.forEach(link => {
        link.addEventListener('click', closeMenu);
    });
});


