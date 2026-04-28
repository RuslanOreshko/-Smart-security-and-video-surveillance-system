const logs = document.getElementById("logs");

const connection = new signalR.HubConnectionBuilder()
  .withUrl("/hubs/notifications")
  .build();

connection.on("ReceiveNotification", (message) => {
  const li = document.createElement("li");
  li.textContent = message;
  logs.appendChild(li);
});

connection
  .start()
  .then(() => console.log("Connected"))
  .catch((err) => console.log(err));

document.getElementById("armBtn").onclick = () => {
  fetch("/api/security/arm", { method: "POST" });
};

document.getElementById("disarmBtn").onclick = () => {
  fetch("/api/security/disarm", { method: "POST" });
};
