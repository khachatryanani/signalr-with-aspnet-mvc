var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/hubs/house").build();

connection.start().then(fulfilled, rejected);

function fulfilled() {
    console.log("Connection to User Hub was Successful");
}
function rejected() {
    console.log("Connection to User Hub Rejected");
}

let lbl_houseJoined = document.getElementById("lbl_houseJoined");

let btn_gryffindor = document.getElementById("btn_gryffindor");
let btn_slytherin = document.getElementById("btn_slytherin");
let btn_hufflepuff = document.getElementById("btn_hufflepuff");
let btn_ravenclaw = document.getElementById("btn_ravenclaw");

let btn_un_gryffindor = document.getElementById("btn_un_gryffindor");
let btn_un_slytherin = document.getElementById("btn_un_slytherin");
let btn_un_hufflepuff = document.getElementById("btn_un_hufflepuff");
let btn_un_ravenclaw = document.getElementById("btn_un_ravenclaw");

let trigger_gryffindor = document.getElementById("trigger_gryffindor");
let trigger_slytherin = document.getElementById("trigger_slytherin");
let trigger_hufflepuff = document.getElementById("trigger_hufflepuff");
let trigger_ravenclaw = document.getElementById("trigger_ravenclaw");


btn_slytherin.addEventListener("click", function (event)
{
    connection.send("JoinHouse", "Slytherin");
    event.preventDefault();
});

btn_gryffindor.addEventListener("click", function (event) {
    connection.send("JoinHouse", "Gryffindor");
    event.preventDefault();
});

btn_hufflepuff.addEventListener("click", function (event) {
    connection.send("JoinHouse", "Hufflepuff");
    event.preventDefault();
});

btn_ravenclaw.addEventListener("click", function (event) {
    connection.send("JoinHouse", "Ravenclaw");
    event.preventDefault();
});

btn_un_slytherin.addEventListener("click", function (event) {
    connection.send("LeaveHouse", "Slytherin");
    event.preventDefault();
});

btn_un_gryffindor.addEventListener("click", function (event) {
    connection.send("LeaveHouse", "Gryffindor");
    event.preventDefault();
});

btn_un_hufflepuff.addEventListener("click", function (event) {
    connection.send("LeaveHouse", "Hufflepuff");
    event.preventDefault();
});

btn_un_ravenclaw.addEventListener("click", function (event) {
    connection.send("LeaveHouse", "Ravenclaw");
    event.preventDefault();
});

trigger_slytherin.addEventListener("click", function (event) {
    connection.send("Notify", "Slytherin");
    event.preventDefault();
});

trigger_gryffindor.addEventListener("click", function (event) {
    connection.send("Notify", "Gryffindor");
    event.preventDefault();
});

trigger_hufflepuff.addEventListener("click", function (event) {
    connection.send("Notify", "Hufflepuff");
    event.preventDefault();
});

trigger_ravenclaw.addEventListener("click", function (event) {
    connection.send("Notify", "Ravenclaw");
    event.preventDefault();
});


connection.on("TriggerNotification", (house) => {
    console.log(`house ${house} is sending notifications`);
});

connection.on("SubscriptionStatus", (strGroupsJoined, houseName, hasSubscribed) => {
    lbl_houseJoined.innerText = strGroupsJoined;

    if (hasSubscribed) {
        //subscribe to

        switch (houseName) {
            case 'slytherin':
                btn_slytherin.style.display = "none";
                btn_un_slytherin.style.display = "";
                break;
            case 'gryffindor':
                btn_gryffindor.style.display = "none";
                btn_un_gryffindor.style.display = "";
                break;
            case 'hufflepuff':
                btn_hufflepuff.style.display = "none";
                btn_un_hufflepuff.style.display = "";
                break;
            case 'ravenclaw':
                btn_ravenclaw.style.display = "none";
                btn_un_ravenclaw.style.display = "";
                break;
            default:
                break;
        }
    }
    else {
        //unsubscribe
        switch (houseName) {
            case 'slytherin':
                btn_slytherin.style.display = "";
                btn_un_slytherin.style.display = "none";
                break;
            case 'gryffindor':
                btn_gryffindor.style.display = "";
                btn_un_gryffindor.style.display = "none";
                break;
            case 'hufflepuff':
                btn_hufflepuff.style.display = "";
                btn_un_hufflepuff.style.display = "none";
                break;
            case 'ravenclaw':
                btn_ravenclaw.style.display = "";
                btn_un_ravenclaw.style.display = "none";
                break;
            default:
                break;
        }
    }

})

connection.on("MemberAdded", (house) =>
    {
        console.log(`new memebr added to the ${house}`);
    });

connection.on("MemberRemoved", (house) =>
    {
        console.log(`memebr left the ${house}`);
    });
