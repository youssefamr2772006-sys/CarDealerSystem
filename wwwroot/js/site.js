document.addEventListener("DOMContentLoaded", function () {

    if (window.lucide) {
        lucide.createIcons();
    }

    var sidebar = document.querySelector(".sidebar");
    var backdrop = document.querySelector(".sidebar-backdrop");
    var toggleBtn = document.querySelector(".sidebar-toggle");

    var MOBILE_BREAKPOINT = 991;

    function isMobile() {
        return window.innerWidth <= MOBILE_BREAKPOINT;
    }

    function closeMobileSidebar() {
        sidebar.classList.remove("show");
        backdrop.classList.remove("show");
    }

    function openMobileSidebar() {
        sidebar.classList.add("show");
        backdrop.classList.add("show");
    }

    if (toggleBtn && sidebar && backdrop) {
        toggleBtn.addEventListener("click", function () {
            if (isMobile()) {
                if (sidebar.classList.contains("show")) {
                    closeMobileSidebar();
                } else {
                    openMobileSidebar();
                }
            } else {
                document.body.classList.toggle("sidebar-collapsed");
            }
        });

        backdrop.addEventListener("click", closeMobileSidebar);

        window.addEventListener("resize", function () {
            if (!isMobile()) {
                closeMobileSidebar();
            }
        });
    }

    // Multi-file upload preview
    document.querySelectorAll('input[type="file"][data-preview-target]').forEach(function (input) {
        input.addEventListener("change", function () {
            var target = document.getElementById(input.getAttribute("data-preview-target"));

            if (!target) {
                return;
            }

            target.innerHTML = "";

            Array.from(input.files)
                .slice(0, 12)
                .forEach(function (file) {
                    if (!file.type.startsWith("image/")) {
                        return;
                    }

                    var item = document.createElement("div");
                    item.className = "image-preview-item";

                    var img = document.createElement("img");
                    img.src = URL.createObjectURL(file);
                    img.alt = file.name;

                    item.appendChild(img);
                    target.appendChild(item);
                });
        });
    });

    // Photo album lightbox
    var galleryThumbs = Array.from(document.querySelectorAll(".gallery-thumb"));
    var overlay = document.querySelector(".lightbox-overlay");

    if (galleryThumbs.length && overlay) {
        var lightboxImage = overlay.querySelector(".lightbox-image");
        var counter = overlay.querySelector(".lightbox-counter");
        var closeBtn = overlay.querySelector(".lightbox-close");
        var prevBtn = overlay.querySelector(".lightbox-prev");
        var nextBtn = overlay.querySelector(".lightbox-next");
        var current = 0;

        function showImage(index) {
            current = (index + galleryThumbs.length) % galleryThumbs.length;
            lightboxImage.src = galleryThumbs[current].getAttribute("data-full");
            counter.textContent = (current + 1) + " / " + galleryThumbs.length;
        }

        function closeLightbox() {
            overlay.classList.remove("show");
        }

        galleryThumbs.forEach(function (thumb, index) {
            thumb.addEventListener("click", function () {
                showImage(index);
                overlay.classList.add("show");
            });
        });

        if (closeBtn) closeBtn.addEventListener("click", closeLightbox);
        if (prevBtn) prevBtn.addEventListener("click", function () { showImage(current - 1); });
        if (nextBtn) nextBtn.addEventListener("click", function () { showImage(current + 1); });

        overlay.addEventListener("click", function (e) {
            if (e.target === overlay) {
                closeLightbox();
            }
        });

        document.addEventListener("keydown", function (e) {
            if (!overlay.classList.contains("show")) {
                return;
            }

            if (e.key === "Escape") closeLightbox();
            if (e.key === "ArrowLeft") showImage(current - 1);
            if (e.key === "ArrowRight") showImage(current + 1);
        });
    }

    // Global vehicle search (topbar)
    var searchInput = document.getElementById("globalCarSearch");
    var searchResults = document.getElementById("globalCarSearchResults");

    if (searchInput && searchResults) {
        var debounceTimer = null;
        var currentRequestToken = 0;

        function escapeHtml(value) {
            return String(value).replace(/[&<>"']/g, function (ch) {
                return { "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" }[ch];
            });
        }

        function formatPrice(value) {
            var number = Number(value) || 0;
            return "$" + number.toLocaleString(undefined, { maximumFractionDigits: 0 });
        }

        function showResults(html) {
            searchResults.innerHTML = html;
            searchResults.classList.add("show");
        }

        function renderResults(items, query) {
            if (!items.length) {
                showResults('<div class="search-empty">No vehicles match "' + escapeHtml(query) + '"</div>');
                return;
            }

            var html = items.map(function (item) {
                var title = escapeHtml(item.brand + " " + item.model) + " &middot; " + item.year;
                var meta = escapeHtml(item.branch || "");

                return (
                    '<a class="search-result-item" href="/Cars/Details/' + item.id + '">' +
                        "<div>" +
                            '<div class="search-result-title">' + title + "</div>" +
                            '<div class="search-result-meta">' + meta + "</div>" +
                        "</div>" +
                        '<div class="search-result-price">' + formatPrice(item.price) + "</div>" +
                    "</a>"
                );
            }).join("");

            showResults(html);
        }

        searchInput.addEventListener("input", function () {
            var query = searchInput.value.trim();

            clearTimeout(debounceTimer);

            if (query.length < 2) {
                searchResults.classList.remove("show");
                searchResults.innerHTML = "";
                return;
            }

            debounceTimer = setTimeout(function () {
                var requestToken = ++currentRequestToken;

                showResults('<div class="search-loading">Searching&hellip;</div>');

                fetch("/Cars/Search?q=" + encodeURIComponent(query))
                    .then(function (res) { return res.json(); })
                    .then(function (data) {
                        if (requestToken === currentRequestToken) {
                            renderResults(data, query);
                        }
                    })
                    .catch(function () {
                        if (requestToken === currentRequestToken) {
                            showResults('<div class="search-empty">Something went wrong. Try again.</div>');
                        }
                    });
            }, 300);
        });

        searchInput.addEventListener("focus", function () {
            if (searchResults.innerHTML.trim() !== "" && searchInput.value.trim().length >= 2) {
                searchResults.classList.add("show");
            }
        });

        document.addEventListener("click", function (e) {
            if (!searchInput.contains(e.target) && !searchResults.contains(e.target)) {
                searchResults.classList.remove("show");
            }
        });

        document.addEventListener("keydown", function (e) {
            if (e.key === "Escape") {
                searchResults.classList.remove("show");
            }
        });
    }

});
