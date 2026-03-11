CREATE DATABASE IF NOT EXISTS db102;
USE db102;
 
-- Основные таблицы (переименовываем patents в clients)
CREATE TABLE IF NOT EXISTS roles (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    INDEX (name)
);
 
CREATE TABLE IF NOT EXISTS categories (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    INDEX (name)
);
 
CREATE TABLE IF NOT EXISTS statuses (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    INDEX (name)
);
 
CREATE TABLE IF NOT EXISTS products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    category_id INT,
    INDEX (category_id),
    FOREIGN KEY (category_id) REFERENCES categories(id)
);
 
CREATE TABLE IF NOT EXISTS clients (
    id INT AUTO_INCREMENT PRIMARY KEY,
    phone VARCHAR(255) NOT NULL,
    fio VARCHAR(100) NOT NULL,
    INDEX (phone)
);
 
CREATE TABLE IF NOT EXISTS orders (
    id INT AUTO_INCREMENT PRIMARY KEY,
    status_id INT,
    client_id INT,
    date_of_creation DATE,
    date_of_completion DATE,
    product_id INT,
    discount VARCHAR(45),
    INDEX (status_id),
    INDEX (client_id),
    INDEX (product_id),
    FOREIGN KEY (status_id) REFERENCES statuses(id),
    FOREIGN KEY (client_id) REFERENCES clients(id),
    FOREIGN KEY (product_id) REFERENCES products(id)
);
 
CREATE TABLE IF NOT EXISTS users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    login VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role_id INT,
    order_id INT,
    fio VARCHAR(45),
    INDEX (role_id),
    INDEX (order_id),
    FOREIGN KEY (role_id) REFERENCES roles(id),
    FOREIGN KEY (order_id) REFERENCES orders(id)
);
 
CREATE TABLE IF NOT EXISTS reports (
    id VARCHAR(100) PRIMARY KEY,
    param VARCHAR(45),
    user_id INT,
    INDEX (user_id),
    FOREIGN KEY (user_id) REFERENCES users(id)
);
 
-- 1. Сначала справочники
INSERT IGNORE INTO roles (name) VALUES
('Системный администратор'),
('Менеджер'),
('Продавец-консультант');
 
INSERT IGNORE INTO categories (name) VALUES
('Латексные шары'),
('Фольгированные шары'),
('Гелиевые шары'),
('Тематические наборы'),
('Аксессуары'),
('Свадебные шары'),
('Детские шары'),
('Новогодние шары');
 
INSERT IGNORE INTO statuses (name) VALUES
('Новый'),
('Подтвержден'),
('В обработке'),
('Готов к выдаче'),
('Выполнен'),
('Отменен');
 
-- 2. Затем клиенты (52 записи)
INSERT IGNORE INTO clients (phone, fio) VALUES
('+79123456789', 'Иванов Алексей Петрович'),
('+79234567890', 'Петрова Мария Сергеевна'),
('+79345678901', 'Сидоров Дмитрий Владимирович'),
('+79456789012', 'Козлова Анна Игоревна'),
('+79567890123', 'Николаев Павел Александрович'),
('+79678901234', 'Федорова Екатерина Викторовна'),
('+79789012345', 'Морозов Сергей Николаевич'),
('+79890123456', 'Волкова Ольга Дмитриевна'),
('+79901234567', 'Алексеев Иван Петрович'),
('+79012345678', 'Семенова Татьяна Алексеевна'),
('+79111223344', 'Кузнецов Андрей Сергеевич'),
('+79222334455', 'Павлова Людмила Ивановна'),
('+79333445566', 'Лебедев Михаил Викторович'),
('+79444556677', 'Соколова Надежда Павловна'),
('+79555667788', 'Новиков Александр Дмитриевич'),
('+79666778899', 'Фролов Артем Сергеевич'),
('+79777889900', 'Захарова Виктория Андреевна'),
('+79888990011', 'Борисов Максим Ильич'),
('+79999001122', 'Киселева Анастасия Павловна'),
('+79000112233', 'Григорьев Денис Олегович'),
('+79112233445', 'Титова Юлия Валерьевна'),
('+79223344556', 'Комарова Ирина Сергеевна'),
('+79334455667', 'Орлов Станислав Викторович'),
('+79445566778', 'Егорова Марина Дмитриевна'),
('+79556677889', 'Щербаков Роман Алексеевич'),
('+79667788990', 'Белова Ангелина Игоревна'),
('+79778899001', 'Медведев Глеб Петрович'),
('+79889900112', 'Сазонова Кристина Олеговна'),
('+79990011223', 'Данилов Артур Владимирович'),
('+79001122334', 'Гусева Валерия Александровна'),
('+79112345670', 'Кудрявцев Евгений Сергеевич'),
('+79223456781', 'Ларина Ольга Викторовна'),
('+79334567892', 'Субботин Алексей Дмитриевич'),
('+79445678903', 'Воронова Дарья Ильинична'),
('+79556789014', 'Жуков Игорь Анатольевич'),
('+79667890125', 'Цветкова Елена Павловна'),
('+79778901236', 'Филиппов Антон Михайлович'),
('+79889012347', 'Дорофеева Светлана Васильевна'),
('+79990123458', 'Марков Константин Игоревич'),
('+79001234569', 'Савельева Тамара Федоровна'),
('+79113456789', 'Блинов Василий Степанович'),
('+79224567890', 'Калашникова Инна Романовна'),
('+79335678901', 'Широков Петр Николаевич'),
('+79446789012', 'Зайцева Вероника Андреевна'),
('+79557890123', 'Маслов Виталий Олегович'),
('+79668901234', 'Исаева Алла Борисовна'),
('+79779012345', 'Горбунов Никита Артемович'),
('+79880123456', 'Пономарева Лидия Семеновна'),
('+79991234567', 'Лыков Дмитрий Валентинович'),
('+79002345678', 'Носкова Элина Рудольфовна'),
('+79114567890', 'Симонов Аркадий Георгиевич'),
('+79225678901', 'Крылова Жанна Леонидовна');
 
-- 3. Затем продукты (54 записи)
INSERT IGNORE INTO products (name, category_id) VALUES
('Шар латексный красный 12"', 1),
('Шар латексный синий 12"', 1),
('Шар латексный зеленый 12"', 1),
('Шар латексный желтый 12"', 1),
('Шар латексный белый 12"', 1),
('Шар латексный черный 12"', 1),
('Шар латексный розовый 12"', 1),
('Шар латексный фиолетовый 12"', 1),
('Шар латексный оранжевый 12"', 1),
('Шар латексный золотой 12"', 1),
('Шар латексный серебряный 12"', 1),
('Шар фольгированный "Сердце" красное', 2),
('Шар фольгированный "Сердце" золотое', 2),
('Шар фольгированный "Звезда" золотая', 2),
('Шар фольгированный "Звезда" серебряная', 2),
('Шар фольгированный "Круг" золотой', 2),
('Шар фольгированный "Круг" серебряный', 2),
('Шар фольгированный "Цифра 1"', 2),
('Шар фольгированный "Цифра 2"', 2),
('Шар фольгированный "Цифра 3"', 2),
('Шар фольгированный "Цифра 4"', 2),
('Шар фольгированный "Цифра 5"', 2),
('Шар фольгированный "Цифра 0"', 2),
('Шар фольгированный "Микки Маус"', 2),
('Шар фольгированный "Единорог"', 2),
('Гелиевый шар "С Днем Рождения"', 3),
('Гелиевый шар "С 8 Марта"', 3),
('Гелиевый шар "С 23 Февраля"', 3),
('Гелиевый шар "С Новым Годом"', 3),
('Гелиевый шар "Любовь"', 3),
('Набор "День Рождения" детский', 4),
('Набор "День Рождения" взрослый', 4),
('Набор "Свадьба" классический', 4),
('Набор "Свадьба" премиум', 4),
('Набор "Новый Год"', 4),
('Набор "8 Марта"', 4),
('Набор "23 Февраля"', 4),
('Набор "Выпускной"', 4),
('Лента для шаров золотая', 5),
('Лента для шаров серебряная', 5),
('Гелий баллон 5л', 5),
('Гелий баллон 10л', 5),
('Насос для шаров ручной', 5),
('Гирлянда из шаров', 5),
('Свадебные шары белые', 6),
('Свадебные шары золотые', 6),
('Свадебные шары с надписью', 6),
('Шары "Миньоны"', 7),
('Шары "Щенячий патруль"', 7),
('Шары "Фиксики"', 7),
('Шары "Маша и Медведь"', 7),
('Новогодний шар "Снеговик"', 8),
('Новогодний шар "Дед Мороз"', 8),
('Новогодний шар "Снегурочка"', 8);
 
-- 4. Затем пользователи
INSERT IGNORE INTO users (login, password, role_id, order_id, fio) VALUES
('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 1, NULL, 'Петров Иван Сергеевич'),
('manager', '6ee4a469cd4e91053847f5d3fcb61dbcc91e8f0ef10be7748da4c4a1ba382d17', 2, NULL, 'Сидорова Анна Дмитриевна'),
('seller', 'a4279eae47aaa7417da62434795a011ccb0ec870f7f56646d181b5500a892a9a', 3, NULL, 'Козлов Михаил Петрович');
 
-- 5. Затем заказы
INSERT IGNORE INTO orders (status_id, client_id, date_of_creation, date_of_completion, product_id, discount) VALUES
(1, 1, '2026-01-15', NULL, 1, '5%'),
(2, 2, '2026-01-16', '2026-01-18', 5, '0%'),
(3, 3, '2026-01-17', NULL, 12, '10%'),
(1, 4, '2026-01-18', NULL, 8, '0%'),
(2, 5, '2026-01-19', '2026-01-20', 25, '15%'),
(1, 6, '2026-01-20', NULL, 30, '0%'),
(3, 7, '2026-01-21', NULL, 42, '5%'),
(2, 8, '2026-01-22', '2026-01-23', 18, '0%'),
(1, 9, '2026-01-23', NULL, 35, '20%'),
(2, 10, '2026-01-24', '2026-01-25', 50, '0%'),
(1, 11, '2026-01-25', NULL, 15, '10%'),
(2, 12, '2026-01-26', '2026-01-27', 28, '0%'),
(3, 13, '2026-01-27', NULL, 40, '5%'),
(1, 14, '2026-01-28', NULL, 22, '0%'),
(2, 15, '2026-01-29', '2026-01-30', 45, '15%');
 
-- 6. Затем отчеты
INSERT IGNORE INTO reports (id, param, user_id) VALUES
('report_2026_01', 'sales_january', 1),
('report_2026_02', 'sales_february', 2),
('report_stock', 'stock_info', 1),
('report_clients', 'clients_info', 2),
('report_finance', 'finance_info', 1);
 
-- Добавляем недостающие поля в таблицу products (без условных операторов)
ALTER TABLE products 
ADD COLUMN article VARCHAR(50) DEFAULT '' AFTER name,
ADD COLUMN price DECIMAL(10,2) DEFAULT 0 AFTER article,
ADD COLUMN description TEXT AFTER price,
ADD COLUMN image LONGBLOB AFTER description;
 
-- Добавляем недостающие поля в таблицу clients
ALTER TABLE clients
ADD COLUMN email VARCHAR(100) AFTER phone,
ADD COLUMN address TEXT AFTER email;
 
-- Добавляем недостающие поля в таблицу orders
ALTER TABLE orders
ADD COLUMN total_amount DECIMAL(10,2) DEFAULT 0 AFTER discount,
ADD COLUMN final_amount DECIMAL(10,2) DEFAULT 0 AFTER total_amount,
ADD COLUMN notes TEXT AFTER final_amount;
 
-- Создаем таблицу для элементов заказа
CREATE TABLE IF NOT EXISTS order_items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT,
    product_id INT,
    quantity INT DEFAULT 1,
    price DECIMAL(10,2),
    total DECIMAL(10,2),
    FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(id),
    INDEX (order_id),
    INDEX (product_id)
);
 
-- Добавляем колонку для номера заказа
ALTER TABLE orders ADD COLUMN order_number VARCHAR(20) AFTER id;
 
-- Добавляем поле для описания в справочники
ALTER TABLE categories ADD COLUMN description TEXT AFTER name;
ALTER TABLE statuses ADD COLUMN description TEXT AFTER name;
ALTER TABLE roles ADD COLUMN description TEXT AFTER name;
 
-- ВРЕМЕННО отключаем безопасный режим для обновлений
SET SQL_SAFE_UPDATES = 0;
 
-- Обновляем номера заказов
UPDATE orders SET order_number = LPAD(id, 6, '0') WHERE id IS NOT NULL;
 
-- ИСПРАВЛЕНО: Генерируем последовательные цифровые артикулы (6 цифр)
-- Артикулы будут идти по порядку: 000001, 000002, 000003, ... 000054
UPDATE products SET 
    article = LPAD(id, 6, '0'),
    price = ROUND(RAND() * 1000 + 50, 2),
    description = CONCAT('Описание товара ', name)
WHERE id IS NOT NULL;
 
-- Обновляем пароли пользователей
UPDATE users SET password = 
CASE 
    WHEN login = 'admin' THEN '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918'
    WHEN login = 'manager' THEN '6ee4a469cd4e91053847f5d3fcb61dbcc91e8f0ef10be7748da4c4a1ba382d17'
    WHEN login = 'seller' THEN 'a4279eae47aaa7417da62434795a011ccb0ec870f7f56646d181b5500a892a9a'
END
WHERE login IN ('admin', 'manager', 'seller');
 
-- Обновляем заказы с суммами
UPDATE orders o
JOIN products p ON o.product_id = p.id
SET 
    o.total_amount = p.price,
    o.final_amount = 
        CASE 
            WHEN o.discount LIKE '%5%' THEN p.price * 0.95
            WHEN o.discount LIKE '%10%' THEN p.price * 0.90
            WHEN o.discount LIKE '%15%' THEN p.price * 0.85
            WHEN o.discount LIKE '%20%' THEN p.price * 0.80
            ELSE p.price
        END
WHERE o.id IS NOT NULL;
 
-- Вставляем данные в order_items
INSERT INTO order_items (order_id, product_id, quantity, price, total)
SELECT o.id, o.product_id, 1, p.price, o.final_amount
FROM orders o
JOIN products p ON o.product_id = p.id
WHERE o.id IS NOT NULL;
 
-- Включаем безопасный режим обратно
SET SQL_SAFE_UPDATES = 1;
 
-- Проверяем данные
SELECT 'Таблицы созданы успешно!' as Status;
 
SELECT COUNT(*) as 'Всего клиентов' FROM clients;
SELECT COUNT(*) as 'Всего товаров' FROM products;
SELECT COUNT(*) as 'Всего заказов' FROM orders;
SELECT COUNT(*) as 'Всего пользователей' FROM users;
 
-- Показываем пример данных с артикулами
SELECT 
    o.order_number,
    c.fio as Клиент,
    p.name as Товар,
    p.article as Артикул,
    o.date_of_creation,
    o.total_amount as Сумма,
    o.discount as Скидка,
    o.final_amount as Итог,
    s.name as Статус
FROM orders o
JOIN clients c ON o.client_id = c.id
JOIN products p ON o.product_id = p.id
JOIN statuses s ON o.status_id = s.id
ORDER BY o.date_of_creation DESC
LIMIT 5;
 
-- Показываем все товары с их артикулами для проверки последовательности
SELECT 'Все товары с артикулами (проверка последовательности):' as Info;
SELECT id, name, article, price 
FROM products 
ORDER BY id;
 
-- Проверяем, что артикулы содержат только цифры
SELECT 'Проверка формата артикулов (только цифры):' as CheckMessage;
SELECT article, name 
FROM products 
WHERE article REGEXP '[^0-9]' 
LIMIT 10;