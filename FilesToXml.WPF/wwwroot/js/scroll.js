function scrollToBottom(element, duration = 300) {
    const start = element.scrollTop;
    const target = element.scrollHeight - element.clientHeight;
    const startTime = performance.now();

    function scrollAnimation(currentTime) {
        const elapsed = currentTime - startTime;
        const progress = Math.min(elapsed / duration, 1);

        element.scrollTop = start + progress * (target - start);

        if (progress < 1) {
            requestAnimationFrame(scrollAnimation);
        }
    }

    requestAnimationFrame(scrollAnimation);
}