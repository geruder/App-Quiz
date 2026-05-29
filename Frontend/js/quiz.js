// gerekli dom elementleri
const questionElement = document.querySelector('.question-word');
const cefrLevel = document.querySelector('#cefr');
const optionsGrid = document.querySelector('.option-grid');
const idkBtn = document.querySelector('.idk-btn');
const historyList = document.getElementById('history-list-sidebar');

// durumlar
let isProcessing = false;
let currentWordData = null;


// initialize
document.addEventListener('DOMContentLoaded', () => {
    // common.js den sonra bunlar execute
    loadQuestion();
    loadHistory();
    
    idkBtn.addEventListener('click', () => handleAnswer(null, null));
});


// logic kismi

async function loadQuestion() { // soru cekme
    isProcessing = false;
    optionsGrid.innerHTML = '<p>Loading...</p>'; // olasi gecikme icin
    questionElement.textContent = "...";
    cefrLevel.textContent = "?"
    idkBtn.disabled = false;

    try {
        const data = await ApiService.getNewWord(currentUser.userId);

        if (!data || data.finished || data.empty) { // hata veya quiz bitmis
            showFinished();
            return;
        }

        currentWordData = data; // hatasizsa set et renderla
        renderButtons(data);
    } catch (err) {
        console.error(err);
        optionsGrid.innerHTML = "<p>Error loading data.</p>";
    }
}

function renderButtons(data) { // apiden gelen dataya gore butonlari renderla
    questionElement.textContent = data.englishWord;
    cefrLevel.textContent = data.cefrLevel;
    optionsGrid.innerHTML = "";

    data.options.forEach(option => {
        const btn = document.createElement('button');
        btn.className = "win-btn option-btn";
        btn.textContent = option;
        btn.addEventListener('click', () => handleAnswer(option, btn));
        optionsGrid.appendChild(btn);
    });
}

async function handleAnswer(selectedOption, btnElement) { // cevabi request et
    if (isProcessing) return;
    isProcessing = true;
    idkBtn.disabled = true;

    
    const userAns = selectedOption ? selectedOption.trim() : null;
    const correctAns = currentWordData.correctMeaning ? currentWordData.correctMeaning.trim() : null;
    
    const isCorrect = Boolean(userAns && (userAns === correctAns));

    
    if (btnElement) { // dogru/yanlisa gore renkler
        if (isCorrect) {
            btnElement.style.backgroundColor = "#4CAF50";
            btnElement.style.color = "white";
            btnElement.style.transition = "transform 0.3s ease-in-out";
            btnElement.style.transform = "scale(1.1)";
        } else {
            btnElement.style.backgroundColor = "#F44336";
            btnElement.style.color = "white";
            btnElement.style.transition = "transform 0.3s ease-in-out";
            btnElement.style.transform = "scale(0.9)";
            highlightCorrect(currentWordData.correctMeaning);
        }
    } else { // idk butonu
        highlightCorrect(currentWordData.correctMeaning);
    }

    // apiye request, biz zaten isledik simdi dbye gitmesi lazim api uzerinden
    await ApiService.submitAnswer(currentUser.userId, currentWordData.wordId, isCorrect);

    // api dondukten sonra history guncelle
    loadHistory();

    // 1.5 saniye bekle sonra yeniden
    setTimeout(loadQuestion, 1500);
}


function highlightCorrect(text) { // dogru cevap yesil
    const buttons = optionsGrid.querySelectorAll('button');
    buttons.forEach(btn => {
        if (btn.textContent === text) {
            btn.style.backgroundColor = "#4CAF50";
            btn.style.color = "white";
            btn.style.transition = "transform 0.3s ease-in-out";
            btn.style.transform = "scale(1.1)";
        }
    });
}

async function loadHistory() { // userid history cek
    const history = await ApiService.getHistory(currentUser.userId); // fetch history
    historyList.innerHTML = ""; 

    if (!history || history.length === 0) {
        historyList.innerHTML = "<li>No history yet.</li>";
        return;
    }

    history.forEach(item => {
        const li = document.createElement('li');
        const cssClass = item.isCorrect ? 'correct-history' : 'wrong-history';
        li.className = `history-item ${cssClass}`;
        
        // history doldurma
        li.innerHTML = `
            <strong>${item.englishWord}</strong><br>
            <small>${item.turkishMeaning}</small>
        `;
        historyList.appendChild(li);
    });
}

function showFinished() { // quiz biterse
    questionElement.textContent = "All Done!";
    optionsGrid.innerHTML = "<p>You have seen all new words!</p>";
    idkBtn.disabled = true;
}