// dom yukleme
const historyList = document.getElementById('history-list');
let myChart = null;


//initialize
document.addEventListener('DOMContentLoaded', () => {
    // auth kismi common.js de
    loadStats();
});


//logic kisimlari

async function loadStats() { // stats yukleme
    try {
        const [monthlyStats, history] = await Promise.all // iki fetch bir promise 
            ([ApiService.getMonthlyStats(currentUser.userId),
            ApiService.getHistory(currentUser.userId)]);

        renderChart(monthlyStats);
        renderHistoryList(history);

    } catch (err) {
        console.error(err);
        historyList.innerHTML = "<li>Error loading data.</li>";
    }
}

function renderChart(statList) { // chart renderlama
    const ctx = document.getElementById('myChart');
    if (myChart) myChart.destroy();
    const labels = statList.map(s => new Date(s.dateRecorded).toLocaleDateString());
    const dataSeen = statList.map(s => s.totalSeen);
    const dataCorrect = statList.map(s => s.totalCorrect);
    const dataAccuracy = statList.map(s => {
        return s.totalSeen > 0 ? ((s.totalCorrect / s.totalSeen) * 100).toFixed(1) : 0;
    });

    myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Accuracy (%)',
                    data: dataAccuracy,
                    type: 'line',
                    borderColor: '#FFD700',
                    backgroundColor: '#FFD700',
                    borderWidth: 3,
                    yAxisID: 'y1',
                    tension: 0.3
                },
                {
                    label: 'Correct',
                    data: dataCorrect,
                    backgroundColor: '#4CAF50',
                    yAxisID: 'y'
                },
                {
                    label: 'Wrong',
                    data: statList.map(s => s.totalWrong),
                    backgroundColor: '#F44336',
                    yAxisID: 'y'
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            interaction: { mode: 'index', intersect: false },
            scales: {
                y: { 
                    type: 'linear', position: 'left', beginAtZero: true,
                    title: { display: true, text: 'Words' } 
                },
                y1: { 
                    type: 'linear', position: 'right', min: 0, max: 100,
                    grid: { drawOnChartArea: false },
                    title: { display: true, text: '%' }
                }
            }
        }
    });
}


function renderHistoryList(history) { // quizdeki history mantigi, userid ye gore history
    historyList.innerHTML = "";

    if (!history || history.length === 0) { // data yoksa
        historyList.innerHTML = "<li>No recent history.</li>";
        return;
    }

    history.forEach(item => {
        const li = document.createElement('li');
        
        const status = item.isCorrect  // dogruluga gore yesil tik veya kirmizi carpi
            ? '<span style="color:green">✔</span>' 
            : '<span style="color:red">✘</span>';
        
        li.style.padding = "5px";
        li.style.borderBottom = "1px dotted #ccc";
        
        
        li.innerHTML = `
            <strong>${item.englishWord}</strong>: ${status}
            <span style="float:right; font-size:0.8em; color:#666;">
                ${new Date(item.attemptAt).toLocaleTimeString()}
            </span>
        `;
        
        historyList.appendChild(li);
    });
}