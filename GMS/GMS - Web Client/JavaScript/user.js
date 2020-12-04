var i = 0;

function loadCharacters(characters, apiToken) {
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
                    loadCharacters(characters, apiToken);
                }
            },
            error: function (error) {
                console.log(character);
            },
        })
};

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
            console.log(character);
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
            console.log(data);
            var arrayLength = data.equipment.length;
            for (var i = 0; i < arrayLength; i++) {
                loadItem(data.equipment[i].id, apiToken)
            }
        },
        error: function (error) {
            console.log(character);
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
            console.log(data.icon);
            var items = "";
            items += "<a href=\"#\" class=\"list-group-item list-group-item-action flex-column align-items-start\">";
            items += "<div class=\"d-flex w-100 justify-content-between\">";
            items += "<h4 class=\"mb-1\">" + data.details.type + "</h4>";
            items += "<small id=\"" + data.details.type + "Level" + "\">" + data.level + "</small>";
            items += "</div>";
            items += "<img id=\"" + data.details.type + "Image" + "\" src=\"" + data.icon + "\" class=\"rounded float-right\" alt=\"Responsive image\" />";
            items += "<h5 id=\"" + data.details.type + "Name" + "\" class=\"mb-1\">" + data.name + "</h5>";
            items += "<p id=\"" + data.details.type + "Rarity" + "\" class=\"mb-1\">" + data.rarity + "</p>";
            items += "<small id=\"" + data.details.type + "Value" + "\" >Value: " + data.vendor_value + "</small>";
            items += "</a>";
            $(equipment).append(items);
        },
        error: function (error) {
            console.log(character);
        },
    })
};