async function runInjection() {
    const term = document.getElementById('search-term').value;
    const resultBox = document.getElementById('injection-result');

    if (!term) {
        resultBox.textContent = "Please enter a search term.";
        return;
    }

    resultBox.textContent = "Connecting to DB... Exploit in progress...";

    try {

        const data = await ApiService.unsafeSearch(term);
        

        resultBox.textContent = JSON.stringify(data, null, 2);
    } catch (error) {
        resultBox.textContent = "ERROR: DB Connection Severed or Script Error.\n" + error.message;
    }
}


async function submitBadForm() {
    const name = document.getElementById('contact-name').value;
    const msg = document.getElementById('contact-msg').value;
    const info = document.getElementById('contact-info').value;
    const statusBox = document.getElementById('contact-status');

    if (!name || !info) {
        statusBox.style.color = "red";
        statusBox.textContent = "Error: Name and Contact Info are required.";
        return;
    }

    try {

        await ApiService.sendContact(name, msg, info);
        
        statusBox.style.color = "green";
        statusBox.textContent = "✅ Row inserted! (Check phpMyAdmin for 1NF violation)";
        

        document.getElementById('contact-name').value = "";
        document.getElementById('contact-msg').value = "";
        document.getElementById('contact-info').value = "";

    } catch (error) {
        statusBox.style.color = "red";
        statusBox.textContent = "❌ Insertion Failed: " + error.message;
    }
}