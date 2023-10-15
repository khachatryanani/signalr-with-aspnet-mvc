var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/hubs/voting").build();

var cloakSpan = document.getElementById("cloakCounter");
var wandSpan = document.getElementById("wandCounter");
var stoneSpan = document.getElementById("stoneCounter");

connection.on("UpdateVotingResults", (cloak, stone, wand) => {
    cloakSpan.innerText = cloak.toString();
    wandSpan.innerText = wand.toString();
    stoneSpan.innerText = stone.toString();
});

connection.start().then(fulfilled, rejected);


//start connection
function fulfilled() {
    //do something on start
    connection.invoke("GetVotingResults").then(results =>
    {
        cloakSpan.innerText = results.cloak.toString();
        wandSpan.innerText = results.wand.toString();
        stoneSpan.innerText = results.stone.toString();
    });
    console.log("Connection to User Hub was Successful");
}
function rejected() {
    console.log("Connection to User Hub Rejected");
}