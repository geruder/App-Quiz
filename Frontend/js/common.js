//global variables
const STORAGE_KEY_USER = 'vocab_user';
const STORAGE_KEY_THEME = 'vocab_theme';

let currentUser = null;


// aninda execute edilecekler
(function init() {
    checkAuth(); // auth kontrol
    loadSavedTheme(); // son temayi load
    createMascot(); //nyancat ve robot icin
})();


// auth logic
function checkAuth() {
    if (window.location.pathname.includes('login.html')) return; // logindeysek kill

    const storedData = localStorage.getItem(STORAGE_KEY_USER);
    
    if (!storedData) { // user data yoksa logine
        window.location.href = 'login.html';
    } else {
        currentUser = JSON.parse(storedData); // user data varsa parse et
    }
}

// logout logic
function logout() { 
    localStorage.removeItem(STORAGE_KEY_USER);
    window.location.href = 'login.html';
}



// theme logic
function loadSavedTheme() {
    const savedTheme = localStorage.getItem(STORAGE_KEY_THEME) || '98'; // localde tercih yoksa default 98
    setTheme(savedTheme);
}

// theme logic2
function setTheme(themeName) {
    localStorage.setItem(STORAGE_KEY_THEME, themeName); // tercihi kaydet

    const linkTag = document.getElementById('theme-link');
    if (linkTag) { // secilene gore css dosyasini degistir
        linkTag.href = `css/theme-${themeName}.css`;
    }
}

function  createMascot(){ // bouncing animation icin div ve maskot olsutur her sayfaya
    if (document.getElementById('app-mascot')) return;

    const mascot = document.createElement('div');
    mascot.id = 'app-mascot';
    mascot.className = 'bouncing-mascot';

    document.body.appendChild(mascot);
}