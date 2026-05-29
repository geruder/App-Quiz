// dom yukle
const tableBody = document.getElementById('word-list-body');
const modal = document.getElementById('word-modal');


// formlar
const inpId = document.getElementById('edit-word-id');
const inpEng = document.getElementById('input-english');
const inpTr = document.getElementById('input-turkish');
const inpCefr = document.getElementById('input-cefr');

let globalWordList = []; // edit kismi icin localde dursun diye




// initialize
document.addEventListener('DOMContentLoaded', () => {
    loadMyWords();
});



// logic kismi
async function loadMyWords() { // userid gore kelime yukle
    tableBody.innerHTML = '<tr><td colspan="4">Loading...</td></tr>'; // gecikme durumunda
    
    try {
        const words = await ApiService.getMyWords(currentUser.userId); // apiden userid ile request
        globalWordList = words; // data tut
        renderTable(words);
    } catch (err) {
        console.error(err);
        tableBody.innerHTML = '<tr><td colspan="4" style="color:red">Error loading words.</td></tr>'; // error
    }
}


function renderTable(words) { // tablo logic
    tableBody.innerHTML = "";

    if (!words || words.length === 0) { // bos ise
        tableBody.innerHTML = '<tr><td colspan="4" style="text-align:center">No words found. Add one!</td></tr>';
        return;
    }

    words.forEach(word => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td><strong>${word.englishWord}</strong></td>
            <td>${word.turkishMeaning}</td>
            <td>${word.cefrLevel}</td>
            <td>
                <button class="win-btn small" onclick="openEditMode(${word.wordId})">Edit</button>
                <button class="win-btn small" onclick="handleDelete(${word.wordId})" style="color:red">Del</button>
            </td>
        `;
        tableBody.appendChild(tr);
    });
}


function openModal() {  // modal logic
    inpId.value = ""; // reset
    inpEng.value = "";
    inpTr.value = "";
    inpCefr.value = "A1"; // default a1
    
    
    modal.classList.remove('hidden');// artik hidden degil
}

function closeModal() { // modal kapama logic
    modal.classList.add('hidden'); // artik hidden
}

function openEditMode(wordId) { // edit logic
    const word = globalWordList.find(w => w.wordId === wordId); // tutulan datadan cek, yoksa undefined
    if (!word) return; // yoksa don


    inpId.value = word.wordId;
    inpEng.value = word.englishWord;
    inpTr.value = word.turkishMeaning;
    inpCefr.value = word.cefrLevel;

    modal.classList.remove('hidden');
}


async function saveWord() { // save logic, create veya update
    const wordId = inpId.value;
    

    if (!inpEng.value || !inpTr.value) { // validasyon
        alert("Please fill in English and Turkish fields.");
        return;
    }

    const wordData = {
        requestingUserId: currentUser.userId,
        userId: currentUser.userId,
        categoryId: 1, // default kategori
        englishWord: inpEng.value,
        turkishMeaning: inpTr.value,
        cefrLevel: inpCefr.value
    };

    try {
        if (wordId) {
            await ApiService.updateWord(wordId, wordData);
        } else { // yoksa create, olmasi imkansiz ama belki
            await ApiService.createWord(wordData);
        }

        closeModal(); // kapa
        loadMyWords(); // f5
    } catch (err) {
        alert("Failed to save: " + err.message);
    }
}


async function handleDelete(wordId) { // silme logic
    if (!confirm("Delete this word?")) return;

    try {
        await ApiService.deleteWord(wordId, currentUser.userId);
        loadMyWords();
    } catch (err) {
        alert("Delete failed.");
    }
}