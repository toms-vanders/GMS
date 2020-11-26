function getAllTheEvents(guildID) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: "GET",
            url: "https://localhost:44377/api/guild/" + guildID,
            data: {

            },
            dataType: "json",
            success: function (data) {
                resolve(data);
            },
            error: function (error) {
                reject(error)
            },
        })
    })
}

function getAndDisplayEventsCaller(eventTypes, keywords, characterName, guildID) {
    getAllTheEvents(guildID)
        .then((data) => {
            displayEventsTable(data, characterName, guildID, eventTypes, keywords)
        })
        .catch((error) => {
            alert("Failed to get any events.");
        })
}

function displayEventsTable(allEvents, characterName, guildID, eventTypes, keywords) {
    $.ajax({
        type: "GET",
        url: "https://localhost:44377/api/guild/" + guildID + "/character/" + characterName,
        data: {

        },
        dataType: "json",
        success: function (data) {
            // Data is only the events user participates in
            // Render table 

            // Extract id(s) of the event(s) user participates in
            var matchingIDs = [];
            for (i = 0; i < data.length; i++) {
                matchingIDs.push(data[i].eventID);
            }

            clearTable();
            var trHTML = "<tbody>";
            allEvents.forEach(obj => {
                if ((eventTypes.includes(obj.eventType)) || (eventTypes.length === 0)) {
                    if (keywords.some(v => obj.name.toLowerCase().includes(v)) || keywords.some(v => obj.description.toLowerCase().includes(v)) || (keywords.length === 0)) {
                        trHTML += "<tr>";
                        const keys = Object.keys(obj);

                        // Checking whether character participates in currently iterated event
                        var characterParticipatesInEvent = matchingIDs.includes(obj.eventID);

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
                                trHTML += "<button type=\"button\" class=\"btn btn-success btn-sm\" data-tooltip=\"tooltip\" data-toggle=\"modal\" data-placement=\"top\" data-target=\"#chooseRoleModal\" data-eventID=\"" + obj.eventID + "\" data-eName=\"" + obj.name + "\" title=\"Join event or waiting list\"><i class=\"fa fa-sign-in\" aria-hidden=\"true\"></i></button> ";
                                trHTML += "<button type=\"button\" onclick=\"location.href='https://localhost:44318/Event/UpdateEventForm?eventID="+obj.eventID+"'\" class=\"btn btn-warning btn-sm\" data-toggle=\"editEvent\" data-placement=\"top\" title=\"Edit event\"><i class=\"fa fa-pencil\" aria-hidden=\"true\"></i></button> ";
                                trHTML += "<button type=\"button\" class=\"btn btn-danger btn-sm\" data-tooltip=\"tooltip\" data-toggle=\"modal\" data-placement=\"top\" title=\"Remove event\" onclick=\"removeEventWrapper(" + obj.eventID + ") \"><i class=\"fa fa-trash-o\" aria-hidden=\"true\"></i></button>";
                                if (characterParticipatesInEvent) trHTML += " <button type=\"button\" class=\"btn btn-primary btn-sm\" data-tooltip=\"tooltip\" data-placement=\"top\" title=\"Cancel your participation in event\" onclick=\"cancelParticipationCaller(" + obj.eventID + ") \"><i class=\"fa fa-minus-square-o\" aria-hidden=\"true\"></i></button>";
                                trHTML += "</td>";
                            }
                        });
                        trHTML += "</tr>";
                    }
                }
            });
            trHTML += "</tbody>";
            $('#events-table').append(trHTML);
            $('[data-tooltip="tooltip"]').tooltip();
            $('[data-tooltip="tooltip"]').tooltip();
            $('[data-tooltip="tooltip"]').tooltip();

        },
        error: function () {
            alert("Errors connecting with the database. Events were not fetched.");
        },
    })
}

function joinEvent(eventID, characterName, characterRole, signUpDateTime) {
    var EventCharacter = {};
    EventCharacter.eventID = parseInt(eventID);
    EventCharacter.characterName = characterName;
    EventCharacter.characterRole = characterRole;
    EventCharacter.signUpDateTime = signUpDateTime;

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        url: 'https://localhost:44377/api/guild/events/join/',
        data: JSON.stringify(EventCharacter),
        dataType: 'json',
        success: function () {
            alert("You successfully joined the event.");
            $('#chooseRoleModal').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }, error: function () {
            alert("Error trying to join the event. You might be trying to join an event you're already a participant of.");
        }
    })
}

function cancelParticipation(eventID, characterName) {
    $.ajax({
        type: 'DELETE',
        contentType: 'application/json; charset=utf-8',
        url: 'https://localhost:44377/api/guild/events/' + eventID + '/withdraw/' + characterName,
        success: function () {
            alert('Event participation was cancelled');
        }, error: function () {
            alert('Error cancelling your event participation');
        }
    })
}

function removeEvent(eventID) {
    $.ajax({
        type: 'DELETE',
        contentType: 'application/json; charset=utf-8',
        url: 'https://localhost:44377/api/guild/events/remove/' + eventID,
        success: function () {
            alert("The event was removed.");
        }, error: function () {
            alert("An error occurred when trying to remove event.");
        }
    })
}

function clearTable() {
    var events = document.getElementById("events-table");
    events.innerHTML = "<thead><tr><th scope=\"col\">ID</th><th scope=\"col\">Name</th><th scope=\"col\">Event type</th><th scope=\"col\">Location</th><th scope=\"col\">Date</th><th scope=\"col\">Description</th><th scope=\"col\">Max. num. of character</th><th scope=\"col\" style=\"width: 14%;\">Actions</th></tr></thead>"
}
