-- Делаем для MS SQL Server :) я плохо знаю Postgres :)

-- Создаем БД
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'VirtualShop')
BEGIN
    CREATE DATABASE VirtualShop
    COLLATE Cyrillic_General_100_CI_AS_SC_UTF8;
END
GO

-- USE
USE VirtualShop;
GO

-- Продукты
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10, 2) NOT NULL,
    QuantityInStock INT NOT NULL
);
GO

-- Пользователи
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    MiddleName NVARCHAR(100) NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    RegistrationDate DATETIME DEFAULT GETDATE()
);
GO

-- Статусы заказов
CREATE TABLE OrderStatuses (
    StatusID INT IDENTITY(1,1) PRIMARY KEY,
    StatusName NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- Добавление статусов заказов
INSERT INTO OrderStatuses (StatusID, StatusName) VALUES
(1, 'Доставлен'),
(2, 'В обработке'),
(3, 'Отправлен'),
(4, 'Отменен'),
(5, 'Ожидает оплаты');

-- Заказы
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT,
    OrderDate DATETIME DEFAULT GETDATE(),
    StatusID INT,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (StatusID) REFERENCES OrderStatuses(StatusID)
);
GO

-- То из чего состоит заказ
CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT,
    ProductID INT,
    Quantity INT NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO

-- Т.к. TotalCost множители находятся в разных таблицах мы не можем использовать computed column.
-- В таком случае создадим вьюшку с расчетом общей стоимости
CREATE VIEW OrderDetailsWithCost AS
SELECT od.OrderDetailID, od.OrderID, od.ProductID, od.Quantity,
       p.Price AS UnitPrice,
       od.Quantity * p.Price AS TotalCost
FROM OrderDetails od
JOIN Products p ON od.ProductID = p.ProductID;
GO


-- Заполнение базы данных тестовыми данными

-- Добавление пользователей
INSERT INTO Users (FirstName, MiddleName, LastName, Email) VALUES
('Иван', 'Антонович', 'Петров', 'ivan@example.com'),
('Мария', 'Андреевна', 'Сидорова', 'maria@example.com'),
('Алексей', 'Викторович', 'Иванов', 'alex@example.com');


-- Добавление продуктов (20 шт)
INSERT INTO Products (ProductName, Description, Price, QuantityInStock) VALUES
('Смартфон XS-100', 'Современный смартфон с большим экраном', 24999.99, 15),
('Ноутбук ProBook', 'Мощный ноутбук для работы и игр', 59999.99, 7),
('Беспроводные наушники', 'Наушники с шумоподавлением', 3999.99, 25),
('Умные часы', 'Часы с функцией мониторинга здоровья', 8999.99, 12),
('Фотокамера HD', 'Профессиональная камера с высоким разрешением', 45999.99, 4),
('Планшет Lite', 'Компактный планшет для повседневных задач', 12999.99, 10),
('Электрочайник', 'Чайник с регулировкой температуры', 1599.99, 18),
('Кофемашина', 'Автоматическая кофемашина для дома', 18999.99, 6),
('Блендер', 'Мощный блендер для смузи и коктейлей', 2999.99, 14),
('Блендер', 'Мощный блендер для смузи и коктейлей. Но со скидкой', 1999.99, 14),
('Микроволновая печь', 'Печь с грилем и конвекцией', 7499.99, 8),
('Игровая консоль', 'Консоль последнего поколения', 39999.99, 5),
('Умная лампа', 'Лампа с регулировкой яркости и цвета', 999.99, 30),
('Роутер WiFi', 'Высокоскоростной роутер с большим радиусом действия', 3499.99, 17),
('Внешний жесткий диск', 'Портативный диск 2ТБ', 4999.99, 22),
('Фитнес-браслет', 'Браслет для отслеживания активности', 2499.99, 19),
('Портативная колонка', 'Водонепроницаемая колонка с мощным звуком', 3799.99, 11),
('Электробритва', 'Бритва с тройным лезвием', 4299.99, 9),
('Паровой утюг', 'Утюг с функцией вертикального отпаривания', 2799.99, 13),
('Пылесос', 'Беспроводной пылесос с HEPA-фильтром', 11999.99, 7),
('Кухонный комбайн', 'Многофункциональный комбайн с насадками', 8499.99, 3);



-- Добавление заказов (5 шт)
INSERT INTO Orders (UserID, StatusID) VALUES
(1, 1), -- Доставлен
(2, 2), -- В обработке
(3, 3), -- Отправлен
(1, 4), -- Отменен
(2, 1); -- Доставлен

-- Добавление деталей заказов (5 шт)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity) VALUES
(1, 3, 2),
(1, 5, 1),
(2, 10, 1),
(3, 8, 1),
(4, 20, 1);





-- SQL запросы из ДЗ

-- 1. Добавление нового продукта
INSERT INTO Products (ProductName, Description, Price, QuantityInStock) 
VALUES ('Pooker','Профессиональная пукательная штука', 100, 5);

-- 2. Обновление цены продукта
UPDATE Products 
SET Price = 1.00 
WHERE ProductID = 1;

-- 3. Выбор всех заказов определенного пользователя
SELECT o.*, s.StatusName 
FROM Orders o
JOIN OrderStatuses s ON o.StatusID = s.StatusID
WHERE UserID = 1;

-- 4. Расчет общей стоимости заказа
SELECT OrderId, SUM(TotalCost) AS 'Общая стоимость заказа' 
FROM OrderDetailsWithCost 
GROUP BY OrderID

-- 5. Подсчет количества товаров на складе
SELECT SUM(QuantityInStock) AS 'Все товары на складе' 
FROM Products;

-- 6. Получение 5 самых дорогих товаров
SELECT TOP 5 *
FROM Products 
ORDER BY Price DESC;

-- 7. Список товаров с низким запасом (менее 5 штук)
SELECT *
FROM Products 
WHERE QuantityInStock < 5
ORDER BY QuantityInStock ASC;