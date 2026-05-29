const AppLogin = {
    
    // login register arasi toggle
    toggleMode() {
        const loginForm = document.getElementById('login-form');
        const regForm = document.getElementById('register-form');
        const errorBox = document.getElementById('error-msg');

        // toggle
        if (loginForm.style.display === 'none') { /* login none ise registerdayiz, logini getir */
            loginForm.style.display = 'block';
            regForm.style.display = 'none';
        } else {
            loginForm.style.display = 'none'; /* degilse registerdayiz, logini getir */
            regForm.style.display = 'block';
        }
        
        // error temizle
        errorBox.textContent = "";
    },

    // login logic
    async doLogin() {
        const email = document.getElementById('login-email').value;
        const password = document.getElementById('login-password').value;
        const errorBox = document.getElementById('error-msg');

        if (!email || !password) {
            errorBox.textContent = "Please fill in all fields.";
            return;
        }

        errorBox.textContent = "Logging in..."; // gecikme olmasi durumunda

        try {
            const data = await ApiService.login(email, password); // api request, kullanici yoksa null donecek
            
            localStorage.setItem(STORAGE_KEY_USER, JSON.stringify({ // userdata localstorage a kayit
                userId: data.userId,
                username: data.username
            }));

            window.location.href = "quiz.html"; // quiz sayfasina yonlendir

        } catch (err) {
            console.error(err);
            errorBox.textContent = "Login failed: Invalid email or password."; // error
        }
    },

    // register logic
    async doRegister() {
        const username = document.getElementById('reg-username').value;
        const email = document.getElementById('reg-email').value;
        const password = document.getElementById('reg-password').value;
        const errorBox = document.getElementById('error-msg');

        if (!username || !email || !password) { // field kontrol
            errorBox.textContent = "Please fill in all fields.";
            return;
        }


        if(!this.isValidEmail(email)){
            errorBox.textContent = "Please enter a valid email address."
            return;
        }


        errorBox.textContent = "Creating account..."; // gecikme olmasi durumunda

        try {
            const data = await ApiService.register(username, email, password); // api request
            
            localStorage.setItem(STORAGE_KEY_USER, JSON.stringify({
                userId: data.userId,
                username: data.username
            }));

            // quize yonlendir
            window.location.href = "quiz.html";

        } catch (err) {
            console.error(err);
            errorBox.textContent = "Registration failed. Email might be taken.";
        }
    },

    //helper method, email kontrol
    isValidEmail(email){
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }
};