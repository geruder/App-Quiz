// gerekli dom elementleri
const questionEl = document.querySelector('.question-word');
const cefrLevel = document.querySelector('#cefr');
const optionsGrid = document.querySelector('.option-grid');
const idkBtn = document.querySelector('.idk-btn');

// durumlar
let isProcessing = false;
let currentWordData = null;


// initialize
document.addEventListener('DOMContentLoaded', () => {
    // common.js den sonra bunlar execute
    loadQuestion();
    // kopya cekmemek icin history yok

    idkBtn.addEventListener('click', () => handleAnswer(null, null));
});



// logic kismi
async function loadQuestion() { // review icin fetch ediyoruz
    isProcessing = false;
    optionsGrid.innerHTML = '<p>Loading...</p>'; // eger gecikme olursa
    questionEl.textContent = "...";
    idkBtn.disabled = false;

    try {
        const data = await ApiService.getReviewWord(currentUser.userId); // apiden review word kismina request

        if (!data || data.finished || data.empty) {
            showFinished();
            return;
        }

        currentWordData = data;
        renderButtons(data);
    } catch (err) {
        console.error(err);
        optionsGrid.innerHTML = "<p>Error loading data.</p>";
    }
}

function renderButtons(data) { // buton render
    questionEl.textContent = data.englishWord;
    cefrLevel.textContent = data.cefrLevel;
    optionsGrid.innerHTML = "";

    data.options.forEach(opt => {
        const btn = document.createElement('button');
        btn.className = "win-btn option-btn";
        btn.textContent = opt;
        btn.addEventListener('click', () => handleAnswer(opt, btn));
        optionsGrid.appendChild(btn);
    });
}


async function handleAnswer(selectedOption, btnElement) {
    if (isProcessing) return;
    isProcessing = true;
    idkBtn.disabled = true;

    const userAns = selectedOption ? selectedOption.trim() : null;
    const correctAns = currentWordData.correctMeaning ? currentWordData.correctMeaning.trim() : null;
    const isCorrect = Boolean(userAns && (userAns === correctAns));

    if (btnElement) {
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
    } else {
        highlightCorrect(currentWordData.correctMeaning);
    }

    await ApiService.submitAnswer(currentUser.userId, currentWordData.wordId, isCorrect);


    setTimeout(loadQuestion, 1500); // siradaki question, history cagirma yok
}

function highlightCorrect(text) { // dogruyu yesil
    const buttons = optionsGrid.querySelectorAll('button');
    buttons.forEach(btn => {
        if (btn.textContent === text) {
            btn.style.backgroundColor = "#4CAF50";
            btn.style.color = "white";
        }
    });
}

function showFinished() { // review biterse
    questionEl.textContent = "Great Job!";
    optionsGrid.innerHTML = "<p>No words pending review.</p>";
    idkBtn.disabled = true;
}