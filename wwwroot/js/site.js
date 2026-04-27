// локальная корзина
const Cart = {
    get() {
        return JSON.parse(localStorage.getItem('reso_cart') || '[]');
    },
    save(items) {
        localStorage.setItem('reso_cart', JSON.stringify(items));
        this.updateUI();
    },
    add(id, name, price) {
        const items = this.get();
        const existing = items.find(i => i.id === id);
        if (existing) {
            existing.qty += 1;
        } else {
            items.push({ id, name, price, qty: 1 });
        }
        this.save(items);
    },
    remove(id) {
        const items = this.get().filter(i => i.id !== id);
        this.save(items);
    },
    total() {
        return this.get().reduce((sum, i) => sum + i.price * i.qty, 0);
    },
    count() {
        return this.get().reduce((sum, i) => sum + i.qty, 0);
    },
    updateUI() {
        const count = this.count();
        const badge = document.getElementById('nav-cart-badge');
        const navCount = document.getElementById('nav-cart-count');
        if (badge) badge.textContent = count;
        if (navCount) navCount.textContent = count;
    }
};

document.addEventListener('DOMContentLoaded', () => {
    Cart.updateUI();
});
