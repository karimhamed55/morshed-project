document.addEventListener("click", (e) => {
    const bgColor = window.getComputedStyle(document.body).backgroundColor;
    const rgb = bgColor.match(/\d+/g).map(Number);
    const brightness = (rgb[0] * 299 + rgb[1] * 587 + rgb[2] * 114) / 1000;

    const isDark = brightness < 128;

    const splash = document.createElement("div");
    splash.className = "splash-click";
    splash.style.left = `${e.clientX}px`;
    splash.style.top = `${e.clientY}px`;

    if (isDark) {
        // لو الخلفية غامقة → سبلاش فاتح ناعم
        splash.style.background = `
        radial-gradient(
            circle,
            rgba(255, 219, 88, 0.12) 0%,
            rgba(0, 153, 204, 0.10) 40%,
            rgba(194, 162, 113, 0.08) 80%,
            transparent 100%
        )`;
    } else {
        // لو الخلفية فاتحة → سبلاش أغمق شوية
        splash.style.background = `
        radial-gradient(
            circle,
            rgba(255, 219, 88, 0.08) 0%,
            rgba(0, 119, 182, 0.10) 35%,
            rgba(194, 162, 113, 0.12) 70%,
            transparent 100%
        )`;
    }

    document.body.appendChild(splash);
    setTimeout(() => splash.remove(), 700);
});