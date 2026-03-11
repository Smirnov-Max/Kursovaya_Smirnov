-- Создание базы данных (если не существует)
CREATE DATABASE IF NOT EXISTS db102;
USE db102;

-- Основные таблицы (все поля включены сразу)
CREATE TABLE IF NOT EXISTS roles (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    INDEX (name)
);

CREATE TABLE IF NOT EXISTS categories (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    INDEX (name)
);

CREATE TABLE IF NOT EXISTS statuses (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    INDEX (name)
);

CREATE TABLE IF NOT EXISTS products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    article VARCHAR(50) DEFAULT '',
    price DECIMAL(10,2) DEFAULT 0,
    description TEXT,
    image LONGBLOB,
    category_id INT,
    INDEX (category_id),
    FOREIGN KEY (category_id) REFERENCES categories(id)
);

CREATE TABLE IF NOT EXISTS clients (
    id INT AUTO_INCREMENT PRIMARY KEY,
    phone VARCHAR(255) NOT NULL,
    email VARCHAR(100),
    fio VARCHAR(100) NOT NULL,
    address TEXT,
    INDEX (phone)
);

CREATE TABLE IF NOT EXISTS orders (
    id INT AUTO_INCREMENT PRIMARY KEY,
    order_number VARCHAR(20),
    status_id INT,
    client_id INT,
    date_of_creation DATE,
    date_of_completion DATE,
    product_id INT,
    discount VARCHAR(45),
    total_amount DECIMAL(10,2) DEFAULT 0,
    final_amount DECIMAL(10,2) DEFAULT 0,
    notes TEXT,
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

-- Таблица элементов заказа (связь многие-ко-многим)
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