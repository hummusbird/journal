const url = window.location.host

async function GetJournal() {
    let res = await fetch("/api/journal/today")
    let text = await res.text();
    console.log(text)
}