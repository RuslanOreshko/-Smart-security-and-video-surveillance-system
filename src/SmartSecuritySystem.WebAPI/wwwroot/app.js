const logs = document.getElementById("logs");
const status = document.getElementById("status");

let lastMotionTime = Date.now();
let currentAlarm = null;

const connection = new signalR.HubConnectionBuilder()
  .withUrl("/hubs/notifications")
  .build();

connection.on("ReceiveNotification", (msg) => {
  const now = new Date().toLocaleTimeString();

  if (msg === "ALARM_START") {
    status.textContent = "ALARM";
    status.className = "alarm";

    currentAlarm = document.createElement("li");
    currentAlarm.textContent = `${now} - ...`;
    logs.prepend(currentAlarm);
  } else if (msg === "ALARM_END") {
    status.textContent = "SAFE";
    status.className = "safe";

    if (currentAlarm) {
      currentAlarm.textContent = currentAlarm.textContent.replace("...", now);
      currentAlarm = null;
    }
  } else if (msg === "DISARM") {
    status.textContent = "OFF";
    status.className = "disarm";
  }
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
