document.addEventListener("DOMContentLoaded", function () {
    const accordionItems = document.querySelectorAll('.accordion-item');

    accordionItems.forEach(item => {
        const button = item.querySelector('.accordion-button');
        const collapse = item.querySelector('.accordion-collapse');

        button.addEventListener('click', () => {
            // Collapse all other sections except the current one
            accordionItems.forEach(otherItem => {
                const otherButton = otherItem.querySelector('.accordion-button');
                const otherCollapse = otherItem.querySelector('.accordion-collapse');
                //if (otherButton !== button) {
                    otherButton.setAttribute('aria-expanded', 'false');
                    otherCollapse.classList.remove('show');
                    otherItem.classList.remove('active'); // Remove active class from other items
                //}
            });

            // Toggle the clicked section
            const currentState = button.getAttribute('aria-expanded') === 'true' || false;
            button.setAttribute('aria-expanded', !currentState);
            if (!currentState) {
                collapse.classList.add('show');
                item.classList.add('active'); // Add active class to the clicked item
            } else {
                collapse.classList.remove('show');
                item.classList.remove('active'); // Remove active class if collapsed
            }
        });

        // Check if the current page URL matches any of the sidebar links
        const links = item.querySelectorAll('a');
        links.forEach(link => {
            if (link.href === window.location.href) {
                // Expand the parent accordion item and set it as active
                const parentCollapse = item.querySelector('.accordion-collapse');
                parentCollapse.classList.add('show');
                button.setAttribute('aria-expanded', 'true');
                item.classList.add('active-page'); // Add active-page class to the matched item
            }
        });
    });
});
