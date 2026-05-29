const API_BASE_URL = "https://localhost:7149/api"; 

const ApiService = {

    // --- auth kisimlari ---
    async register(username, email, password) { // kayit
        return post(`${API_BASE_URL}/auth/register`, { username, email, password });
    },
    async login(email, password) { // giris
        return post(`${API_BASE_URL}/auth/login`, { email, password });
    },



    // --- quiz kisimlari ---
    async getNewWord(userId) {  // quiz kelime
        return get(`${API_BASE_URL}/word/quiz/${userId}`);
    },
    async getReviewWord(userId) {  // review kelime
        return get(`${API_BASE_URL}/word/review/${userId}`);
    },
    async submitAnswer(userId, wordId, isCorrect) {  // cevap
        return post(`${API_BASE_URL}/word/answer`, { userId, wordId, isCorrect });
    },



    // --- history ve stats kisimlari ---
    async getMonthlyStats(userId) { // aylik istatistik
        return get(`${API_BASE_URL}/word/stats/monthly/${userId}`);
    },
    async getHistory(userId) { // history al
        return get(`${API_BASE_URL}/word/history/${userId}`);
    },



    // --- kategori kismi ---
    async getCategories() { // kategorileri getir
        return get(`${API_BASE_URL}/category`);
    },
    async getWordsByCategory(categoryId) { // kategoriye gore kelimeleri getir
        return get(`${API_BASE_URL}/category/${categoryId}/words`);
    },



    // --- crud kisimlari ---
    async getMyWords(userId) {  // kullaniciya gore kelimeleri cek
        return get(`${API_BASE_URL}/word/my-words/${userId}`);
    },
    async createWord(wordData) { // yeni kelime ekle
        return post(`${API_BASE_URL}/word/create`, wordData);
    },
    async updateWord(wordId, updateData) { // kelime guncelle, kullanici dogruysa
        return put(`${API_BASE_URL}/word/update/${wordId}`, updateData);
    },
    async deleteWord(wordId, userId) { // kelime sil, kullanici dogruysa
        const response = await fetch(`${API_BASE_URL}/word/delete/${wordId}?requestingUserId=${userId}`, { method: "DELETE" });
        return handleResponse(response);
    },


    
    // --- diger kisimlar, fusun hoca icin demo ---
    async sendContact(name, message, contactInfo) {
        return post(`${API_BASE_URL}/contact`, { userName: name, message, contactInfoCombined: contactInfo });
    },
    async unsafeSearch(term) {
        return get(`${API_BASE_URL}/word/unsafe-search?term=${term}`);
    }
};

// --- yardimci fonksiyonlar ---
// kod tekrari olmamasi icin istekleri burada yapiyoruz
async function get(url) {  // get request
    try {
        const response = await fetch(url);
        return handleResponse(response);
    } catch (err) {
        console.error("GET Error:", err);
        return null;
    }
}

async function post(url, data) { // post request
    try {
        const response = await fetch(url, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        });
        return handleResponse(response);
    } catch (err) {
        console.error("POST Error:", err);
        throw err;
    }
}

async function put(url, data) { // put request
    try {
        const response = await fetch(url, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        });
        return handleResponse(response);
    } catch (err) {
        console.error("PUT Error:", err);
        throw err;
    }
}

async function handleResponse(response) {  // response kontrol
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || `API Error: ${response.status}`);
    }
    return await response.json();
}