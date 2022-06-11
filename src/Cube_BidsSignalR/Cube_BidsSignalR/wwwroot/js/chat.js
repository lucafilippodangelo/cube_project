
    "use strict";

    var connection = new signalR.HubConnectionBuilder().withUrl("/anHub").build();

    //Disable send button until connection is established
    //document.getElementById("sendButton").disabled = true;

    connection.on("ReceiveMessage", function (user, message) {
        var newNode = document.createElement("li");
        const list = document.getElementById("messagesList");
        list.insertBefore(newNode, list.children[0]);
        //document.getElementById("messagesList").insertBefore(newnode, list.children[0]);

        // We can assign user-supplied strings to an element's textContent because it
        // is not interpreted as markup. If you're assigning in any other way, you 
        // should be aware of possible script injection concerns.
        newNode.textContent = `${user} says ${message}`;

        if (user == "1") {
            newNode.fontcolor("red");
        }

        
    });

    connection.start().then(function () {
        //document.getElementById("sendButton").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });

    //document.getElementById("sendButton").addEventListener("click", function (event) {
    //    var user = document.getElementById("userInput").value;
    //    var message = document.getElementById("messageInput").value;
    //    connection.invoke("SendMessage", user, message).catch(function (err) {
    //        return console.error(err.toString());
    //    });
    //    event.preventDefault();
    //});

