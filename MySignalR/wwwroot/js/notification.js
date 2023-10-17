var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/hubs/notification").build();

document.getElementById("sendButton").disabled = true;
connection.start().then(fulfilled, rejected);

connection.on("LoadMessages", function (messages, counter)
{
    var messagesInput = document.getElementById("messageList");
    messagesInput.innerHTML = "";

    var notificationCounter = document.getElementById("notificationCounter");
    notificationCounter.innerHTML = "<span>(" + counter + ")</span>";

    for (var i = messages.length - 1; i >= 0; i--) {
        var li = document.createElement("li");
        li.textContent = "Notification - " + messages[i];
        messagesInput.appendChild(li);
    }
});

document.getElementById("sendButton").addEventListener("click",(event) =>
{
    var message = document.getElementById("notificationInput").value;
    connection.send("SendMessage", message).then(function ()
    {
        document.getElementById("notificationInput").value = "";
    });

    event.preventDefault();
});

function fulfilled() {
    console.log("Connection was Successful");
    document.getElementById("sendButton").disabled = false;
    connection.send("LoadMessages");
}
function rejected() {
    console.log("Connection Rejected");
}