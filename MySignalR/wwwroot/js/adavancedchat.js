var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/hubs/adavancedchat").build();

connection.start();

connection.on("NewUserOnline", function (userId, userName) {

    addMessage(`${userName} has opened a connection..`);
    //var spanOnline = document.getElementById(`spanOnline${userId}`);
    //if (typeof (spanOnline) != 'undefined' && spanOnline != null) {
    //    // Exists.
    //    spanOnline.classList.add("bg-success");
    //    spanOnline.classList.remove("bg-danger");
    //    spanOnline.setAttribute("title", "Online");
    //}

});

connection.on("NewUserDisconnected", function (userId, userName) {

    addMessage(`${userName} has closed a connection..`);
    //var spanOnline = document.getElementById(`spanOnline${userId}`);
    //if (typeof (spanOnline) != 'undefined' && spanOnline != null) {
    //    // Exists.
    //    spanOnline.classList.add("bg-success");
    //    spanOnline.classList.remove("bg-danger");
    //    spanOnline.setAttribute("title", "Online");
    //}

});

connection.on("RoomAdded", (roomId, roomName, userId, userName) =>
{
    addMessage(`${userName} has added the room: ${roomName}`);
    fillRoomDropDown();
});

connection.on("RoomDeleted", function (deleted, selected, roomName, userName) {
    addMessage(`${userName} has deleted the room: ${roomName}`);
    fillRoomDropDown();
});

connection.on("PublicMessage", (roomId, UserId, userName, message, roomName) =>
{
    addMessage(`${userName} has sent a message to the room ${roomName}: ${message}`);

});

connection.on("PrivateMessage", (senderId, senderName, receiverId, message, chatId, receiverName) => {
    addMessage(`${senderName} says ${message} to ${receiverName}`);
});


function sendPublicMessage()
{
    let inputMsg = document.getElementById("txtPublicMessage");
    let dllSelRoom = document.getElementById("ddlSelRoom");

    var message = inputMsg.value;
    let roomId = ddlSelRoom.value;
    let roomName = ddlSelRoom.options[ddlSelRoom.selectedIndex].text;

    connection.send("SendPublicMessage", Number(roomId), message, roomName);

    inputMsg.value = '';
}

function sendPrivateMessage() {
    let inputMsg = document.getElementById("txtPrivateMessage");
    let receiver = document.getElementById("ddlSelUser");

    var message = inputMsg.value;
    let receiverId = receiver.value;
    let receiverName = receiver.options[receiver.selectedIndex].text;

    connection.send("SendPrivateMessage", receiverId, message, receiverName);

    inputMsg.value = '';
}

function addnewRoom(maxRoom) {

    let createRoomName = document.getElementById('createRoomName');

    var roomName = createRoomName.value;

    if (roomName == null && roomName == '') {
        return;
    }

    /*POST*/
    $.ajax({
        url: '/ChatRooms/PostChatRoom',
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ id: 0, name: roomName }),
        async: true,
        processData: false,
        cache: false,
        success: function (json) {

            connection.invoke("SendRoomAdded", json.id, json.name);
            createRoomName.value = '';
        },
        error: function (xhr) {
            alert('error');
        }
    })



}

function deleteRoom() {

    var options = document.getElementById('ddlDelRoom');
    var roomName = options.options[options.selectedIndex].text;

    var roomId = options.value;

    let text = `Do you want to delete Chat Room ${roomName}?`;
    if (confirm(text) == false) {
        return;
    }

    /*DELETE*/
    $.ajax({
        url: `/ChatRooms/DeleteChatRoom/${roomId}`,
        dataType: "json",
        type: "DELETE",
        contentType: 'application/json;',
        async: true,
        processData: false,
        cache: false,
        success: function (json) {
            connection.invoke("SendRoomDeleted", json.deleted, json.selected, roomName).catch(function (err) {
                return console.error(err.toString());
            });
        },
        error: function (xhr) {
            alert('error');
        }
    });

    //$.getJSON('/ChatRooms/DeleteChatRoom', {id: roomId })
    //    .done(function (json) {

    //connection.invoke("SendDeleteRoomMessage", json.deleted, json.selected, roomName).catch(function (err) {
    //    return console.error(err.toString());
    //});

    //    })
    //    .fail(function (jqxhr, textStatus, error) {
    //        var err = textStatus + ", " + error;
    //        console.log("Request Failed: " + err);
    //    });
}

document.addEventListener('DOMContentLoaded', (event) => {
    fillRoomDropDown();
    fillUserDropDown();
})


function fillUserDropDown() {

    $.getJSON('/ChatRooms/GetChatUser')
        .done(function (json) {

            var ddlSelUser = document.getElementById("ddlSelUser");

            ddlSelUser.innerText = null;

            json.forEach(function (item) {
                var newOption = document.createElement("option");

                newOption.text = item.userName;//item.whateverProperty
                newOption.value = item.id;
                ddlSelUser.add(newOption);


            });

        })
        .fail(function (jqxhr, textStatus, error) {

            var err = textStatus + ", " + error;
            console.log("Request Failed: " + jqxhr.detail);
        });

}

function fillRoomDropDown() {

    $.getJSON('/ChatRooms/GetChatRoom')
        .done(function (json) {
            var ddlDelRoom = document.getElementById("ddlDelRoom");
            var ddlSelRoom = document.getElementById("ddlSelRoom");

            ddlDelRoom.innerText = null;
            ddlSelRoom.innerText = null;

            json.forEach(function (item) {
                var newOption = document.createElement("option");

                newOption.text = item.name;
                newOption.value = item.id;
                ddlDelRoom.add(newOption);


                var newOption1 = document.createElement("option");

                newOption1.text = item.name;
                newOption1.value = item.id;
                ddlSelRoom.add(newOption1);

            });

        })
        .fail(function (jqxhr, textStatus, error) {

            var err = textStatus + ", " + error;
            console.log("Request Failed: " + jqxhr.detail);
        });

}


function addMessage(msg) {
    if (msg == null || msg == '') {
        return;
    }

    var ui = document.getElementById("messagesList");
    var li = document.createElement("li");
    li.innerHTML = msg;
    ui.appendChild(li);
}