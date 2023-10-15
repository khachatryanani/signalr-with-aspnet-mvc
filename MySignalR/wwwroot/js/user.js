var connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/user").build();

connection.on("UpdateTotalViews", (value) =>
{
    var counterSpan = document.getElementById("totalViewsCounter");
    counterSpan.innerText = value.toString();
});

connection.on("UpdateTotalUsers", (value) => {
    var counterSpan = document.getElementById("totalUsersCounter");
    counterSpan.innerText = value.toString();
});

function NewViewLoaded()
{
    connection.send("NewWindowLoaded");
}

connection.start().then(fulfilled, rejected);


//start connection
function fulfilled() {
    //do something on start
    console.log("Connection to User Hub was Successful");
    NewViewLoaded();
}
function rejected() {
    console.log("Connection to User Hub Rejected");
}