PROJECT: Vocab Quiz - 3rd Grade Project


------------------------------------------------------------
PREREQUISITES:
------------------------------------------------------------
1. Visual Studio 2022 (with ASP.NET & Web Development workload).
2. XAMPP (or any MySQL Server) running on Port 3306.
3. A modern Web Browser.
------------------------------------------------------------



STEP 1: DATABASE SETUP
------------------------------------------------------------
1. Start Apache and MySQL in XAMPP.
2. Open your browser and go to -> http://localhost/phpmyadmin/
3. Click on the "Import" tab in the top menu.
4. Click "Choose File" and select the file: "vocab_uni_db.sql" (which i included in the sourcefiles).
5. Click "Import" / "Go" at the bottom.
------------------------------------------------------------



STEP 2: BACKEND SETUP (.NET API)
------------------------------------------------------------
1. Go to the "Backend" folder.
2. Double-click "VocabQuizAPI.sln" to open it in Visual Studio 2022.
3. Check the Database Connection:
   - Open "appsettings.json".
   - Make sure the connection string matches your local setup.
   - Default configured: "Server=localhost;Port=3306;Database=vocab_uni_db;Uid=root;Pwd=;"
   - If your root user has a password, please update 'Pwd='.
4. Press the Green Play Button (Start Debugging) in Visual Studio.
   - Keep this running in the background.
------------------------------------------------------------



STEP 3: FRONTEND SETUP (The App)
------------------------------------------------------------
1. Go to the "Frontend" folder.
2. Double-click "login.html" to open it in your web browser.
   (Or use VS Code - "Live Server").
------------------------------------------------------------


TEST CREDENTIALS (LOGIN)
------------------------------------------------------------
You can use these accounts:

User 1 (New User - Empty History):
Email: test@test.com
Password: 123

User 2 (Advanced User - Full History & Stats):
Email: student@uni.edu
Password: mypassword123  <-- (Use this to see the charts!)

------------------------------------------------------------



FEATURES TO REVIEW
------------------------------------------------------------
1. Quiz Mode: Spaced repetition logic.
2. Review Mode: Re-tests mistakes. History sidebar is hidden to prevent cheating.
3. Statistics: Chart.js visualization of daily progress (Check User 2).
4. Dashboard: Full CRUD operations for Custom Words.
------------------------------------------------------------
