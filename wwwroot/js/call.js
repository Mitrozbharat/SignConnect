
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/callHub")
    .build();

connection.start();

function startCall() {
    const name = document.getElementById("userName").value;
    const type = document.getElementById("callType").value;
    const mobile = document.getElementById("mobile").value;

    if (!name) return alert("Enter name");

    if (type === "whatsapp") {
        if (!mobile) return alert("Enter mobile number");
        window.open(`https://wa.me/${mobile}`, "_blank");
        return;
    }

    document.getElementById("meetingUI").style.display = "block";
    connection.invoke("JoinRoom", name);
}

function sendMessage() {
    const name = document.getElementById("userName").value;
    const message = document.getElementById("messageInput").value;
    if (!message) return;

    connection.invoke("SendMessage", name, message);
    document.getElementById("messageInput").value = "";
}

connection.on("ReceiveMessage", function (user, message, time) {
    const msgDiv = document.createElement("div");
    msgDiv.className = user === document.getElementById("userName").value
        ? "msg-you"
        : "msg-other";

    msgDiv.innerHTML = `<b>${user}</b>: ${message} <small>${time}</small>`;

    const messages = document.getElementById("messages");
    messages.appendChild(msgDiv);
    messages.scrollTop = messages.scrollHeight;
});

connection.on("UserJoined", function (user) {
    document.getElementById("remoteBox").innerText = user + " Connected";
});

connection.on("UserLeft", function () {
    document.getElementById("remoteBox").innerText = "Waiting for user...";
});
