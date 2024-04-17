const url = window.location.host
const delay = ms => new Promise(res => setTimeout(res, ms));

async function GetJournal() {
    let res = await fetch("/api/journal/today")
    let text = await res.text();
    document.getElementById("textzone").value = text;
    console.log(text)
    document.getElementById("textzone").scrollTop = document.getElementById("textzone").scrollHeight;
}

function SendEntry() {
    let formData = new FormData();
    formData.append("data", document.getElementById("entrybox").value)

    fetch("/api/journal/new",
        {
            body: formData,
            method: "POST"
        })
        .catch(e => console.log(e))
    delay(150)
    document.getElementById("textzone").scrollTop = document.getElementById("textzone").scrollHeight;
    GetJournal();
    document.getElementById("entrybox").value = "";
}