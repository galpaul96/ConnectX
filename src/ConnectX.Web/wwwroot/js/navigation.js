(() => {
    let activeShell = null;
    let dispose = null;

    function initialize() {
        const shell = document.querySelector('.app-shell');
        if (shell === activeShell) return;
        dispose?.();
        activeShell = shell;
        if (!shell) return;

        const sidebar = shell.querySelector('.app-sidebar');
        const main = shell.querySelector('.app-main');
        const toggle = shell.querySelector('.menu-toggle');
        const close = shell.querySelector('.sidebar-close');
        const scrim = shell.querySelector('.sidebar-scrim');
        const narrow = matchMedia('(max-width: 900px)');
        const controller = new AbortController();
        const options = { signal: controller.signal };

        function setOpen(open, restoreFocus = false) {
            const isOpen = narrow.matches && open;
            shell.classList.toggle('navigation-open', isOpen);
            toggle.setAttribute('aria-expanded', String(isOpen));
            sidebar.inert = narrow.matches && !isOpen;
            main.inert = isOpen;
            document.body.classList.toggle('navigation-open', isOpen);
            if (isOpen) close.focus();
            else if (restoreFocus) toggle.focus();
        }

        toggle.addEventListener('click', () => setOpen(true), options);
        close.addEventListener('click', () => setOpen(false, true), options);
        scrim.addEventListener('click', () => setOpen(false, true), options);
        sidebar.addEventListener('click', event => {
            if (event.target.closest('a')) setOpen(false);
        }, options);
        document.addEventListener('keydown', event => {
            if (!shell.classList.contains('navigation-open')) return;
            if (event.key === 'Escape') {
                setOpen(false, true);
                return;
            }
            if (event.key !== 'Tab') return;
            const focusable = [...sidebar.querySelectorAll('a, button, input, select, textarea, [tabindex]:not([tabindex="-1"])')]
                .filter(element => !element.disabled);
            if (!focusable.length) return;
            if (event.shiftKey && document.activeElement === focusable[0]) {
                event.preventDefault();
                focusable.at(-1).focus();
            } else if (!event.shiftKey && document.activeElement === focusable.at(-1)) {
                event.preventDefault();
                focusable[0].focus();
            }
        }, options);
        narrow.addEventListener('change', () => setOpen(false), options);
        setOpen(false);
        dispose = () => {
            controller.abort();
            document.body.classList.remove('navigation-open');
        };
    }

    new MutationObserver(initialize).observe(document.body, { childList: true, subtree: true });
    initialize();
})();
