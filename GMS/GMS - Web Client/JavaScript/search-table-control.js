function getEvents(eventTypes, keywords) {
    var events = document.getElementById("events-table");
    var guildID = "116E0C0E-0035-44A9-BB22-4AE3E23127E5" // TODO this is to be fetched automatically

    $.ajax({
        type: "GET",
        url: "https://localhost:44377/api/guild/" + guildID,
        data: {
            
        },
        dataType: "json",
        success: function (json) {
            clearTable();
            var trHTML = "";
            json.forEach(obj => {
                if ((eventTypes.includes(obj.eventType)) || (eventTypes.length === 0)) {
                    if (keywords.some(v => obj.name.toLowerCase().includes(v)) || keywords.some(v => obj.description.toLowerCase().includes(v)) || (keywords.length === 0)) {
                        trHTML += "<tr>";
                        const keys = Object.keys(obj);
                        console.log(keys);
                        Object.entries(obj).forEach(([key, value]) => {
                            if (key === "participants" || key === "waitingList" || key === "guildID" || key === "rowId") {
                            } else if (key === "eventID") {
                                trHTML += "<th scope=\"row\">" + value + "</th>"
                            } else if (key === "date") {
                                let date = new Date(value + "Z");
                                trHTML += "<td>" + date.toUTCString() + "</td>";
                            } else {
                                trHTML += "<td>" + value + "</td>";
                            }
                            if (Object.is(keys.length - 1, keys.indexOf(key))) {
                                trHTML += "<td>";
                                trHTML += "<button type=\"button\" class=\"btn btn-success btn-sm\"><i class=\"fa fa-sign-in\" aria-hidden=\"true\"></i></button> ";
                                trHTML += "<button type=\"button\" class=\"btn btn-warning btn-sm\"><i class=\"fa fa-pencil\" aria-hidden=\"true\"></i></button> ";
                                trHTML += "<button type=\"button\" class=\"btn btn-danger btn-sm\"><i class=\"fa fa-trash-o\" aria-hidden=\"true\"></i></button>";
                                trHTML += "</td>";
                            }
                        });
                    }
                }
            });
            trHTML += "</tr>";
            $('#events-table').append(trHTML);
        },
        error: function () {
            alert("Failed to get events"); // TODO might want to change this
        }
    })
}

function clearTable() {
    var events = document.getElementById("events-table");
    events.innerHTML = "<tr><th scope=\"col\">ID</th><th scope=\"col\">Name</th><th scope=\"col\">Event type</th><th scope=\"col\">Location</th><th scope=\"col\">Date</th><th scope=\"col\">Description</th><th scope=\"col\">Max. num. of character</th><th scope=\"col\" style=\"width: 12%;\">Actions</th></tr>"
}