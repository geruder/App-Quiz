const categoryListDiv = document.getElementById('category-list');
const wordListDiv = document.getElementById('word-list');
const categoryTitle = document.getElementById('selected-category-name');


document.addEventListener('DOMContentLoaded', () => {

    fetchAndRenderCategories();
});


async function fetchAndRenderCategories() {
    try {
        const categories = await ApiService.getCategories();
        categoryListDiv.innerHTML = "";

        if (!categories || categories.length === 0) {
            categoryListDiv.innerHTML = "<p>No categories found.</p>";
            return;
        }

        categories.forEach(cat => {
            const btn = document.createElement('button');
            btn.className = "win-btn";
            btn.style.width = "100%";
            btn.style.textAlign = "left";
            btn.style.marginBottom = "5px";
            btn.textContent = cat.categoryName;


            btn.addEventListener('click', () => {

                document.querySelectorAll('#category-list .win-btn').forEach(b => b.classList.remove('active'));
                btn.classList.add('active');


                fetchAndRenderWords(cat.categoryId, cat.categoryName);
            });

            categoryListDiv.appendChild(btn);
        });
    } catch (err) {
        console.error("Failed to load categories:", err);
        categoryListDiv.innerHTML = "<p style='color:red;'>Error loading categories.</p>";
    }
}


async function fetchAndRenderWords(categoryId, categoryName) {
    categoryTitle.textContent = `Words in: ${categoryName}`;
    wordListDiv.innerHTML = "<p style='padding:10px;'>Loading words...</p>";

    try {
        const words = await ApiService.getWordsByCategory(categoryId);
        wordListDiv.innerHTML = "";

        if (!words || words.length === 0) {
            wordListDiv.innerHTML = "<p style='padding:10px;'>This category is currently empty.</p>";
            return;
        }


        words.forEach(word => {
            const item = document.createElement('div');
            item.style.padding = "8px";
            item.style.borderBottom = "1px dotted #ccc";
            item.style.display = "flex";
            item.style.justifyContent = "space-between";

            item.innerHTML = `
                <span><strong>${word.englishWord}</strong></span>
                <span style="color: #666;">${word.turkishMeaning}</span>
            `;
            wordListDiv.appendChild(item);
        });
    } catch (err) {
        console.error("Failed to load words:", err);
        wordListDiv.innerHTML = "<p style='padding:10px; color:red;'>Error fetching words for this category.</p>";
    }
}