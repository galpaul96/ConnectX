const dialogs = new WeakMap();
export function open(dialog, reference) {
    const previousFocus = document.activeElement;
    const root = document.documentElement;
    const previousOverflow = root.style.overflow;
    const previousGutter = root.style.scrollbarGutter;
    root.style.scrollbarGutter = "stable";
    root.style.overflow = "hidden";
    const cancel = event => { event.preventDefault(); reference.invokeMethodAsync("CloseAsync"); };
    const click = event => {
        if (event.target !== dialog) return;
        const rect = dialog.getBoundingClientRect();
        if (dialog.classList.contains("spell-modal-backdrop") || event.clientX < rect.left || event.clientX > rect.right || event.clientY < rect.top || event.clientY > rect.bottom)
            reference.invokeMethodAsync("CloseAsync");
    };
    const keydown = event => {
        if (event.key !== "Tab") return;
        const focusable = [...dialog.querySelectorAll('button, a[href], input, select, textarea, summary, [tabindex]:not([tabindex="-1"])')]
            .filter(element => !element.disabled && element.getClientRects().length > 0);
        const first = focusable[0];
        const last = focusable.at(-1);
        if (!first) return;
        if (event.shiftKey && (document.activeElement === first || !dialog.contains(document.activeElement))) {
            event.preventDefault();
            last.focus();
        } else if (!event.shiftKey && document.activeElement === last) {
            event.preventDefault();
            first.focus();
        }
    };
    dialogs.set(dialog, { previousFocus, cancel, click, keydown, previousOverflow, previousGutter });
    dialog.addEventListener("cancel", cancel);
    dialog.addEventListener("click", click);
    dialog.addEventListener("keydown", keydown);
    if (!dialog.open) dialog.showModal();
}
export function close(dialog) {
    const state = dialogs.get(dialog);
    dialog.close();
    if (state) {
        document.documentElement.style.overflow = state.previousOverflow;
        document.documentElement.style.scrollbarGutter = state.previousGutter;
        dialog.removeEventListener("cancel", state.cancel);
        dialog.removeEventListener("click", state.click);
        dialog.removeEventListener("keydown", state.keydown);
        if (state.previousFocus?.isConnected) state.previousFocus.focus();
    }
    dialogs.delete(dialog);
}
