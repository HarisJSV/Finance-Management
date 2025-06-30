CREATE TABLE USERS(
user_id INTEGER PRIMARY KEY IDENTITY(1,1),
username VARCHAR(40) NOT NULL,
password varchar(30) NOT NULL,
email varchar(50) UNIQUE
);

CREATE TABLE ExpenseCategories(
category_id INT PRIMARY KEY IDENTITY(1,1),
category_name VARCHAR(30) UNIQUE
)

CREATE TABLE EXPENSES(
expense_id INTEGER PRIMARY KEY IDENTITY(1,1),
user_id INTEGER,
amount INTEGER NOT NULL,
category_id INT NOT NULL,
date DATE NOT NULL,
description VARCHAR(100),
FOREIGN KEY (user_id) REFERENCES USERS(user_id),
FOREIGN KEY(category_id) REFERENCES ExpenseCategories(category_id)
);


