var i = 0;

function loadCharacters(apiToken) {
    $.ajax({
        type: "GET",
        url: "https://localhost:44377/gw2api/characters",
        headers: { 'Authorization': apiToken },
        data: {

        },
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                loadEmptyCharacter(data[i], i);
            }
            document.getElementsByClassName('progress-bar').item(0).setAttribute('aria-valuenow', 15);
            document.getElementsByClassName('progress-bar').item(0).setAttribute('style', 'width:' + 15 + '%');
            loadCharactersCore(data, apiToken);
        },
        error: function (error) {
            //console.log(character);
        },
    })
};

function loadEmptyCharacter(character, number) {
    var character = '<a href="/User/CharacterPage?name=' + character.replace(/ /g, '%20') + '" class="list-group-item list-group-item-action flex-column align-items-start" >' +
                    '<div class="d-flex w-100 justify-content-between" >' +
                    '<h5 class="mb-1">' + character + '</h5>' +
                    '<small id="' + number + '-Level">' +
                    '   <div class="spinner-border spinner-border-sm text-success" role="status">' +
                    '       <span class="sr-only">Loading...</span>' +
                    '   </div>' +
                    '</small>' +
                    '</div>' +
                    '<div id="' + number + '-Race" class="mb-1">' +
                    '   <div class="spinner-border spinner-border-sm text-success" role="status">' +
                    '       <span class="sr-only">Loading...</span>' +
                    '   </div>' +
                    '</div>' +
                    '<div id="' + number + '-Profession" class="mb-1">' +
                    '   <div class="spinner-border spinner-border-sm text-success" role="status">' +
                    '       <span class="sr-only">Loading...</span>' +
                    '   </div>' +
                    '</div>' +
                    '<small id="' + number + '-Gender">' +
                    '   <div class="spinner-border spinner-border-sm text-success" role="status">' +
                    '       <span class="sr-only">Loading...</span>' +
                    '   </div>' +
                    '</small>' +
                    '</a >';
    $(characters).append(character);
}

function loadCharactersCore(characters, apiToken) {
    $.ajax({
        type: "GET",
        url: "https://localhost:44377/gw2api/characters/" + characters[i] + "/core",
        headers: { 'Authorization': apiToken },
        data: {

        },
        dataType: "json",
        success: function (data) {
            $("#" + i.toString() + "-Level").html(data.level);
            $("#" + i.toString() + "-Race").html(data.race);
            $("#" + i.toString() + "-Profession").html(data.profession);
            $("#" + i.toString() + "-Gender").html(data.gender);
            i++;
            if (i < characters.length) {
                loadCharactersCore(characters, apiToken);
                document.getElementsByClassName('progress-bar').item(0).setAttribute('aria-valuenow', (i / characters.length)*100);
                document.getElementsByClassName('progress-bar').item(0).setAttribute('style', 'width:' + (i / characters.length) * 100 + '%');
            } else {
                $('.toast').toast('hide');
                document.getElementsByClassName('progress-bar').item(0).setAttribute('aria-valuenow', 100);
                document.getElementsByClassName('progress-bar').item(0).setAttribute('style', 'width:' + 100 + '%');
                document.getElementsByClassName('progress-bar').item(0).classList.remove("progress-bar-striped");
                document.getElementsByClassName('progress-bar').item(0).classList.remove("progress-bar-animated");
                document.getElementsByClassName('progress-bar').item(0).appendChild(document.createTextNode("Success"));
            }
        },
        error: function (error) {
            //console.log(character);
        },
    })
};

function loadEvents(apiToken, userToken) {
    $.ajax({
        type: "GET",
        url: "https://localhost:44377/gw2api/account",
        headers: { 'Authorization': apiToken },
        data: {

        },
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.guilds.length; i++) {
                loadGuildEvents(data.guilds[i], userToken);
            }
        },
        error: function (error) {
            //console.log(data);
        },
    })
};

function loadGuildEvents(guildId, userToken) {
    $.ajax({
        type: "GET",
        url: "https://localhost:44377/api/guild/" + guildId,
        headers: { 'Authorization': userToken },
        data: {

        },
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                loadGuildEvent(data[i]);
            }
        },
        error: function (error) {
            return "";
        },
    })
};

function loadGuildEvent(event) {
    let date = new Date(event.date + "Z");
    var event = '<tr>' +
        '<th scope=\"row\">' + event.eventID + '</th>' +
        '<td>' + event.name + '</td>' +
        '<td>' + event.eventType + '</td>' +
        '<td>' + event.location + '</td>' +
        '<td>' + date.toUTCString() + '</td>' +
        '</tr>';
    $(events).append(event);
}

function loadCharacter(character, apiToken) {
    $.ajax({
        type: "GET",
        url: "https://localhost:44377/gw2api/characters/" + character + "/core",
        headers: { 'Authorization': apiToken },
        data: {

        },
        dataType: "json",
        success: function (data) {
            $("#level").html(data.level);
            $("#race").html(data.race);
            $("#profession").html(data.profession);
            $("#gender").html(data.gender);
        },
        error: function (error) {
            //console.log(character);
        },
    })
};

function loadEquipmentList(character, apiToken) {
    $.ajax({
        type: "GET",
        url: "https://localhost:44377/gw2api/characters/" + character + "/equipment",
        headers: { 'Authorization': apiToken },
        data: {

        },
        dataType: "json",
        success: function (data) {
            var arrayLength = data.equipment.length;
            for (var i = 0; i < arrayLength; i++) {
                loadItem(data.equipment[i].id, apiToken)
            }
            $('.toast').toast('hide');
        },
        error: function (error) {
            //console.log(character);
        },
    })
};

function loadItem(id, apiToken) {
    $.ajax({
        type: "GET",
        url: "https://localhost:44377/gw2api/items/" + id,
        headers: { 'Authorization': apiToken },
        data: {

        },
        dataType: "json",
        success: function (data) {
            var items = '<a href="#" class="list-group-item list-group-item-action flex-column align-items-start">' +
                        '<div class="d-flex w-100 justify-content-between">' +
                        '<h4 class="mb-1">' + data.details.type + '</h4>' +
                        '<small id="' + data.details.type + 'Level">' + data.level + '</small>' +
                        '</div>' +
                        '<img id="' + data.details.type + 'Image" src="' + data.icon + '" class="rounded float-right" alt="Responsive image" />' +
                        '<h5 id="' + data.details.type + 'Name" class="mb-1">' + data.name + '</h5>' +
                        '<p id="' + data.details.type + 'Rarity" class="mb-1">' + data.rarity + '</p>' +
                        '<small id="' + data.details.type + 'Value">Value: ' + data.vendor_value + '</small>';
                        '</a>';
            $(equipment).append(items);
        },
        error: function (error) {
            //console.log(character);
        },
    })
};