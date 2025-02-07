document.addEventListener("DOMContentLoaded", () => {
  loadUrls();
});

const API_URL = "/api/ShortenApi";

const validateUrl = (url) => {
  try {
    const parsedUrl = new URL(url);

    const domainPattern =
      /\.(com|net|org|edu|gov|ru|by|ua|io|dev|info|biz|tv|xyz|en)$/i;

    if (!domainPattern.test(parsedUrl.hostname)) {
      return false;
    }

    if (
      parsedUrl.hostname.startsWith("www.") &&
      parsedUrl.hostname.split(".").length < 3
    ) {
      return false;
    }

    return true;
  } catch (e) {
    return false;
  }
};

const loadUrls = () => {
  fetch(API_URL)
    .then((response) => response.json())
    .then((data) => {
      const tbody = document.getElementById("urlTableBody");
      tbody.innerHTML = "";
      data.forEach((url) => {
        const row = `<tr>
                      <td><a href="${url.originalUrl}" target="_blank">${url.originalUrl}</a></td>
                      <td><a href="${API_URL}/${url.shortenedUrl}" target="_blank">${url.shortenedUrl}</a></td>
                      <td>${new Date(url.createdAt).toLocaleDateString()}</td>
                      <td>${url.clicks}</td>
                      <td><button class="button is-danger is-small" onclick="deleteUrl(${url.id})">Delete</button></td>
                    </tr>`;
        tbody.innerHTML += row;
      });
    });
};

const shortenUrl = () => {
  const originalUrl = document.getElementById("originalUrl").value;

  if (validateUrl(originalUrl)) {
    fetch(API_URL, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ originalUrl: originalUrl }),
    })
      .then((response) => response.json())
      .then(() => {
        loadUrls();
      })
      .catch((error) => {
        Toastify({
          text: `${error}`,
          position: "center",
          style: {
            background:
              "linear-gradient(to right, rgb(255, 95, 109), rgb(255, 195, 113))",
          },
        }).showToast();
      });
  } else
    Toastify({
      text: `Invalid URL`,
      position: "center",
      style: {
        background:
          "linear-gradient(to right, rgb(255, 95, 109), rgb(255, 195, 113))",
      },
    }).showToast();
};

const deleteUrl = (id) => {
  fetch(`${API_URL}/${id}`, {
    method: "DELETE",
  })
    .then(() => {
      loadUrls();
    })
    .catch((error) => {
      console.error("Error:", error);
    });
};
