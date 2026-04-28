const logs = document.getElementById("logs");
const status = document.getElementById("status");

let lastMotionTime = Date.now();

const connection = new signalR.HubConnectionBuilder()
  .withUrl("/hubs/notifications")
  .build();

connection.on("ReceiveNotification", (message) => {
  lastMotionTime = Date.now();

  status.textContent = "ALARM!!!!!!!!!!!!!!";
  status.className = "alarm";

  const li = document.createElement("li");
  li.textContent = message;
  logs.appendChild(li);
});

setInterval(() => {
  const now = Date.now();

  if (
    now - lastMotionTime > 60000 &&
    status.className !== "safe" &&
    status.className !== "disarm"
  ) {
    status.textContent = "SAFE";
    status.className = "safe";
  }
}, 1000);

connection
  .start()
  .then(() => console.log("Connected"))
  .catch((err) => console.log(err));

document.getElementById("armBtn").onclick = () => {
  fetch("/api/security/arm", { method: "POST" });
};

document.getElementById("disarmBtn").onclick = () => {
  status.textContent = "protection is not active";
  status.className = "disarm";
  fetch("/api/security/disarm", { method: "POST" });
};
