var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/hubs/chat").build();

document.getElementById("sendMessage").disabled = true;
connection.start().then(fulfilled, rejected);

document.getElementById("sendMessage").addEventListener("click", function (event)
{
    let sender = document.getElementById("senderEmail").value;
    let message = document.getElementById("chatMessage").value;
    let receiver = document.getElementById("receiverEmail").value;

    if (receiver.length > 0) {
        connection.send("SendMessageToReceiver", sender, receiver, message).then(() => {
            document.getElementById("chatMessage").value = "";
            document.getElementById("receiverEmail").value = "";
        });
    }
    else
    {
        connection.send("SendMessageToAll", sender, message).then(() => {
            document.getElementById("chatMessage").value = "";
        });

    }
});

connection.on("MessageReceived", function (user, message)
{
    let ul = document.getElementById("messagesList");
    let li = document.createElement("li");
    li.textContent = `${user} - ${message}`;

    ul.appendChild(li);
});

function fulfilled() {
    console.log("Connection was Successful");
    document.getElementById("sendMessage").disabled = false;
}
function rejected() {
    console.log("Connection Rejected");
}